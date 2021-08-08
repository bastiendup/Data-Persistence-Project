using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public string playerName;
    public int highScore;
    public string highScorePlayer;

    [SerializeField]
    private int maxScoreboardEntries = 10;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        UpdateScore();
    }

    [System.Serializable]
    public struct PlayerData
    {
        public string name;
        public int score;
    }

    [System.Serializable]
    public class SavePlayerData
    {
        public List<PlayerData> highscoresList = new List<PlayerData>();
    }

    private SavePlayerData GetSavedScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SavePlayerData data = JsonUtility.FromJson<SavePlayerData>(json);

            return data;
        }
        else
            return null;
    }

    private void SaveScores(SavePlayerData savePlayerData)
    {
        string json = JsonUtility.ToJson(savePlayerData);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void UpdateScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            SavePlayerData data = JsonUtility.FromJson<SavePlayerData>(json);
            highScorePlayer = data.highscoresList[0].name;
            highScore = data.highscoresList[0].score;
        }

    }

    public void AddDataFromGame(string name, int score)
    {
        //Check if it's the first player to play
        SavePlayerData savePlayerData = GetSavedScore();

        if (savePlayerData.highscoresList[0].name == "The Game Creator")
        {
            savePlayerData.highscoresList.Remove(savePlayerData.highscoresList[0]);
            SaveScores(savePlayerData);
        }

        PlayerData playerData = new PlayerData();
        playerData.name = name;
        playerData.score = score;
        AddEntry(playerData);

        //Update the highscore player/score
        UpdateScore();
    }

    private void AddEntry(PlayerData playerData)
    {
        SavePlayerData savePlayerData = GetSavedScore();

        if (savePlayerData == null)
            savePlayerData = new SavePlayerData();

        bool scoreAdded = false;

        for (int i = 0; i < savePlayerData.highscoresList.Count; i++)
        {
            if (playerData.score > savePlayerData.highscoresList[i].score)
            {
                savePlayerData.highscoresList.Insert(i, playerData);
                scoreAdded = true;
                break;
            }
        }
        //If the score hasn't been added && there is some place left in the scoreboard
        if (!scoreAdded && savePlayerData.highscoresList.Count < maxScoreboardEntries)
            savePlayerData.highscoresList.Add(playerData);

        if (savePlayerData.highscoresList.Count > maxScoreboardEntries)
            savePlayerData.highscoresList.RemoveRange(maxScoreboardEntries, savePlayerData.highscoresList.Count - maxScoreboardEntries);

        SaveScores(savePlayerData);
    }

    public void DisplayHighscore(Transform highscoresHolder, GameObject entryTextPrefab)
    {
        foreach (Transform child in highscoresHolder)
            Destroy(child.gameObject);

        SavePlayerData savePlayerData = GetSavedScore();
        var highScoreList = savePlayerData.highscoresList;

        //Check if highScoreList is empty, if so create new best player named The Game Creator;
        if (highScoreList.Count == 0)
        {
            PlayerData playerData = new PlayerData();
            playerData.name = "The Game Creator";
            playerData.score = 9999;
            AddEntry(playerData);

            var obj = Instantiate(entryTextPrefab, highscoresHolder);
            obj.GetComponent<ScoreEntryUI>().Initialise(0, playerData);
        }

        else
        {
            for (int i = 0; i < highScoreList.Count; i++)
            {
                var obj = Instantiate(entryTextPrefab, highscoresHolder);
                obj.GetComponent<ScoreEntryUI>().Initialise(i, highScoreList[i]);
            }
        }
    }
    public void ClearHighscores()
    {
        SavePlayerData savePlayerData = GetSavedScore();
        savePlayerData.highscoresList.RemoveRange(0, savePlayerData.highscoresList.Count);

        SaveScores(savePlayerData);
    }

}
