using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    private Rigidbody projectileRB;

    private int m_remainingBounces = 5;

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

    public void DestroyProjectile()
    {
        if (m_lifeTime >= 0.25f)
        {
            Debug.Log("Destroy command accepted");
            Destroy(gameObject);
        }
    }
}
