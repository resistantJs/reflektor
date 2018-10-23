using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody projectileRB;
    private int m_remainingBounces = 5;

    private void Awake()
    {
        
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

	}

    private void OnCollisionEnter(Collision collision)
    {

        m_remainingBounces--;
        if (m_remainingBounces <= 0)
        {
            Destroy(gameObject);
        }
        Debug.Log(gameObject + " - Remaining Bounces: " + m_remainingBounces);
    }
}
