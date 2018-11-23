using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTarget : MonoBehaviour
{
    public delegate void TargetHitEvent(int _scoreTargetValue);
    public static event TargetHitEvent TargetHit;

    [SerializeField]
    private int m_scoreValue = 1000;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("Hit");

            TargetHit(m_scoreValue);

            AudioManager.Instance.Play("LevelComplete");

            Destroy(collision.collider.gameObject);
        }
    }
}
