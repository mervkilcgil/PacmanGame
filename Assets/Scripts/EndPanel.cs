using TMPro;
using UnityEngine;

public class EndPanel : Panel
{
    [SerializeField] private TextMeshProUGUI scoreText;
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
        scoreText.text = "SCORE\n" + GameManager.Instance.Score.ToString();
        
    }

    private void OnClickStartButton()
    {
        GameManager.Instance.StartGame();
        Close();
    }
}