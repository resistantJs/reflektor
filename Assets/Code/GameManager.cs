using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Manager
{
    #region Properties

    private static GameManager m_instance = null;
    private GameObject m_activeProjectile = null;
    private int m_remainProjectiles = 5;
    private bool m_enablePlay = true;
    private bool m_gameOver = false;
    private bool m_targetHit = false;

    [SerializeField]
    private float m_nextLevelDelay = 3.0f;

    #region Events

    public delegate void GameOverEvent();
    public static event GameOverEvent GameIsOver;

    public delegate void LevelWonEvent();
    public static event LevelWonEvent LevelWasWon;

    public delegate void LevelStartEvent();
    public static event LevelStartEvent LevelHasStarted;

    #endregion

    #endregion

    private void OnEnable()
    {
        ScoreTarget.TargetHit += LevelWon;
        Projectile.ProjectileCreated += SetProjectileReference;
        Projectile.ProjectileDestroyed += ClearProjectileReference;
        SceneManager.sceneLoaded += NewLevelLoaded;
    }

    private void OnDisable()
    {
        ScoreTarget.TargetHit -= LevelWon;
        Projectile.ProjectileCreated -= SetProjectileReference;
        Projectile.ProjectileDestroyed -= ClearProjectileReference;
        SceneManager.sceneLoaded -= NewLevelLoaded;
    }

    // Use this for initialization
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
        RemainingProjectiles = 5;

        UIManager.Instance.SetTxtRemainBounces("Remaining Bounces: No active projectile");
        UIManager.Instance.SetTxtGameStatus("");

        LevelHasStarted();
    }

    private void LevelWon(int _scoreTargetValue)
    {
        Debug.Log("Level won");

        TargetHit = true;
        EnablePlay = false;

        UIManager.Instance.SetTxtGameStatus("YOU WIN");

        StartCoroutine(ChangeLevel(GetNextLevelIndex(), m_nextLevelDelay));

        LevelWasWon();
    }

    public void LevelSelectedLevel(int _buildIndex)
    {
        SceneManager.LoadScene(_buildIndex);
    }

    private int GetNextLevelIndex()
    {
        int _nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (_nextLevelIndex >= SceneManager.sceneCountInBuildSettings)
        {
            return 0;
        }
        else
        {
            return SceneManager.GetActiveScene().buildIndex + 1;
        }
    }

    public void UseProjectile()
    {
        if (RemainProjectiles > 0)
        {
            RemainProjectiles--;
            UIManager.Instance.SetTxtRemainProjectiles("Remaining Projectiles: " + RemainProjectiles);
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
        UIManager.Instance.SetTxtRemainBounces("Remaining Bounces: No active projectile");
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

    private bool OnMenu()
    {
        int _activeSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;

        if (_activeSceneBuildIndex == 0 || _activeSceneBuildIndex == 4)
        {
            return true;
        }

        return false;
    }

    private bool CheckGameOver()
    {
        if (!OnMenu())
        {
            if (ActiveProjectile == null)
            {
                Debug.Log("No active projectile");
                if (RemainingProjectiles <= 0)
                {
                    Debug.Log("No remaining projectiles");
                    if (!TargetHit)
                    {
                        Debug.Log("Target has not been hit");
                        if (!GameOver)
                        {
                            Debug.Log("Game is not already over");

                            Debug.Log("Game Over");

                            GameOver = true;
                            EnablePlay = false;

                            UIManager.Instance.SetTxtGameStatus("GAME OVER");
                            AudioManager.Instance.Play("GameOver");

                            StartCoroutine(ChangeLevel(0, m_nextLevelDelay));

                            GameIsOver();

                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    private IEnumerator ChangeLevel(int _buildIndex, float _delay)
    {
        Debug.Log("Loading level " + _buildIndex + " in " + _delay + " seconds");

        yield return new WaitForSecondsRealtime(_delay);

        SceneManager.LoadScene(_buildIndex);
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

    #region Accessors

    public static GameManager Instance
    {
        get
        {
            return m_instance;
        }

        set
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
            return RemainProjectiles;
        }
        private set
        {
            RemainProjectiles = value;
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

    #endregion
}
