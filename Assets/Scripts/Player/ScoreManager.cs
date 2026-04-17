using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Xml.Serialization;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public Text scoreText;

    public int Score { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }
    
    void Start()
    {
        scoreText.text = "SCORE  : " + Score.ToString();
    }

    void Update()
    {
    }
    
    public void AddScore(int scoreAmount)
    {
        Score += scoreAmount;
        scoreText.text = "SCORE  : " + Score.ToString();

    }

    public void ResetScore()
    {
        Score = 0;
        scoreText.text = "SCORE  : " + Score.ToString();

    }
}
