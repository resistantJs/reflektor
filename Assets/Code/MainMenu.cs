using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
