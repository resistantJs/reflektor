using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour 
{
    private static int m_lastLevelIndex = 0;

    public static void SetLastLevelIndex(int _index)
    {
        if (_index > 0 && _index < SceneManager.sceneCountInBuildSettings)
        {
            m_lastLevelIndex = _index;
        }
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
