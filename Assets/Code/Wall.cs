using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField]
    private float m_impactShakeDuration = 0.5f;
    [SerializeField]
    private float m_impactShakeMagnitude = 1f;
    [SerializeField]
    private float m_impactShakePerShakeReduction = 1f;

    private void OnCollisionEnter(Collision collision)
    {
        EffectsManager.Instance.ShakeScreen(m_impactShakeDuration, m_impactShakeMagnitude, m_impactShakePerShakeReduction);
    }
}