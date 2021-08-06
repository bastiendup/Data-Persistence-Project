using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public string playerName;
    public int highScore;
    public string highScorePlayer;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);


        LoadHighScore();
    }

    [System.Serializable]
    class HighScoreData
    {
        public string highScorePlayer;
        public int highScore;
    }

    public void SaveHighscore()
    {
        HighScoreData data = new HighScoreData();
        data.highScorePlayer = playerName;
        data.highScore = highScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {

            string json = File.ReadAllText(path);

            HighScoreData data = JsonUtility.FromJson<HighScoreData>(json);
            highScorePlayer = data.highScorePlayer;
            highScore = data.highScore;

            Debug.Log(playerName);
            Debug.Log(highScore);
        }

    }

    public void ClearHighScore()
    {
        HighScoreData data = new HighScoreData();
        data.highScorePlayer = "/";
        data.highScore = 0;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void SetHighScore(string name, int score)
    {
        Debug.Log("New highscore set : " + name + ", " + score);
        highScorePlayer = name;
        highScore = score;
        SaveHighscore();
    }

    
}
