using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreEntryUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameText;
    
    [SerializeField]
    private TextMeshProUGUI scoreText;

    public void Initialise(int index, DataManager.PlayerData data)
    {
        nameText.text = $"<size=120%> {index+1}. <size=100%> {data.name}";
        scoreText.text = $"[ {data.score} ]";
    }
}
