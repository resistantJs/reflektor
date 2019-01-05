using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class defining the behaviour of the MainMenuScreen menu
/// The MainMenuScreen menu appears in the MainMenu scene.
/// </summary>
public class MainMenuScreen : MonoBehaviour
{
    /// <summary>
    /// Resets the player's total score upon arriving at this menu
    /// Returning to the Main Menu completes the full game loop, so the player's score is reset, enabling them to begin another loop anew
    /// </summary>
    private void Start()
    {
        Debug.Log("MainMenu: Score reset");
        ScoreManager.Instance.ResetScore();
    }

    /// <summary>
    /// Quits the game if the player presses the Quit button
    /// </summary>
    private void Update()
    {
        if (InputManager.Instance.Quit)
        {
            Application.Quit();
        }
    }

    /// <summary>
    /// Loads the first level scene (Level1) when the player presses the New Game button on this menu
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene(3);
    }

    /// <summary>
    /// Quits the game when the player presses the Quit Game button on this menu
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
