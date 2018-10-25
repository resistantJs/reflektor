using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    private Rigidbody projectileRB;
    private Text m_txtRemainBounces;
    private int m_remainingBounces = 5;

    private void Awake()
    {
        m_txtRemainBounces = GameObject.Find("txtRemainBounces").GetComponent<Text>();
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

        m_txtRemainBounces.text = "Remaining Bounces: " + m_remainingBounces;
    }

    public int RemainingBounces
    {
        get
        {
            return m_remainingBounces;
        }
    }
}
