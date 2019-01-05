using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class defining the behaviour of the GameOverScreen menu
/// </summary>
public class GameOverScreen : MonoBehaviour
{
    /// <summary>
    /// Stores the build index of the previous scene
    /// </summary>
    private int m_lastLevelIndex = 0;

    /// <summary>
    /// Sets the value of m_lastLevelIndex by accessing the LevelManager's LastLevelIndex property
    /// </summary>
    public void Awake()
    {
        m_lastLevelIndex = LevelManager.Instance.LastLevelIndex;
    }

    /// <summary>
    /// Loads the Main Menu scene
    /// Called when the pressed the Main Menu button the GameOverScreen menu
    /// </summary>
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Loads the previous scene, enabling the player to reattempt the level
    /// </summary>
    public void RestartLevel()
    {
        /// Validates the value of m_lastLevelIndex before using it to load a scene
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
