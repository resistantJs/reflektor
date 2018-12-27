using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Manager
{
    private static UIManager m_instance = null;

    [SerializeField]
    private int[] m_menuLevels;
    private Text m_txtGameStatus = null;
    private Text m_txtScore = null;
    private Text m_txtRemainBounces = null;
    private Text m_txtRemainProjectiles = null;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += NewLevelLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= NewLevelLoaded;
    }

    // Use this for initialization
    private void Awake()
    {
        InitManager();
        SetReferences();
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
        Debug.Log("UIManager: Setting References");

        if (!OnMenu())
        {
            Debug.Log("UIManager SetReferences: on gameplay level, setting references");

            m_txtGameStatus = GameObject.Find("txtGameStatus").GetComponent<Text>();
            m_txtScore = GameObject.Find("txtScore").GetComponent<Text>();
            m_txtRemainBounces = GameObject.Find("txtRemainBounces").GetComponent<Text>();
            m_txtRemainProjectiles = GameObject.Find("txtRemainProjectiles").GetComponent<Text>();
        }
        else
        {
            Debug.Log("UIManager SetReferences: not on gameplay level, not setting references");
        }
    }

    protected override void NewLevelLoaded(Scene _scene, LoadSceneMode _mode)
    {
        SetReferences();
    }

    private bool OnMenu()
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

    public void SetTxtGameStatus(int _messageCode)
    {
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

    public void SetTxtScore(int _value)
    {
        if (m_txtScore != null)
        {
            if (_value > 0)
            {
                m_txtScore.text = "SCORE: " + _value;
            }
        }
        else
        {
            Debug.Log("m_txtScore is null");
        }
    }

    public void SetTxtRemainBounces(int _value, bool _projectileActive)
    {
        if (m_txtRemainBounces != null)
        {
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

    public void SetTxtRemainProjectiles(int _value)
    {
        if (m_txtRemainProjectiles != null)
        {
            m_txtRemainProjectiles.text = "PROJECTILES: " + _value;
        }
        else
        {
            Debug.Log("m_txtRemainProjectiles is null");
        }
    }

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
