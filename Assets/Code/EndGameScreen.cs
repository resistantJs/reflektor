using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Class defining the behaviour of the EndGameScreen menu
/// This menu appears in the EndGame scene, which is loaded after the player clears all the gameplay levels in sequence
/// </summary>
public class EndGameScreen : MonoBehaviour
{
    /// <summary>
    /// Reference to the txtFinalScoreReadout GUI element
    /// Interaction with this element is not handled by the UIManager, as txtFinalScoreReadout appears in this scene only
    /// </summary>
    private Text m_txtFinalScoreReadout = null;

    /// <summary>
    /// Gets the reference to the txtFinalScoreReadout GUI element
    /// </summary>
    private void Awake()
    {
        m_txtFinalScoreReadout = GameObject.Find("txtFinalScoreReadout").GetComponent<Text>();
    }

    /// <summary>
    /// Displays the player's final score on screen when the scene loads
    /// </summary>
    private void Start()
    {
        Debug.Log("The final score");
        /// Accessed the txtFinalScoreReadout element and uses the SceneManager's TotalScore property to set its value to the player's total score at that point, which would be their final score as there are no more opportunities to increase it
        m_txtFinalScoreReadout.text = ScoreManager.Instance.TotalScore.ToString();
    }

    /// <summary>
    /// Loads the Main Menu scene
    /// Called when the player presses the Main Menu button on the EndGameScreen menu 
    /// </summary>
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
