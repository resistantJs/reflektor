using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private Text m_txtGameStatus;
    private Text m_txtScore;
    private Text m_txtRemainBounces;
    private Text m_txtRemainProjectiles;
    private Text m_txtRemainLives;

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        m_txtGameStatus = GameObject.Find("txtGameStatus").GetComponent<Text>();
        m_txtScore = GameObject.Find("txtScore").GetComponent<Text>();
        m_txtRemainBounces = GameObject.Find("txtRemainBounces").GetComponent<Text>();
        m_txtRemainProjectiles = GameObject.Find("txtRemainProjectiles").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update()
    {
		
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

    public Text TxtRemainLives
    {
        get
        {
            return m_txtRemainLives;
        }

        set
        {
            m_txtRemainLives = value;
        }
    }
}
