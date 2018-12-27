using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScreen : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("MainMenu: Score reset");
        ScoreManager.Instance.ResetScore();
    }

    private void Update()
    {
        if (InputManager.Instance.Quit)
        {
            Application.Quit();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(3);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
