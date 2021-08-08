using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


#if UNITY_EDITOR
using UnityEditor;
#endif

// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputPlayerName;

    [SerializeField]
    private TextMeshProUGUI highScoreText;

    [SerializeField]
    private Transform highScoreHolder;

    [SerializeField]
    private GameObject scoreEntryPrefab;

    private void Awake()
    {
        //Check if we are in the menu scene (MenuUIHandler also used in the highscore scene)
        if (inputPlayerName != null && highScoreText != null)
        {
            UpdateHighscoreText();
        }

        //Check if we're in the highscore scene
        if (highScoreHolder != null)
            DataManager.Instance.DisplayHighscore(highScoreHolder, scoreEntryPrefab);
    }

    public void StartGame()
    {
        DataManager.Instance.playerName = inputPlayerName.text;
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void Clear()
    {
        DataManager.Instance.ClearHighscores();
        DataManager.Instance.DisplayHighscore(highScoreHolder, scoreEntryPrefab);
        DataManager.Instance.UpdateScore();
    }

    public void OpenHighScores()
    {
        SceneManager.LoadScene(2);
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void UpdateHighscoreText()
    {
        var highScorePlayer = DataManager.Instance.highScorePlayer;
            var highscore = DataManager.Instance.highScore;

            highScoreText.text = $"Top Score : {highScorePlayer} [ {highscore} ]";
    }


}
