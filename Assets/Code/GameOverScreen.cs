using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour 
{
    private int m_lastLevelIndex = 0;

    public void Awake()
    {
        m_lastLevelIndex = GameManager.Instance.LastLevelIndex;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartLevel()
    {
        if (m_lastLevelIndex > 0)
        {
            Debug.Log("Loading previous level: " + m_lastLevelIndex);

            SceneManager.LoadScene(m_lastLevelIndex);
        }
        else
        {
            Debug.Log("Previous level index not set");
        }
    }
}
