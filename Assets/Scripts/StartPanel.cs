public class StartPanel : Panel
{
    protected override void OnStartPanel()
    {
        base.OnStartPanel();

    }

    public override void Open()
    {
        base.Open();
        primaryButton.onClick.AddListener(OnClickStartButton);
    }

    private void OnClickStartButton()
    {
        GameManager.Instance.StartGame();
        Close();
    }
}