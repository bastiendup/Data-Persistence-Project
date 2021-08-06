using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI highScoreText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        DisplayHighscore(false);

    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void LateUpdate()
    {
        if (m_Points > DataManager.Instance.highScore)
        {
            DataManager.Instance.SetHighScore(DataManager.Instance.playerName, m_Points);
            DisplayHighscore(true);
        }

    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Current Score : [ {m_Points} ]";
        // DataManager.Instance.playerScore = m_Points;
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        //Check if the current score is the new highscore
        //If so set new highscore data
        if (m_Points > DataManager.Instance.highScore)
        {
            DataManager.Instance.SetHighScore(DataManager.Instance.playerName, m_Points);
            DisplayHighscore(false);
        }
    }

    public void DisplayHighscore(bool isNewHighscore)
    {
        var highScorePlayer = DataManager.Instance.highScorePlayer;
        var highScore = DataManager.Instance.highScore;
        if (isNewHighscore)
            highScoreText.text = $"New Best Score : {highScorePlayer} [ {highScore} ]";
        else
            highScoreText.text = $"Best Score : {highScorePlayer} [ {highScore} ]";
    }
}

