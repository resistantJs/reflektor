using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manager responsible for loading scenes (refered to as levels) and accessing information about them
/// Contains methods for loading levels with or without a delay and accessing information about them
/// All managers implement the singleton pattern to prevent the existance of multiple instances of the same manager in the scene
/// </summary>
public class LevelManager : Manager
{
    /// <summary>
    /// Singleton instance for the manager
    /// Initialised inside InitManager
    /// </summary>
    private static LevelManager m_instance = null;
    /// <summary>
    /// Stores the build index of the previous scene (the one before the current scene was loaded)
    /// </summary>
    private int m_lastLevelIndex = 0;
    /// <summary>
    /// Array storing build indicies of menu scenes (i.e. scenes that do not contain gameplay)
    /// Set in the Inspector
    /// </summary>
    [SerializeField]
    private int[] m_menuLevels;
    /// <summary>
    /// Time to wait (seconds) before loading the next scene in the sequence
    /// </summary>
    [SerializeField]
    private float m_nextLevelDelay = 3.0f;

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
    /// Initialises the singleton instance of the manager and sets it not be destroyed when a new scene is loaded
    /// Contains other initialisation code for a manager
    /// </summary>
    protected override void InitManager()
    {
        /// Instantiates the instance if has not been
        if (m_instance == null)
        {
            m_instance = this;
        }
        /// If an instance already exists, destroys this one
        else if (m_instance != this)
        {
            Destroy(gameObject);
        }

        /// Sets this instance to not be destroyed when a new scene is loaded
        DontDestroyOnLoad(gameObject);
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

    /// <summary>
    /// Contains code to get references to objects the manager depends on
    /// Should be called inside NewLevelLoaded
    /// </summary>s
    protected override void SetReferences()
    {
        Debug.Log("LevelManager: " + NO_REFERENCES_MESSAGE);
    }

    /// <summary>
    /// Loads the specified scene immediately
    /// </summary>
    /// <param name="_buildIndex">The build index of the scene to load</param>
    public void LoadSelectedLevel(int _buildIndex)
    {
        /// Stores build index of the current scene before loading the specified one
        m_lastLevelIndex = SceneManager.GetActiveScene().buildIndex;
        /// Loads the specified scene
        SceneManager.LoadScene(_buildIndex);
    }

    /// <summary>
    /// Returns the build index of the next scene in the sequence.
    /// </summary>
    /// <returns>
    /// Returns the build index of the next scene. Returns the Main Menu's build index if the current scene is the last in the sequence
    /// </returns>
    public int GetNextLevelIndex()
    {
        /// Stores the build index of the next scene (current scene build index + 1)
        int _nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

        /// Checks if the build index of the next is greater than scenes in build settings, returning the build index of the Main Menu if so
        /// Because the first three scenes in the build settings are menus, Level1 has the build index 1
        if (_nextLevelIndex >= SceneManager.sceneCountInBuildSettings)
        {
            return 2;
        }
        /// If the build index of the next scene is not greater than the number of scenes in build settings, returns the build index of the next scene
        else
        {
            return SceneManager.GetActiveScene().buildIndex + 1;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>
    /// Bool indicated whether or not the current scene is marked as a menu
    /// True: the current scene is marked as a menu
    /// False: the current scene is not marked as a menu
    /// </returns>
    public bool OnMenu()
    {
        /// Stores the build index of the current scene
        int _activeSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;

        /// Checks the current scene's build index against the indicies of those marked as menus
        foreach (int element in m_menuLevels)
        {
            /// If the current scene's build index matches that of a scene marked as menu, returns true
            if (element == _activeSceneBuildIndex)
            {
                return true;
            }
        }

        /// If the build index of the current does not match any of those marked as menus, returns false
        return false;
    }

    /// <summary>
    /// Provides public access to the LoadLevelAfterDelay coroutine, which loads a level after a delay
    /// Starts the LoadLevelAfterDelay coroutine, passing in the provided values
    /// </summary>
    /// <param name="_buildIndex">The build index of the scene to be loaded</param>
    /// <param name="_delay">The time to wait (seconds) before loading the specified scene</param>
    public void ChangeLevel(int _buildIndex, float _delay)
    {
        /// Starts the LoadLevelAfterDelay using the specified values
        StartCoroutine(LoadLevelAfterDelay(_buildIndex, _delay));
    }

    /// <summary>
    /// Loads the specified scene after a delay
    /// </summary>
    /// <param name="_buildIndex">The build index of the scene to be loaded</param>
    /// <param name="_delay">The time to wait (seconds) beofre loading the specified scene</param>
    /// <returns></returns>
    private IEnumerator LoadLevelAfterDelay(int _buildIndex, float _delay)
    {
        Debug.Log("Loading level " + _buildIndex + " in " + _delay + " seconds");

        /// Waits the number of seconds specified by _delay before continuing execution
        yield return new WaitForSecondsRealtime(_delay);

        /// Loads the specified scene
        LoadSelectedLevel(_buildIndex);
    }

    /// <summary>
    /// Property to access the manager's singleton instance outside this class
    /// Can be set privately only
    /// </summary>
    /// <value>Publically gets and privately sets the value of the class's singleton instance, m_instance</value>
    public static LevelManager Instance
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
    /// Property to access the default level load delay value
    /// </summary>
    /// <value>Publically gets and privately sets the value of float m_nextLevelDelay</value>
    public float NextLevelDelay
    {
        get
        {
            return m_nextLevelDelay;
        }

        private set
        {
            m_nextLevelDelay = value;
        }
    }

    /// <summary>
    /// Property to access the build index of the previously loaded level
    /// </summary>
    /// <value>Publically gets and privately sets the value of int m_lastLevelIndex</value>
    public int LastLevelIndex
    {
        get
        {
            return m_lastLevelIndex;
        }

        private set
        {
            m_lastLevelIndex = value;
        }
    }
}
