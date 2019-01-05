using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manager responsible for updating the in-game GUI
/// Contains methods for accessing the elements of the GUI
/// All managers implement the singleton pattern to prevent the existance of multiple instances of the same manager in the scene
/// </summary>
public class UIManager : Manager
{
    /// <summary>
    /// Singleton instance for the manager
    /// Initialised inside InitManager
    /// </summary>
    private static UIManager m_instance = null;
    /// <summary>
    /// Array storing build indicies of menu scenes (i.e. scenes that do not contain gameplay)
    /// Set in the Inspector
    /// </summary>
    [SerializeField]
    private int[] m_menuLevels;
    /// <summary>
    /// Reference to the txtGameStatus element of the GUI
    /// Element is responsible for displaying large messages such as "GAME OVER"
    /// </summary>
    private Text m_txtGameStatus = null;
    /// <summary>
    /// Reference to the txtScore element of the GUI
    /// Element is responsible for displaying the player's total score
    /// </summary>
    private Text m_txtScore = null;
    /// <summary>
    /// Reference to the txtRemainBounces element of the GUI
    /// Element is responsible for displaying the number of remaining bounces for the active projectile
    /// </summary>
    private Text m_txtRemainBounces = null;
    /// <summary>
    /// Reference to the txtRemainProjectiles element of the GUI
    /// Element is responsible for displaying the number of projectiles remaining in the player's stock
    /// </summary>
    private Text m_txtRemainProjectiles = null;

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
        Debug.Log("UIManager: Awake");
        InitManager();
        SetReferences();
    }

    /// <summary>
    /// Initialises the singleton instance of the manager and sets it not be destroyed when a new scene is loaded
    /// Contains other initialisation code for a manager
    /// </summary>
    protected override void InitManager()
    {
        /// Instantiates the instance if it has not been already
        if (Instance == null)
        {
            Instance = this;
        }
        /// If an instance already exists, destroys this one
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        /// Sets this instance to not be destroyed when a new scene is loaded
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Contains code to get references to objects the manager depends on
    /// Should be called inside NewLevelLoaded
    /// </summary>s
    protected override void SetReferences()
    {
        Debug.Log("UIManager: Setting References");

        /// Due to the UIManager depending on objects that exist in gameplay scenes only, references are set only when the current scene is not marked as a menu scene 
        if (!OnMenu())
        {
            Debug.Log("UIManager SetReferences: on gameplay level, setting references");

            /// Gets references to the elements of the GUI
            m_txtGameStatus = GameObject.Find("txtGameStatus").GetComponent<Text>();
            if (m_txtGameStatus == null)
                Debug.Log("m_txtGameStatus ref is null");

            m_txtScore = GameObject.Find("txtScore").GetComponent<Text>();
            if (m_txtScore == null)
                Debug.Log("m_txtScore ref is null");

            m_txtRemainBounces = GameObject.Find("txtRemainBounces").GetComponent<Text>();
            if (m_txtRemainBounces == null)
                Debug.Log("m_txtRemainBounces ref is null");

            m_txtRemainProjectiles = GameObject.Find("txtRemainProjectiles").GetComponent<Text>();
            if (m_txtRemainProjectiles == null)
                Debug.Log("m_txtRemainProjectiles ref is null");
        }
        /// If the current scene is marked as a menu scene, no references are set
        else
        {
            Debug.Log("UIManager SetReferences: not on gameplay level, not setting references");
        }
    }

    /// <summary>
    /// Method compatible with SceneManager.sceneLoad event
    /// Enables managers to respond to a scene being loaded, for example by reacquiring references to objects in the scene
    /// </summary>
    /// <param name="_scene">Scene reference</param>
    /// <param name="_mode">LoadSceneMode reference</param
    protected override void NewLevelLoaded(Scene _scene, LoadSceneMode _mode)
    {
        SetReferences();
    }

    /// <summary>
    /// Identifies whether or not the current scene is marked as a menu
    /// Certain objects depend on others that are not present in menu scenes, so it is necessary to first check the current scene before attempting to get a reference
    /// </summary>
    /// <returns>
    /// Bool indicated whether or not the current scene is marked as a menu
    /// True: the current scene is marked as a menu
    /// False: the current scene is not marked as a menu
    /// </returns>
    private bool OnMenu()
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
    /// Sets the message displayed in the txtGameStatus GUI element
    /// Displayed message is selected from a pre-set range using an integer
    /// </summary>
    /// <param name="_messageCode">Integer corresponding to the message to display</param>
    public void SetTxtGameStatus(int _messageCode)
    {
        /// Does not update element if reference is null
        if (m_txtGameStatus != null)
        {
            switch (_messageCode)
            {
                case 0:
                    m_txtGameStatus.text = "";
                    break;
                case 1:
                    m_txtGameStatus.text = "GAME OVER";
                    break;
                case 2:
                    m_txtGameStatus.text = "LEVEL CLEAR";
                    break;
            }
        }
        else
        {
            Debug.Log("m_txtGameStatus is null");
        }
    }

    /// <summary>
    /// Sets the value displayed in the txtScore GUI element
    /// </summary>
    /// <param name="_value">Score value to displayed on the GUI</param>
    public void SetTxtScore(int _value)
    {
        /// Does not update element if reference is null
        if (m_txtScore != null)
        {
            /// Passed score value must be negative
            if (_value >= 0)
            {
                m_txtScore.text = "SCORE: " + _value;
            }
        }
        else
        {
            Debug.Log("m_txtScore is null");
        }
    }

    /// <summary>
    /// Sets the value displayed in the txtRemainBounces GUI element
    /// </summary>
    /// <param name="_value">Value to displayed if there is an active projectile</param>
    /// <param name="_projectileActive">
    /// Value indicating whether there is an active projectile
    /// True: there is an active projectile. Provided integer value will be displayed
    /// False: there is no active projectile. A 'no projectile' message will be displayed
    /// </param>
    public void SetTxtRemainBounces(int _value, bool _projectileActive)
    {
        /// Does not update element if reference is null
        if (m_txtRemainBounces != null)
        {
            /// Provided integer value is displayed only the caller specifies that there is active projectile
            if (_projectileActive)
            {
                m_txtRemainBounces.text = "BOUNCES: " + _value;
            }
            else
            {
                m_txtRemainBounces.text = "BOUNCES: NO PROJECTILE";
            }
        }
        else
        {
            Debug.Log("m_txtRemainBounces is null");
        }
    }

    /// <summary>
    /// Sets the value displayed in txtRemainProjectiles
    /// </summary>
    /// <param name="_value">Value to display</param>
    public void SetTxtRemainProjectiles(int _value)
    {
        /// Does not update element if reference is null
        if (m_txtRemainProjectiles != null)
        {
            m_txtRemainProjectiles.text = "PROJECTILES: " + _value;
        }
        else
        {
            Debug.Log("m_txtRemainProjectiles is null");
        }
    }

    /// <summary>
    /// Property to access the manager's singleton instance outside this class
    /// Can be set privately only
    /// </summary>
    /// <value>
    /// gets and privately sets the value of the class's singleton instance, m_instance</value>
    public static UIManager Instance
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
