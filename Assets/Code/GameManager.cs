using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private ScoreManager m_scoreManager;
    private Projectile m_activeProjectile;

    private int m_remainProjectiles = 5;
    private int m_remainLives = 3;
    private bool m_gameOver = false;
    private bool m_targetHit = false;

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
        if (m_activeProjectile == null & m_remainProjectiles <= 0 && !m_gameOver)
        {
            m_remainLives--;
        }

		if (m_remainLives <= 0)
        {
            m_gameOver = true;
            UIManager.instance.TxtGameStatus.text = "GAME OVER";
        }

        if (m_targetHit)
        {
            UIManager.instance.TxtGameStatus.text = "YOU WIN";
        }

        UIManager.instance.TxtRemainProjectiles.text = "Remaining Projectiles: " + m_remainProjectiles;
        UIManager.instance.TxtRemainLives.text = "Remaining Lives: " + m_remainLives;

    }

    public int RemainingProjectiles
    {
        gets
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

    public Projectile ActiveProjectile
    {
        set
        {
            m_activeProjectile = value;
        }
    }

    public void TargetHit()
    {
        m_targetHit = true;
    }
}
