using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Manager
{
    private static UIManager m_instance = null;

    private Text m_txtGameStatus = null;
    private Text m_txtScore = null;
    private Text m_txtRemainBounces = null;
    private Text m_txtRemainProjectiles = null;

    [SerializeField]
    private int[] m_nonGameplayLevels = null;

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

        if (InGameplayLevel())
        {
            m_txtGameStatus = GameObject.Find("txtGameStatus").GetComponent<Text>();
            m_txtScore = GameObject.Find("txtScore").GetComponent<Text>();
            m_txtRemainBounces = GameObject.Find("txtRemainBounces").GetComponent<Text>();
            m_txtRemainProjectiles = GameObject.Find("txtRemainProjectiles").GetComponent<Text>();
        }
    }

    protected override void NewLevelLoaded(Scene _scene, LoadSceneMode _mode)
    {
        SetReferences();
    }

    private bool InGameplayLevel()
    {
        foreach (int element in m_nonGameplayLevels)
        {
            if (SceneManager.GetActiveScene().buildIndex == element)
            {
                return false;
            }
        }

        return true;
    }

    public void SetTxtGameStatus(string _value)
    {
        if (m_txtGameStatus != null)
        {
            if (_value != null)
            {
                m_txtGameStatus.text = _value;
            }
        }
        else
        {
            Debug.Log("m_txtGameStatus is null");
        }
    }

    public void SetTxtScore(string _value)
    {
        if (m_txtScore != null)
        {
            if (_value != null)
            {
                m_txtScore.text = _value;
            }
        }
        else
        {
            Debug.Log("m_txtScore is null");
        }
    }

    public void SetTxtRemainBounces(string _value)
    {
        if (m_txtRemainBounces != null)
        {
            if (_value != null)
            {
                m_txtRemainBounces.text = _value;
            }
        }
        else
        {
            Debug.Log("m_txtRemainBounces is null");
        }
    }

    public void SetTxtRemainProjectiles(string _value)
    {
        if (m_txtRemainProjectiles != null)
        {
            if (_value != null)
            {
                m_txtRemainProjectiles.text = _value;
            }
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
