using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Manager : MonoBehaviour
{
    protected abstract void SetInstance();

    protected abstract void SetReferences();

    protected abstract void UpdateReferences(Scene _scene, LoadSceneMode _mode);
}
