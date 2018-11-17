using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowObstacle : MonoBehaviour {

    private Animator m_animator = null;

    private void Awake()
    {
        m_animator = gameObject.GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Projectile")
        {
            Debug.Log("Glow obstacle collision enter");
            m_animator.SetBool("hit", true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Projectile")
        {
            Debug.Log("Glow obstacle collision exit");
            m_animator.SetBool("hit", false);
        }
    }
}
