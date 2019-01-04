using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manager responsible for all visual effects, such as screenshake
/// Contains methods for triggering visual effects
/// All managers implement the singleton pattern to prevent the existance of multiple instances of the same manager in the scene
/// </summary>
public class EffectsManager : Manager
{
    /// <summary>
    /// Singleton instance for the manager
    /// Initialised inside InitManager
    /// </summary>
    private static EffectsManager m_instance = null;
    /// <summary>
    /// Reference to the current scene's main camera
    /// Enables effects that utilise the camera, such as screenshake
    /// Initialised inside InitManager
    /// </summary>
    private Camera m_mainCamera = null;

    /// <summary>
    /// Subscribes to relevant events
    /// </summary>
    private void OnEnable()
    {
        /// Subscribes to sceneLoad event to enable the manager to respond to scene changes
        SceneManager.sceneLoaded += NewLevelLoaded;
    }

    /// <summary>
    /// Unsubscribes from events when the Manager is disabled
    /// </summary>
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= NewLevelLoaded;
    }

    /// <summary>
    /// Calls initialisation methods for the manager
    /// </summary>
    private void Awake()
    {
        InitManager();
        SetReferences();
    }

    /// <summary>
    /// Property to access the manager's singleton instance outside this class
    /// Can be set privately only
    /// </summary>
    /// <value>Publically gets and privately sets the value of the class's singleton instance, m_instance</value>
    public static EffectsManager Instance
    {
        get
        {
            return m_instance;
        }

        private set
        {
            m_instance = value;
        }
    }

    /// <summary>
    /// Provides public access to the Shake coroutine, which actually produces the screen shake effect
    /// </summary>
    /// <param name="_duration">The duration of the screen shake effect</param>
    /// <param name="_magnitude">The initial magnitude of the shakes</param>
    /// <param name="_reductionMultiplier">Mutliplier for the amount of maginitude reduction applied to each shake. Defaults to 1x</param>
    public void ShakeScreen(float _duration, float _magnitude, float _reductionMultiplier = 1)
    {
        /// Shakes the screen if the reference to the Main Camera is not null
        if (m_mainCamera != null)
        {
            /// Starts the Shake coroutine using the specified values
            StartCoroutine(Shake(_duration, _magnitude, _reductionMultiplier));
        }
        /// Logs a message if the Main Camera reference is null
        else
        {
            Debug.Log("No Main Camera in scene");
        }
    }

    /// <summary>
    /// Creates a screen shake effect by adjusted the X and Y position of the scene's main camera by random amounts each frame for the duration of the effect
    /// </summary>
    /// <param name="_duration">The duration of the screen shake effect</param>
    /// <param name="_magnitude">The initial magnitude of the shakes</param>
    /// <param name="_reductionMultiplier">Mutliplier for the amount of maginitude reduction applied to each shake. Defaults to 1x</param>
    /// <returns></returns>
    private IEnumerator Shake(float _duration, float _magnitude, float _reductionMultiplier)
    {
        /// The postion of the Main Camera prior to starting the effect
        Vector3 _originalPos = m_mainCamera.transform.localPosition;
        /// Stores how long the effect has been occuring
        float _elapsedTime = 0.0f;
        /// The amount of reduction applied to the magnitude of each shake. Effected by the passed in reduction multiplier
        float _shakeReduction = _magnitude / _duration * _reductionMultiplier;

        /// Loops until the effect has run for the specified duration, shaking the screen each cycle
        while (_elapsedTime < _duration)
        {
            /// Generates a randomised X and Y positions for the main camera this cycle
            /// Positions are scaled by specified magnitude
            float _x = Random.Range(-1f, 1f) * _magnitude;
            float _z = Random.Range(-1f, 1f) * _magnitude;

            /// Updates the main camera's position with the newly generated values
            m_mainCamera.transform.localPosition = new Vector3(_x, _originalPos.y, _z);

            /// Adds to elapsed time since the effect began
            _elapsedTime += Time.deltaTime;

            /// Clamps the value after magnitude reduction has been applied to ensure effect ends, instead of applying negative position offsets
            if (_magnitude - _shakeReduction * Time.deltaTime < 0)
            {
                _magnitude = 0;
            }
            else
            {
                _magnitude -= _shakeReduction * Time.deltaTime;
            }

            /// Delays the next cycle of the loop until the next frame in order to make the effect visible
            yield return null;
        }

        /// Resets the main camera to its original position when the effect ends
        m_mainCamera.transform.localPosition = _originalPos;
    }

    /// <summary>
    /// Initialises the singleton instance of the manager and sets it not be destroyed when a new scene is loaded
    /// Contains other initialisation code for a manager
    /// </summary>
    protected override void InitManager()
    {
        /// Instantiates the instance if has not been already
        if (Instance == null)
        {
            Instance = this;
        }
        /// If an instance already exists, destroys this one
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        /// Sets this instance to be not be destroyed when a new scene is loaded
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Contains code to get references to objects the manager depends on
    /// Should be called inside NewLevelLoaded
    /// </summary>s
    protected override void SetReferences()
    {
        Debug.Log("EffectsManager: Setting References");
        /// Gets a reference to the Main Camera in the scene
        m_mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    /// <summary>
    /// Method compatible with SceneManager.sceneLoad event
    /// Enables managers to respond to a scene being loaded, for example by reacquiring references to objects in the scene
    /// </summary>
    /// <param name="_scene">Scene reference</param>
    /// <param name="_mode">LoadSceneMode reference</param>
    protected override void NewLevelLoaded(Scene _scene, LoadSceneMode _mode)
    {
        SetReferences();
    }
}
