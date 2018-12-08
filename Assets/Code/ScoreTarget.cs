using UnityEngine;

public class ScoreTarget : MonoBehaviour
{
    [SerializeField]
    private readonly int m_scoreValue = 100;

    public delegate void TargetHitEvent(int _scoreTargetValue);
    public static event TargetHitEvent TargetHit;

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
