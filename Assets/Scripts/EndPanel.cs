public class EndPanel : Panel
{
    protected override void OnStartPanel()
    {
        base.OnStartPanel();
        GameManager.Instance.OnEndGame += Open;
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