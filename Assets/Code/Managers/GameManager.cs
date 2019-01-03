using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Manager
{
    // Properties
    private static GameManager m_instance = null;
    private GameObject m_activeProjectile = null;
    private int m_remainProjectiles = 5;
    private bool m_enablePlay = true;
    private bool m_gameOver = false;
    private bool m_targetHit = false;

    // Events
    public delegate void GameOverEvent();
    public static event GameOverEvent GameIsOver;
    public delegate void LevelWonEvent();
    public static event LevelWonEvent LevelWasWon;
    public delegate void LevelStartEvent();
    public static event LevelStartEvent LevelHasStarted;

    // Subscribing to events
    private void OnEnable()
    {
        ScoreTarget.TargetHit += LevelWon;
        Projectile.ProjectileCreated += SetProjectileReference;
        Projectile.ProjectileDestroyed += ClearProjectileReference;
        SceneManager.sceneLoaded += NewLevelLoaded;
    }

    // Unsuscribing from events
    private void OnDisable()
    {
        ScoreTarget.TargetHit -= LevelWon;
        Projectile.ProjectileCreated -= SetProjectileReference;
        Projectile.ProjectileDestroyed -= ClearProjectileReference;
        SceneManager.sceneLoaded -= NewLevelLoaded;
    }

    // Initialisation
    private void Awake()
    {
        InitManager();
        SetReferences();
    }

    private void SetUpGame()
    {
        Debug.Log("Setting up game");

        GameOver = false;
        EnablePlay = true;
        TargetHit = false;
        RemainingProjectiles = 5;

        UIManager.Instance.SetTxtRemainBounces(0, false);
        UIManager.Instance.SetTxtGameStatus(0);

        LevelHasStarted();
    }

    private void LevelWon(int _scoreTargetValue)
    {
        Debug.Log("Level won");

        TargetHit = true;
        EnablePlay = false;

        UIManager.Instance.SetTxtGameStatus(2);

        LevelManager.Instance.ChangeLevel(LevelManager.Instance.GetNextLevelIndex(), LevelManager.Instance.NextLevelDelay);

        LevelWasWon();
    }

    public void UseProjectile()
    {
        if (RemainProjectiles > 0)
        {
            RemainProjectiles--;
            UIManager.Instance.SetTxtRemainProjectiles(RemainProjectiles);
        }
    }

    private void SetProjectileReference(GameObject _projectile)
    {
        if (_projectile != null)
        {
            ActiveProjectile = _projectile;
        }
    }

    private void ClearProjectileReference(GameObject _projectile)
    {
        ActiveProjectile = null;
        UIManager.Instance.SetTxtRemainBounces(0, false);
        CheckGameOver();
    }

    protected override void InitManager()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    protected override void SetReferences()
    {
        Debug.Log("GameManager: Setting References");
        Debug.Log(NO_REFERENCES_MESSAGE);
    }

    protected override void NewLevelLoaded(Scene _scene, LoadSceneMode _mode)
    {
        SetReferences();
        SetUpGame();
    }

    private void CheckGameOver()
    {
        if (!LevelManager.Instance.OnMenu())
        {
            Debug.Log("Game Over Check: Not on menu");
            if (ActiveProjectile == null)
            {
                Debug.Log("Game Over Check: No active projectile");
                if (m_remainProjectiles <= 0)
                {
                    Debug.Log("Game Over Check: No remaining projectiles");
                    if (!TargetHit)
                    {
                        Debug.Log("Game Over Check: Target has not been hit");
                        if (!GameOver)
                        {
                            Debug.Log("Game Over Check: Game is not already over");
                            Debug.Log("Game Over Check: Game Over");

                            GameOver = true;
                            EnablePlay = false;

                            GameIsOver();

                            UIManager.Instance.SetTxtGameStatus(1);
                            AudioManager.Instance.Play("GameOver");

                            LevelManager.Instance.ChangeLevel(1, LevelManager.Instance.NextLevelDelay);
                        }
                    }
                }
            }
        }
        else
        {
            Debug.Log("On menu");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (InputManager.Instance.Quit)
        {
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                Application.Quit();
            }
        }
    }

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

    public bool GameOver
    {
        get
        {
            return m_gameOver;
        }

        private set
        {
            m_gameOver = value;
        }
    }

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

    public bool TargetHit
    {
        get
        {
            return m_targetHit;
        }

        private set
        {
            m_targetHit = value;
        }
    }

    private GameObject ActiveProjectile
    {
        get
        {
            return m_activeProjectile;
        }

        set
        {
            m_activeProjectile = value;
        }
    }

    public int RemainProjectiles
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
}
