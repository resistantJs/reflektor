using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : Manager
{
    private static ScoreManager m_instance = null;

    private int m_totalScore = 0;

    private void OnEnable()
    {
        ScoreTarget.TargetHit += CalculateLevelScore;
    }

    private void OnDisable()
    {
        ScoreTarget.TargetHit -= CalculateLevelScore;
    }

    private void Awake()
    {
        InitManager();
        SetReferences();
    }

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

    private void CalculateLevelScore(int _targetValue)
    {
        Debug.Log("Score Manager calculate level score");
        TotalScore += _targetValue + GameManager.Instance.RemainingProjectiles * 5;
        UIManager.Instance.SetTxtScore("Score: " + m_totalScore);
    }

    public void ResetScore()
    {
        m_totalScore = 0;
        UIManager.Instance.SetTxtScore("Score: " + m_totalScore);
        Debug.Log("Score reset");
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

    protected override void NewLevelLoaded(Scene _scene, LoadSceneMode _mode)
    {
        SetReferences();
    }

    protected override void SetReferences()
    {
        Debug.Log("ScoreManager: Setting References");
        Debug.Log("No references");
    }
}
