using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Manager
{
    // Properties
    private static GameManager m_instance = null;
    private GameObject m_activeProjectile = null;
    private int m_remainProjectiles = 5;
    private bool m_enablePlay = true;
    private bool m_levelClear = false;
    private bool m_gameOver = false;
    private bool m_targetHit = false;

    // Subscribing to events
    private void OnEnable()
    {
        ScoreTarget.TargetHit += SetTargetHit;
        Projectile.ProjectileStateChanged += SetProjectileReference;
        SceneManager.sceneLoaded += NewLevelLoaded;
    }

    // Unsuscribing from events
    private void OnDisable()
    {
        ScoreTarget.TargetHit -= SetTargetHit;
        Projectile.ProjectileStateChanged -= SetProjectileReference;
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

        m_levelClear = false;
        m_gameOver = false;
        m_enablePlay = true;
        m_targetHit = false;

        m_remainProjectiles = 5;

        UIManager.Instance.SetTxtRemainBounces(0, false);
        UIManager.Instance.SetTxtGameStatus(0);
    }

    private void SetTargetHit(int _scoreTargetValue)
    {
        m_targetHit = true;
    }

    private void StateLevelClear()
    {
        Debug.Log("Level won");

        m_levelClear = true;
        m_targetHit = true;
        m_enablePlay = false;

        UIManager.Instance.SetTxtGameStatus(2);
        AudioManager.Instance.Play("LevelComplete");

        LevelManager.Instance.ChangeLevel(LevelManager.Instance.GetNextLevelIndex(), LevelManager.Instance.NextLevelDelay);
    }

    private void StateGameOver()
    {
        Debug.Log("Game Over Check: Game is not already over");
        Debug.Log("Game Over Check: Game Over");

        m_gameOver = true;
        m_enablePlay = false;

        UIManager.Instance.SetTxtGameStatus(1);
        AudioManager.Instance.Play("GameOver");

        LevelManager.Instance.ChangeLevel(1, LevelManager.Instance.NextLevelDelay);
    }

    public void UseProjectile()
    {
        if (m_remainProjectiles > 0)
        {
            m_remainProjectiles--;
            UIManager.Instance.SetTxtRemainProjectiles(m_remainProjectiles);
        }
    }

    private void SetProjectileReference(GameObject _projectile)
    {
        if (_projectile != null)
        {
            m_activeProjectile = _projectile;
            m_enablePlay = false;
        }
        else
        {
            m_activeProjectile = null;
            m_enablePlay = true;
            UIManager.Instance.SetTxtRemainBounces(0, false);
        }
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

    private void CheckGameState()
    {
        if (!LevelManager.Instance.OnMenu())
        {
            Debug.Log("CheckGameState: Not on menu");

            if (!m_targetHit)
            {
                Debug.Log("CheckGameState: Target has not been hit");

                if (m_activeProjectile == null)
                {
                    Debug.Log("CheckGameState: No active projectile");

                    if (m_remainProjectiles <= 0)
                    {
                        Debug.Log("CheckGameState: No remaining projectiles");

                        if (!m_gameOver)
                        {
                            Debug.Log("CheckGameState: Game is not already over");

                            StateGameOver();
                        }
                    }
                }
            }
            else
            {
                if (!m_levelClear)
                {
                    Debug.Log("CheckGameState: Target has been hit");

                    StateLevelClear();
                }
            }
        }
        else
        {
            Debug.Log("CheckGameOver: On a menu");
        }
    }

    private void Quit()
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

    // Update is called once per frame
    private void Update()
    {
        Quit();
        CheckGameState();
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
