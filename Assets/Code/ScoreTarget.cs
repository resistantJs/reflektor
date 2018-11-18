using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTarget : MonoBehaviour
{
    [SerializeField]
    private int m_scoreValue = 100;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("Hit");
            GameManager.Instance.Score = m_scoreValue;
            GameManager.Instance.TargetHit();
            Destroy(collision.collider.gameObject);
        }
    }
}
