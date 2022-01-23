using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        primaryButton.onClick.AddListener(OnClickExitButton);
        GameManager.Instance.OnEndGame += Open;
    }
    public override void Open()
    {
        base.Open();
        scoreText.text = "SCORE\n" + GameManager.Instance.Score.ToString();
        
    }

    private void OnClickExitButton()
    {
        Close();
        StartCoroutine(nameof(SceneLoad));
    }

    IEnumerator SceneLoad()
    {
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene("Game");
    }
}