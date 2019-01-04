using UnityEngine;
using UnityEngine.SceneManagement;

/* 
 * Base class for Manager objects
 * Defines a constant containg a default log message for if a manager does need have any references to set
 * Declares methods for initialising the manager, responding to a scene load, and setting references
 */
public abstract class Manager : MonoBehaviour
{
    /* Default log message for if a manager has no references to set
     * Should be prefixed with the name of the manager to faciliate debugging
     */
    protected const string NO_REFERENCES_MESSAGE = "No references";

    /* Contains initialisation code for a manager
     * Should be called inside Awake
     */
    protected abstract void InitManager();

    /* Method compatible with SceneManager.sceneLoad event
     * Enables managers to respond to a scene being loaded, for example by reacquiring references to objects in the scene  
     */
    protected abstract void NewLevelLoaded(Scene _scene, LoadSceneMode _mode);

    /* Contains code to get references to objects the manager depends on
     * Should be called inside NewLevelLoaded
     */
    protected abstract void SetReferences();
}