using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manager responsible for managing the game rules and state
/// Contains methods for detecting win and lose states, initialising the game state, and quitting the game
/// All managers implement the singleton pattern to prevent the existance of multiple instances of the same manager in the scene
/// </summary>
public class GameManager : Manager
{
    /// <summary>
    /// Singleton instance for the manager
    /// Initialised inside InitManager
    /// </summary>
    private static GameManager m_instance = null;
    /// <summary>
    /// Reference the currently active projectile (i.e. the one that is in flight)
    /// </summary>
    private GameObject m_activeProjectile = null;
    /// <summary>
    /// The player's remaining number of projectiles. 
    /// Initially set to 5
    /// </summary>
    private int m_remainProjectiles = 5;
    /// <summary>
    /// Controls whether play is enabled or not
    /// Used to control whether player input is enabled or not
    /// True: play is enabled
    /// False: play is disabled
    /// </summary>
    private bool m_enablePlay = true;
    /// <summary>
    /// Indicates whether player has cleared (i.e won) the level
    /// True: the player has cleared the level
    /// False: the player has not yet cleared the level
    /// </summary>
    private bool m_levelClear = false;
    /// <summary>
    /// Indicates whether game is over (i.e. player expended all of their projectiles)
    /// True: the game is over
    /// False: the game is not yet over
    /// </summary>
    private bool m_gameOver = false;
    /// <summary>
    /// Indicates whether the player has hit the score target with one of their projectiles
    /// </summary>
    private bool m_targetHit = false;

    /// <summary>
    /// Subscribes to relevant events
    /// </summary>
    private void OnEnable()
    {
        /// Subscribes to TargetHit to detect when a projectile hits the target
        ScoreTarget.TargetHit += SetTargetHit;
        /// Suscribes to ProjectileStateChanged to respond to the projectiles being created and destroyed
        Projectile.ProjectileStateChanged += SetProjectileReference;
        /// Subscribes to sceneLoad event to enable the manager to respond to scene changes
        SceneManager.sceneLoaded += NewLevelLoaded;
    }

