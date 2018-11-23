using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Manager
{
    private static GameManager m_instance;

    private GameObject m_activeProjectile;

    private int m_remainProjectiles = 5;
    private bool m_enablePlay = true;
    private bool m_gameOver = false;

    private bool m_targetHit = false;

    [SerializeField]
    private float m_restartDelay = 3.0f;
    [SerializeField]
    private float m_nextLevelDelay = 3.0f;

    private void OnEnable()
    {
        ScoreTarget.TargetHit += LevelWon;
        Projectile.ProjectileCreated += SetProjectileReference;
        SceneManager.sceneLoaded += NewLevelLoaded;
    }

    private void OnDisable()
    {
        ScoreTarget.TargetHit -= LevelWon;
        Projectile.ProjectileCreated -= SetProjectileReference;
        SceneManager.sceneLoaded -= NewLevelLoaded;
    }

    // Use this for initialization
    void Awake()
    {
        InitManager();
        SetReferences();
	}

    

    public int ActiveLevelBuildIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    private void SetUpGame()
    {
        Debug.Log("Setting up game");
        EnablePlay = true;
        RemainingProjectiles = 5;
    }

    private void LevelWon(int _scoreTargetValue)
    {
        m_targetHit = true;

        EnablePlay = false;
        
        UIManager.Instance.TxtGameStatus.text = "YOU WIN";

        Invoke("LoadNextLevel", m_nextLevelDelay);
    }

    

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        Debug.Log("Loading another level");
        int _nextLevelIndex = ActiveLevelBuildIndex() + 1;

        if (_nextLevelIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("Loading Main Menu");

            SceneManager.LoadScene(0);
        }
        else
        {
            Debug.Log("Loading Next Level: " + _nextLevelIndex);

            SceneManager.LoadScene(_nextLevelIndex);
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

    public void UseProjectile()
    {
        if (m_remainProjectiles > 0)
        {
            m_remainProjectiles--;
            UIManager.Instance.TxtRemainProjectiles.text = "Remaining Projectiles: " + m_remainProjectiles;
        }
    }

    private void SetProjectileReference(GameObject _projectile)
    {
        m_activeProjectile = _projectile;
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

    void Restart()
    {
        m_gameOver = false;
        m_enablePlay = true;
        m_remainProjectiles = 5;
        ScoreManager.Instance.ResetScore();
        UIManager.Instance.TxtRemainBounces.text = "Remaining Bounces: No active projectile";
        UIManager.Instance.TxtGameStatus.text = "";
    }

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
        Debug.Log("No references");
    }

    protected override void NewLevelLoaded(Scene _scene, LoadSceneMode _mode)
    {
        SetReferences();
        SetUpGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.Instance.Quit)
        {
            if (ActiveLevelBuildIndex() != 0)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                Application.Quit();
            }
        }

        if (ActiveLevelBuildIndex() != 0 && ActiveLevelBuildIndex() != 4)
        {
            if (m_activeProjectile == null && m_remainProjectiles <= 0 && !m_gameOver && !m_targetHit)
            {
                m_gameOver = true;
                m_enablePlay = false;
                UIManager.Instance.TxtGameStatus.text = "GAME OVER";
                AudioManager.Instance.Play("GameOver");
                SceneManager.LoadScene(0);

                Invoke("ReturnToMainMenu", m_restartDelay);
            }

            if (m_activeProjectile != null)
            {
                UIManager.Instance.TxtRemainBounces.text = "Remaining Bounces: No Active Projectile";
            }
        }
        else
        {
            Restart();
        }
    }
}
