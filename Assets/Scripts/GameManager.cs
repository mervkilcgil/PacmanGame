using System;
using UnityEngine.SceneManagement;

public class GameManager
{
    private static GameManager instance;
    private GameState gameState;
    private int gameScore;
    public Action OnStartGame;
    public Action OnEndGame;
    public Action OnWinGame;
    private int dotCount;
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
    }

    public void StartGame()
    {
        gameState = GameState.Playing;
        gameScore = 0;
        OnStartGame?.Invoke();
    }


    public void IncreaseScore(int score = 1)
    {
        gameScore += score;
    }

    public void GameOver()
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