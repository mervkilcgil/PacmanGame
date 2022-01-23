using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPanel : Panel
{
    [SerializeField] private TextMeshProUGUI scoreText;
    public void Start()
    {
        OnStartPanel();
    }
    protected override void OnStartPanel()
    {
        base.OnStartPanel();
        primaryButton.onClick.AddListener(OnClickExitButton);
        GameManager.Instance.OnWinGame += Open;
    }
    public override void Open()
    {
        base.Open();
        scoreText.text = "WIN";
        
    }

    private void OnClickExitButton()
    {
        Close();
        StartCoroutine(nameof(SceneLoad));
    }

    IEnumerator SceneLoad()
    {
        yield return new WaitForSecondsRealtime(1f);
        Application.Quit();
    }
}