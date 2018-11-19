using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public delegate void ProjectileCreatedEvent(GameObject _projectile);
    public static event ProjectileCreatedEvent ProjectileCreated;

    private int m_remainingBounces = 5;

    [SerializeField]
    private float m_minimumLifeTime = 0.15f;
    private float m_lifeTime = 0.0f;

    private void Awake()
    {
        Debug.Log(gameObject + " Projectile Created");
        ProjectileCreated(gameObject);
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
                UIManager.Instance.TxtRemainBounces.text = "Remaining Bounces: " + RemainingBounces.ToString();
            }
        }
        else if (collision.collider.tag == "Target")
        {
            Destroy(gameObject);
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
