using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manager responsible for detecting user input
/// Contains methods for detecting certain types of user input, such as whether specific buttons are pressed or the position of the player's mouse
/// All managers implement the singleton pattern to prevent the existance of multiple instances of the same manager in the scene
/// </summary>
public class InputManager : Manager
{
    /// <summary>
    /// Singleton instance for the manager
    /// Initialised inside InitManager
    /// </summary>
    private static InputManager m_instance = null;
    /// <summary>
    /// Indicates whether or not the player is pressing the Fire button
    /// True: the button is being pressed
    /// False: the button is not being pressed
    /// </summary>
    private bool m_fire = false;
    /// <summary>
    /// Indicates whether or not the player is pressing the Quit button
    /// True: the button is being pressed
    /// False: the button is not being pressed
    /// </summary>
    private bool m_quit = false;
    /// <summary>
    /// Indicates whether or not the player is pressing the Destroy Projectile button
    /// True: the button is being pressed
    /// False: the button is not being pressed
    /// </summary>
    private bool m_destroyProjectile = false;
    /// <summary>
    /// Stores the mouse pointer's current position
    /// </summary>
    private Vector3 m_mousePos = Vector3.zero;

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
    /// Calls SetInputs to respond to the player's input
    /// </summary>
    private void Update()
    {
        SetInputs();
    }

    /// <summary>
    /// Queries the Unity's input system to check if the players is making certain inputs and sets the input variables accordingly
    /// Called in Update loop to ensure responsive input
    /// </summary>
    private void SetInputs()
    {
        /// Gets the position of the player's mouse point
        m_mousePos = Input.mousePosition;

        /// Sets the value of m_fire depending on if the Fire1 button is held down or not
        if (Input.GetButtonDown("Fire1"))
        {
            m_fire = true;
        }
        else
        {
            m_fire = false;
        }

        /// Sets the value of m_destroyProjectile depending on if the Fire2 button is held down or not
        if (Input.GetButtonDown("Fire2"))
        {
            m_destroyProjectile = true;
        }
        else
        {
            m_destroyProjectile = false;
        }

        /// Sets the value of m_quit depending on if the Cancel button is held down or not
        if (Input.GetButtonDown("Cancel"))
        {
            m_quit = true;
        }
        else
        {
            m_quit = false;
        }
    }

    /// <summary>
    /// Initialises the singleton instance of the manager and sets it not be destroyed when a new scene is loaded
    /// Contains other initialisation code for a manager
    /// </summary>
    protected override void InitManager()
    {
        /// Instantiates the instance if has not been
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
        Debug.Log("Input Manager: " + NO_REFERENCES_MESSAGE);
    }

    /// <summary>
    /// Property to access the current position of the player's mouse pointer
    /// </summary>
    /// <value>Publicly gets and privately sets the value of Vector3 m_mousePos</value>
    public Vector3 MousePos
    {
        get
        {
            return m_mousePos;
        }

        private set
        {
            m_mousePos = value;
        }
    }

    /// <summary>
    /// Property to check whether or not the player is pressing the Fire button
    /// </summary>
    /// <value>Publicly gets and privately sets value of bool m_fire</value>
    public bool Fire
    {
        get
        {
            return m_fire;
        }

        private set
        {
            m_fire = value;
        }
    }

    /// <summary>
    /// Property to check whether or not the player is pressing the Quit button
    /// </summary>
    /// <value>Publicly gets and privately sets the value of bool m_quit</value>
    public bool Quit
    {
        get
        {
            return m_quit;
        }

        private set
        {
            m_quit = value;
        }
    }

    /// <summary>
    /// Property to check whether or not the player is pressing the Destroy Projectile button
    /// </summary>
    /// <value>Publicly gets and privately sets the value of bool m_destroyProjectile</value>
    public bool DestroyProjectile
    {
        get
        {
            return m_destroyProjectile;
        }

        private set
        {
            m_destroyProjectile = value;
        }
    }

    /// <summary>
    /// Property to access the manager's singleton instance outside this class
    /// Can be set privately only
    /// </summary>
    /// <value>Publicly gets and privately sets the value of the class's singleton instance, m_instance</value>
    public static InputManager Instance
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
}
