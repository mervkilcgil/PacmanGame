public class GameManager
{
    private static GameManager instance;
    private GameState gameState;
    private int gameScore;

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
    

    public void IncreaseScore()
    {
        gameScore++;
    }
}

public enum GameState
{
    Playing,
    GameOver
}