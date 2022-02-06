public class StartPanel : Panel
{
    public void Start()
    {
        OnStartPanel();
    }
    protected override void OnStartPanel()
    {
        base.OnStartPanel();
        primaryButton.onClick.AddListener(OnClickStartButton);
        SoundManager.Instance.PlayMusic();
    
    }

    public override void Open()
    {
        base.Open();
    }

    public void OnClickStartButton()
    {
        GameManager.Instance.StartGame();
        Close();
        SoundManager.Instance.StopMusic();
    }
}