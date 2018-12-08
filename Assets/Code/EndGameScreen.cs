using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour
{
    private Text m_txtFinalScoreReadout;

    private void Awake()
    {
        m_txtFinalScoreReadout = GameObject.Find("txtFinalScoreReadout").GetComponent<Text>();
    }

    private void Start()
    {
        Debug.Log("The final score");
        m_txtFinalScoreReadout.text = ScoreManager.Instance.TotalScore.ToString();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
