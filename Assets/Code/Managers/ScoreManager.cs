using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manager responsible for the player's score
/// Contains methods for accessing the player's score, increasing it, and resetting it
/// All managers implement the singleton pattern to prevent the existance of multiple instances of the same manager in the scene
/// </summary>
public class ScoreManager : Manager
{
    /// <summary>
    /// Singleton instance for the manager
    /// Initialised inside InitManager
    /// </summary>
    private static ScoreManager m_instance = null;
    /// <summary>
    /// Stores the player's score
    /// </summary>
    private int m_totalScore = 0;

    /// <summary>
    /// Subscribes to relevant events
    /// </summary>
    private void OnEnable()
    {
        /// Subscribes to TargetHit to enable the manager to respond when the Score Target is hit
        ScoreTarget.TargetHit += CalculateLevelScore;
        /// Subscribes to sceneLoad event to enable the manager to respond to scene changes
        SceneManager.sceneLoaded += NewLevelLoaded;
    }

    /// <summary>
    /// Unsubscribes from events when the Manager is disabled
    /// </summary>
    private void OnDisable()
    {
        ScoreTarget.TargetHit -= CalculateLevelScore;
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
    /// Delegate compatible with ScoreTarget.TargetHit
    /// Called when one of the player's projectiles collides with the Score Target
    /// Calculates the players score the level and adds it to their total score
    /// </summary>
    /// <param name="_targetValue">The score value of the Score Target</param>
    private void CalculateLevelScore(int _targetValue)
    {
        Debug.Log("Score Manager calculate level score");
        /// Calculates the player's level score (score target value * remaining projectiles, multiplied by 5)
        TotalScore += _targetValue + GameManager.Instance.RemainingProjectiles * 5;
        /// Updates the GUI to reflect the user's new score value
        UIManager.Instance.SetTxtScore(m_totalScore);
    }

    /// <summary>
    /// Resets the player's score to zero
    /// </summary>
    public void ResetScore()
    {
        /// Sets the players score to 0
        m_totalScore = 0;
        /// Updates the GUI to display the user's new score
        UIManager.Instance.SetTxtScore(m_totalScore);
        Debug.Log("Score reset");
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
        Debug.Log("ScoreManager: " + NO_REFERENCES_MESSAGE);
    }

    /// <summary>
    /// Property to access the player's total score
    /// </summary>
    /// <value>Publically gets and privately sets the value of int m_totalScore</value>
    public int TotalScore
    {
        get
        {
            return m_totalScore;
        }

        private set
        {
            m_totalScore = value;
        }
    }

    /// <summary>
    /// Property to access the manager's singleton instance outside this class
    /// Can be set privately only
    /// </summary>
    /// <value>Publically gets and privately sets the value of the class's singleton instance, m_instance</value>
    public static ScoreManager Instance
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
