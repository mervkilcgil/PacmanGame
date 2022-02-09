using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreArea : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private List<GameObject> lifeObjects;
    
    private static ScoreArea instance;
    
    public static ScoreArea Instance => instance;
    public void Awake()
    {
        instance = this;
    }
    
    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
    }
    
    public void SetHighScore(int highScore)
    {
        highScoreText.text = highScore.ToString();
    }
    
    public void SetLives(int lives)
    {
        for (int i = 0; i < lifeObjects.Count; i++)
        {
            if (i < lives)
            {
                lifeObjects[i].SetActive(true);
            }
            else
            {
                lifeObjects[i].SetActive(false);
            }
        }
    }
}