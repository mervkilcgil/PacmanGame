using System;
using UnityEngine.SceneManagement;

public class GameManager
{
    private static GameManager instance;
    private GameState gameState;
    private int gameScore;
    public Action OnStartGame;
    public Action OnEndGame;
    
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
            if(instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }

    public void StartGame()
    {
        gameState = GameState.Playing;
        gameScore = 0;
        OnStartGame?.Invoke();
    }
    

    public void IncreaseScore()
    {
        gameScore++;
    }

    public void GameOver()
    {
        gameState = GameState.GameOver;
        OnEndGame?.Invoke();
        //SceneManager.LoadScene("Game");
    }
}

public enum GameState
{
    Playing,
    GameOver
}