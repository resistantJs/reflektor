using UnityEngine;

/// <summary>
/// Class definining the behaviour of the LevelSelect menu
/// This menu appears in the MainMenu scene
/// </summary>
public class LevelSelect : MonoBehaviour
{
    /// <summary>
    /// Loads the selected level
    /// The build index passed is determined by which button is pressed on the LevelSelect menu
    /// For example, pressing the Level One button will call this method with an argument of 1, the build index of Level1
    /// </summary>
    /// <param name="_buildIndex"></param>
    public void LoadSelectedLevel(int _buildIndex)
    {
        /// Calls the LoadSelectLevel method of the LevelManager, which loads the selected level without a delay
        LevelManager.Instance.LoadSelectedLevel(_buildIndex);
    }
}
