using UnityEngine;

public class StartPanel : Panel
{
    [SerializeField]private SoundManager soundManager;
    public void Start()
    {
        OnStartPanel();
    }
    protected override void OnStartPanel()
    {
        base.OnStartPanel();
        primaryButton.onClick.AddListener(OnClickStartButton);
        soundManager.PlayMusic();
    
    }

    public override void Open()
    {
        base.Open();
        
    }

    public void OnClickStartButton()
    {
        GameManager.Instance.StartGame();
        Close();
    }
}