    /// <summary>
    /// Unsubscribes from events when the Manager is disabled
    /// </summary>
    private void OnDisable()
    {
        ScoreTarget.TargetHit -= SetTargetHit;
        Projectile.ProjectileStateChanged -= SetProjectileReference;
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
    /// Initialises the game state before play begins
    /// </summary>
    private void Start()
    {
        SetUpGame();
    }

    /// <summary>
    /// Initialises the game state
    /// </summary>
    private void SetUpGame()
    {
        Debug.Log("Setting up game");

        /// Sets game state variables to correct values at the start of play
        m_levelClear = false;
        m_gameOver = false;
        m_enablePlay = true;
        m_targetHit = false;

        /// Resets the player's projectile stock
        m_remainProjectiles = 5;

        Debug.Log("SetUpGame: Setting remaining projectiles GUI");
        UIManager.Instance.SetTxtRemainProjectiles(m_remainProjectiles);

        Debug.Log("SetUpGame: Setting remaining bounces GUI");
        /// Updates the GUI to display the player's current projectile stock
        UIManager.Instance.SetTxtRemainBounces(0, false);

        Debug.Log("SetUpGame: Clearing game status text");
        /// Clears the game over or level clear message from the screen
        UIManager.Instance.SetTxtGameStatus(0);

        Debug.Log("SetUpGame: Setting total score GUI, with value: " + ScoreManager.Instance.TotalScore);
        UIManager.Instance.SetTxtScore(ScoreManager.Instance.TotalScore);
    }

    /// <summary>
    /// Delegate compatible with ScoreTarget.TargetHit
    /// Called when a projectile strikes the Score Target in the level
    /// </summary>
    /// <param name="_scoreTargetValue">The amount of score striking the score target is worth</param>
    private void SetTargetHit(int _scoreTargetValue)
    {
        /// Sets the value of m_targetHit to signify that player has struck the Score Target
        m_targetHit = true;
    }

    /// <summary>
    /// Sets the game state upon the player clearing a level
    /// </summary>
    private void StateLevelClear()
    {
        Debug.Log("Level won");

        /// Sets the values to indicate the player has cleared the level
        m_levelClear = true;
        m_targetHit = true;
        /// Prevents user input
        m_enablePlay = false;

        /// Displays the 'LEVEL CLEAR' message on the GUI
        UIManager.Instance.SetTxtGameStatus(2);

        /// Plays the level complete sound effect
        AudioManager.Instance.Play("LevelComplete");

        /// Sets the scene to change to the next in sequence after a delay
        LevelManager.Instance.ChangeLevel(LevelManager.Instance.GetNextLevelIndex(), LevelManager.Instance.NextLevelDelay);
    }

    /// <summary>
    /// Sets the game state upon a game over
    /// </summary>
    private void StateGameOver()
    {
        Debug.Log("Game Over Check: Game is not already over");
        Debug.Log("Game Over Check: Game Over");

        /// Sets the values to indicate that the game is over
        m_gameOver = true;
        m_enablePlay = false;

        /// Displays the 'GAME OVER' message on the GUI
        UIManager.Instance.SetTxtGameStatus(1);

        /// Plays the game over sound effect
        AudioManager.Instance.Play("GameOver");

        /// Sets the scene to change to Game Over screen after a delay
        LevelManager.Instance.ChangeLevel(1, LevelManager.Instance.NextLevelDelay);
    }

    /// <summary>
    /// If the player has remaining projectile's, decrements their stock by one and updates the GUI display the new value
    /// </summary>
    public void UseProjectile()
    {
        if (m_remainProjectiles > 0)
        {
            m_remainProjectiles--;
            UIManager.Instance.SetTxtRemainProjectiles(m_remainProjectiles);
        }
    }

    /// <summary>
    /// Delegate compatible with Projectile.ProjectileStateChanged
    /// Called when a projectile is created or destroyed
    /// </summary>
    /// <param name="_projectile">Reference to the currently active projectile</param>
    private void SetProjectileReference(GameObject _projectile)
    {
        /// If the passed value is not null, disables input
        /// This path will run if a projectile is active
        if (_projectile != null)
        {
            m_activeProjectile = _projectile;
            m_enablePlay = false;
        }
        /// If the passed value is null, disables input and updates GUI to indicate that there is not active projectile
        /// This path will run when the currently active projectile is destroyed
        else
        {
            m_activeProjectile = null;
            m_enablePlay = true;
            /// Displays the 'NO PROJECTILE' message on the GUI
            UIManager.Instance.SetTxtRemainBounces(0, false);
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
    /// Contains code to get references to objects the manager depends on
    /// Should be called inside NewLevelLoaded
    /// </summary>s
    protected override void SetReferences()
    {
        Debug.Log("GameManager: Setting References");
        Debug.Log("GameManager: " + NO_REFERENCES_MESSAGE);
    }

    /// <summary>
    /// Method compatible with SceneManager.sceneLoad event
    /// Enables managers to respond to a scene being loaded, for example by reacquiring references to objects in the scene
    /// </summary>
    /// <param name="_scene">Scene reference</param>
    /// <param name="_mode">LoadSceneMode reference</param>
    protected override void NewLevelLoaded(Scene _scene, LoadSceneMode _mode)
    {
        Debug.Log("GameManager: NewLevelLoad");
        SetReferences();
        SetUpGame();
    }

    /// <summary>
    /// Checks the current game state
    /// Calls the appropriate method upon detecting a particular state
    /// </summary>
    private void CheckGameState()
    {
        /// Checks if the current scene is a menu, such as the Main Menu
        /// Game state is not checked if the current scene is defined as a menu 
        if (!LevelManager.Instance.OnMenu())
        {
            //Debug.Log("CheckGameState: Not on menu");

            /// Checks if one of the player's projectiles has hit the Score Target or not
            if (!m_targetHit)
            {
                //Debug.Log("CheckGameState: Target has not been hit");

                /// Checks if there is an active projectile
                if (m_activeProjectile == null)
                {
                    //Debug.Log("CheckGameState: No active projectile");

                    /// Checks if the player has expended their stock of the projectiles for the level
                    if (m_remainProjectiles <= 0)
                    {
                        //Debug.Log("CheckGameState: No remaining projectiles");

                        /// Checks if the game is already in a game over state
                        /// Prevents the game over state being set multiple times
                        if (!m_gameOver)
                        {
                            //Debug.Log("CheckGameState: Game is not already over");

                            /// Upon detecting a game over, calls StateGameOver to set game state
                            StateGameOver();
                        }
                    }
                }
            }
            /// If the player has hit the target, calls StateLevelClear to set game state
            else
            {
                if (!m_levelClear)
                {
                    //Debug.Log("CheckGameState: Target has been hit");
                    /// Upon detecting that the player has cleared the level, calls StateGameOver to set game state
                    StateLevelClear();
                }
            }
        }
        else
        {
            //Debug.Log("CheckGameOver: On a menu");
        }
    }

    /// <summary>
    /// Upon pressing the Quit Game button, quits the game if the player is on the Main Menu. Otherwise returns to the Main Menu
    /// </summary>
    private void Quit()
    {
        /// Does not quit if play is disabled
        if (m_enablePlay)
        {
            /// Detects if the user has pressed the Quit button
            if (InputManager.Instance.Quit)
            {
                /// If the player is not on the Main Menu, load the Main Menu
                if (SceneManager.GetActiveScene().buildIndex != 0)
                {
                    SceneManager.LoadScene(0);
                }
                /// If the player is on the Main Menu, quit the game
                else
                {
                    Application.Quit();
                }
            }
        }
    }

    /// <summary>
    /// Calls Quit to check if the user is attempting to quit the game
    /// Calls CheckGameState to detect a change in game state
    /// </summary>
    private void Update()
    {
        Quit();
        CheckGameState();
    }

    /// <summary>
    /// Property to access the manager's singleton instance outside this class
    /// Can be set privately only
    /// </summary>
    /// <value>Publicly gets and privately sets the value of the class's singleton instance, m_instance</value>
    public static GameManager Instance
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
    /// Property to check how many projectiles remain in the player's stock
    /// </summary>
    /// <value>Publicly gets and privately sets the value of m_remainProjectiles</value>
    public int RemainingProjectiles
    {
        get
        {
            return m_remainProjectiles;
        }
        private set
        {
            m_remainProjectiles = value;
        }
    }

    /// <summary>
    /// Property to check if play is enabled or not
    /// </summary>
    /// <value>Public gets and privately sets the value of m_enablePlay</value>
    public bool EnablePlay
    {
        get
        {
            return m_enablePlay;
        }

        private set
        {
            m_enablePlay = value;
        }
    }
}
