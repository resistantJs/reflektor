using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Base class for Manager objects
/// Defines a constant containg a default log message for if a manager does need have any references to set
/// Declares methods for initialising the manager, responding to a scene load, and setting references
/// </summary>
public abstract class Manager : MonoBehaviour
{
    /// <summary>
    /// Default log message for if a manager has no references to set
    /// Should be prefixed with the name of the manager to faciliate debugging
    /// </summary>
    protected const string NO_REFERENCES_MESSAGE = "No references";

    /// <summary>
    /// Initialises the singleton instance of the manager and sets it not be destroyed when a new scene is loaded
    /// Contains other initialisation code for a manager
    /// Should be called inside Awake
    /// </summary>
    protected abstract void InitManager();

    /// <summary>
    /// Method compatible with SceneManager.sceneLoad event
    /// Enables managers to respond to a scene being loaded, for example by reacquiring references to objects in the scene  
    /// </summary>
    /// <param name="_scene">Scene reference</param>
    /// <param name="_mode">LoadSceneMode reference</param>
    protected abstract void NewLevelLoaded(Scene _scene, LoadSceneMode _mode);

    /// <summary>
    /// Contains code to get references to objects the manager depends on
    /// Should be called inside NewLevelLoaded
    /// </summary>
    protected abstract void SetReferences();
}