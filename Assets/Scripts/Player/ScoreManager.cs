using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Xml.Serialization;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;

    [SerializeField] private int score = 0;
    
    void Start()
    {
        scoreText.text = "SCORE  : " + score.ToString();
    }

    void Update()
    {
    }
    
    public void AddScore(int scoreAmount)
    {
        score = score += scoreAmount;
        scoreText.text = "SCORE  : " + score.ToString();

    }
}
