using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : Manager
{
    private static UIManager m_instance;

    private Text m_txtGameStatus;
    private Text m_txtScore;
    private Text m_txtRemainBounces;
    private Text m_txtRemainProjectiles;

    // Use this for initialization
    void Awake()
    {
        InitManager();
        SetReferences();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += NewLevelLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= NewLevelLoaded;
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
    }

    protected override void SetReferences()
    {
        m_txtGameStatus = UnityEngine.GameObject.Find("txtGameStatus").GetComponent<Text>();
        m_txtScore = UnityEngine.GameObject.Find("txtScore").GetComponent<Text>();
        m_txtRemainBounces = UnityEngine.GameObject.Find("txtRemainBounces").GetComponent<Text>();
        m_txtRemainProjectiles = UnityEngine.GameObject.Find("txtRemainProjectiles").GetComponent<Text>();
    }

    protected override void NewLevelLoaded(Scene _scene, LoadSceneMode _mode)
    {
        SetReferences();
    }

    public Text TxtGameStatus
    {
        get
        {
            return m_txtGameStatus;
        }

        set
        {
            m_txtGameStatus = value;
        }
    }

    public Text TxtScore
    {
        get
        {
            return m_txtScore;
        }

        set
        {
            m_txtScore = value;
        }
    }

    public Text TxtRemainBounces
    {
        get
        {
            return m_txtRemainBounces;
        }

        set
        {
            m_txtRemainBounces = value;
        }
    }

    public Text TxtRemainProjectiles
    {
        get
        {
            return m_txtRemainProjectiles;
        }

        set
        {
            m_txtRemainProjectiles = value;
        }
    }

    public static UIManager Instance
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
}
