using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Manager : MonoBehaviour
{
    protected const string NO_REFERENCES_MESSAGE = "No references";

    protected abstract void InitManager();

    protected abstract void NewLevelLoaded(Scene _scene, LoadSceneMode _mode);

    protected abstract void SetReferences();
}