using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int m_remainingBounces = 5;

    [SerializeField]
    private float m_minimumLifeTime = 0.15f;
    private float m_lifeTime = 0.0f;

    private void Awake()
    {
        GameManager.instance.ActiveProjectile = gameObject;
        Debug.Log(gameObject + " Projectile Created");
    }

    private void Update()
    {
        //UIManager.instance.TxtRemainBounces.text = "Remaining Bounces: " + m_remainingBounces;
        m_lifeTime += Time.deltaTime;
        PrematureDestroy();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Collidable")
        {
            if (m_remainingBounces <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Hit collidable object");
                m_remainingBounces--;
            }
        }
    }

    public int RemainingBounces
    {
        get
        {
            return m_remainingBounces;
        }
    }

    private void PrematureDestroy()
    {
        if (InputManager.Instance.DestroyProjectile && m_lifeTime >= m_minimumLifeTime)
        {
            Debug.Log("Destroy command accepted");
            Destroy(gameObject);
        }
    }
}
