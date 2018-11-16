using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private ScoreManager m_scoreManager;
    private GameObject m_activeProjectile;
    private Projectile m_activeProjectileScript;

    private int m_remainProjectiles = 5;
    private bool m_enablePlay = true;
    private bool m_gameOver = false;
    private bool m_targetHit = false;
    private bool m_reduceLives = true;

    private float m_restartDelay = 5.0f;

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
	}
	
	// Update is called once per frame
	void Update()
    {
        if (InputManager.Instance.Quit)
        {
            Application.Quit();
        }

        if (InputManager.Instance.Fire && m_activeProjectile != null)
        {
            Debug.Log("Destory projectile command issued");
            m_activeProjectileScript.DestroyProjectile();
        }

        if (m_activeProjectile == null & m_remainProjectiles <= 0 && !m_gameOver && m_reduceLives)
        {
            m_gameOver = true;
            m_enablePlay = false;
            UIManager.instance.TxtGameStatus.text = "GAME OVER";

            Invoke("Restart", m_restartDelay);
        }

        if (m_targetHit)
        {
            m_enablePlay = false;
            UIManager.instance.TxtGameStatus.text = "YOU WIN";
            if (m_activeProjectile != null)
            {
                Destroy(m_activeProjectile.gameObject);
            }

            Invoke("Restart", m_restartDelay);
        }

        UIManager.instance.TxtRemainProjectiles.text = "Remaining Projectiles: " + m_remainProjectiles;


        if (m_activeProjectile != null)
        {
            UIManager.instance.TxtRemainBounces.text = "Remaining Bounces: " + m_activeProjectileScript.RemainingBounces.ToString();
        }
        else
        {
            UIManager.instance.TxtRemainBounces.text = "Remaining Bounces: No Active Projectile";
        }
    }

    public int RemainingProjectiles
    {
        get
        {
            return m_remainProjectiles;
        }
    }

    public void UseProjectile()
    {
        if (m_remainProjectiles > 0)
        {
            m_remainProjectiles--;
        }
    }

    public GameObject ActiveProjectile
    {
        set
        {
            m_activeProjectile = value;
            m_activeProjectileScript = m_activeProjectile.GetComponent<Projectile>();
        }
    }

    public bool EnablePlay
    {
        get
        {
            return m_enablePlay;
        }
    }

    public void TargetHit()
    {
        m_targetHit = true;
    }

    void Restart()
    {
        m_gameOver = false;
        m_enablePlay = true;
        m_targetHit = false;
        m_remainProjectiles = 5;
        ScoreManager.instance.ResetScore();
        UIManager.instance.TxtRemainBounces.text = "Remaining Bounces:";
        UIManager.instance.TxtGameStatus.text = "";
    }
}
