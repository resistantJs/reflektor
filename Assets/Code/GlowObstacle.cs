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
            Invoke("StopFlash", 0.05f);
            AudioManager.Instance.Stop("GlowObstacleHit");
            AudioManager.Instance.Play("GlowObstacleHit");
        }
    }

    private void StopFlash()
    {
        Debug.Log("Stopping flash effect");
        m_animator.SetBool("hit", false);
        //AudioManager.Instance.Stop("GlowObstacleHit");
    }
}
