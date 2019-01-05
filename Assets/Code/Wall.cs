using UnityEngine;

/// <summary>
/// Class defining the behaviour of a wall object
/// Walls trigger a screenshake effect a projectile strikes them
/// </summary>
public class Wall : MonoBehaviour
{
    /// <summary>
    /// The duration of the shake effect
    /// </summary>
    [SerializeField]
    private float m_impactShakeDuration = 0.5f;
    /// <summary>
    /// The initial magnitude (strength) of the shake effect
    /// </summary>
    [SerializeField]
    private float m_impactShakeMagnitude = 1f;
    /// <summary>
    /// The mangitude reduction multiplier for the shake effect
    /// </summary>
    [SerializeField]
    private float m_impactShakePerShakeReduction = 1f;

    /// <summary>
    /// Triggers the screenshake effect with the specified values when the wall is hit by an object, i.e. a projectile
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        /// Triggers the screenshake effect with the specified parameters
        EffectsManager.Instance.ShakeScreen(m_impactShakeDuration, m_impactShakeMagnitude, m_impactShakePerShakeReduction);
    }
}