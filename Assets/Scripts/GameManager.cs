using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    private static GameManager instance;
    private GameState gameState;
    private int gameScore;
    public Action OnStartGame;
    public Action OnEndGame;
    public Action OnRestartGame;
    public Action OnWinGame;
    private int dotCount;
    private int eatenGhostCount;
    private int lives;
    private int highScore;
    private Player player;
    public Vector2 PlayerPosition { get { return player.transform.position; } }

    public Player Player { set=> player = value; }
    public int DotCount
    {
        get => dotCount;
        set
        {
            dotCount = value;
            if (dotCount == 0)
            {
                OnWin();
            }
        }
    }

    public GameState GameState
    {
        get => gameState;
        set => gameState = value;
    }

    public int Score => gameScore;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }

            return instance;
        }
    }

    public GameManager()
    {
        gameState = GameState.NotStarted;
        highScore = PlayerPrefs.GetInt("PacManGameHighScore", 0);
    }

    public void StartGame()
    {
        gameState = GameState.Playing;
        gameScore = 0;
        eatenGhostCount = 0;
        lives = 3;
        ScoreArea.Instance.SetHighScore(highScore);
        ScoreArea.Instance.SetScore(gameScore);
        ScoreArea.Instance.SetLives(lives);
        OnStartGame?.Invoke();
    }

    public void EatPellet()
    {
        IncreaseScore(10);
    }

    public void EatPowerPellet()
    {
        IncreaseScore(50);
    }

    public void EatGhost()
    {
        eatenGhostCount++;
        IncreaseScore((int)Math.Pow(2, eatenGhostCount));
    }

    private void IncreaseScore(int score)
    {
        gameScore += score;
        ScoreArea.Instance.SetScore(gameScore);
        if (gameScore >= highScore)
        {
            highScore = gameScore;
            PlayerPrefs.SetInt("PacManGameHighScore", highScore);
            ScoreArea.Instance.SetHighScore(highScore);
        }
        if (score >= 10000)
            IncreaseLife();
    }

    private void IncreaseLife()
    {
        lives++;
        ScoreArea.Instance.SetLives(lives);
    }

    private void DecreaseLife()
    {
        lives--;
        ScoreArea.Instance.SetLives(lives);
        if (lives == 0)
        {
            GameOver();
        }
        else
        {
            OnRestartGame?.Invoke();
        }
    }

    public void OnDeath()
    {
        SoundManager.Instance.PlayDeath();
        DecreaseLife();
    }

    private void GameOver()
    {
        gameState = GameState.GameOver;
        OnEndGame?.Invoke();
        OnStartGame = null;
        OnEndGame = null;
    }

    public void OnWin()
    {
        gameState = GameState.Win;
        OnWinGame?.Invoke();
        OnStartGame = null;
        OnEndGame = null;
    }
}

public enum GameState
{
    NotStarted,
    Playing,
    GameOver,
    Win
}