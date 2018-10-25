using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTarget : MonoBehaviour
{
    [SerializeField]
    private int m_scoreValue = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("Hit");
            ScoreManager.instance.AddToScore(m_scoreValue);
            Destroy(collision.collider.gameObject);
        }
    }
}
