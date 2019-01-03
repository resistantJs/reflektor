using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    public void LoadSelectedLevel(int _buildIndex)
    {
        LevelManager.Instance.LoadSelectedLevel(_buildIndex);
    }
}
