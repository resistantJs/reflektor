using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Manager : MonoBehaviour
{
    protected abstract void InitManager();

    protected abstract void NewLevelLoaded(Scene _scene, LoadSceneMode _mode);

    protected abstract void SetReferences();
}