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

    private int m_score = 0;

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

    // Update is called once per frame
    void Update()
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

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (m_activeProjectile == null & m_remainProjectiles <= 0 && !m_gameOver)
            {
                m_gameOver = true;
                m_enablePlay = false;
                UIManager.Instance.TxtGameStatus.text = "GAME OVER";
                AudioManager.Instance.Play("GameOver");

                Invoke("Restart", m_restartDelay);
            }

            if (m_activeProjectile != null)
            {
                UIManager.Instance.TxtRemainBounces.text = "Remaining Bounces: No Active Projectile";
            }
        }
    }

    private void SetUpGame()
    {
        Debug.Log("Setting up game");
        EnablePlay = true;
        RemainingProjectiles = 5;
    }

    private void LevelWon(int _scoreTargetValue)
    {
        EnablePlay = false;
        Score = _scoreTargetValue;
        UIManager.Instance.TxtScore.text = "Score: " + m_score;

        UIManager.Instance.TxtGameStatus.text = "YOU WIN";

        Invoke("LoadNextLevel", m_nextLevelDelay);
    }

    private void LoadNextLevel()
    {
        Debug.Log("Loading another level");
        int _nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

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
        ResetScore();
        UIManager.Instance.TxtRemainBounces.text = "Remaining Bounces:";
        UIManager.Instance.TxtGameStatus.text = "";
    }

    public int Score
    {
        get
        {
            return Score;
        }

        set
        {
            if (value > 0)
            {
                m_score += value;
            }
        }
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

    private void ResetScore()
    {
        m_score = 0;
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
}
