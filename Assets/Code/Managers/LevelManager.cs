using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Manager
{
    private static LevelManager m_instance = null;
    private int m_lastLevelIndex = 0;
    [SerializeField]
    private int[] m_menuLevels;
    [SerializeField]
    private float m_nextLevelDelay = 3.0f;

    private void Awake()
    {
        InitManager();
        SetReferences();
    }

    protected override void InitManager()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
        else if (m_instance != this)
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
        Debug.Log("LevelManager: " + NO_REFERENCES_MESSAGE);
    }

    public void LoadSelectedLevel(int _buildIndex)
    {
        m_lastLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(_buildIndex);
    }

    public int GetNextLevelIndex()
    {
        int _nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (_nextLevelIndex >= SceneManager.sceneCountInBuildSettings)
        {
            return 2;
        }
        else
        {
            return SceneManager.GetActiveScene().buildIndex + 1;
        }
    }

    public bool OnMenu()
    {
        int _activeSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;

        foreach (int element in m_menuLevels)
        {
            if (element == _activeSceneBuildIndex)
            {
                return true;
            }
        }

        return false;
    }

    public void ChangeLevel(int _buildIndex, float _delay)
    {
        StartCoroutine(LoadLevelAfterDelay(_buildIndex, _delay));
    }

    private IEnumerator LoadLevelAfterDelay(int _buildIndex, float _delay)
    {
        Debug.Log("Loading level " + _buildIndex + " in " + _delay + " seconds");

        yield return new WaitForSecondsRealtime(_delay);

        LoadSelectedLevel(_buildIndex);
    }

    public static LevelManager Instance
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

    public float NextLevelDelay
    {
        get
        {
            return m_nextLevelDelay;
        }

        private set
        {
            m_nextLevelDelay = value;
        }
    }

    public int LastLevelIndex
    {
        get
        {
            return m_lastLevelIndex;
        }

        private set
        {
            m_lastLevelIndex = value;
        }
    }
}
