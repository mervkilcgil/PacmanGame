public class EndPanel : Panel
{
    public void Start()
    {
        OnStartPanel();
    }
    protected override void OnStartPanel()
    {
        base.OnStartPanel();
        primaryButton.onClick.AddListener(OnClickStartButton);
        GameManager.Instance.OnEndGame += Open;
    }
    public override void Open()
    {
        base.Open();
        
    }

    private void OnClickStartButton()
    {
        GameManager.Instance.StartGame();
        Close();
    }
}