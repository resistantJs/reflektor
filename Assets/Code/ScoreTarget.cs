using UnityEngine;

/// <summary>
/// Class defining the behaviour of a Score Target
/// Score Targets are stationary objects in levels. The player must hit the Score Target with one of their projetiles to clear the level
/// </summary>
public class ScoreTarget : MonoBehaviour
{
    /// <summary>
    /// The score value of the Score Target
    /// This amount is credited to the player's total score when one of their projectiles hits the target
    /// </summary>
    [SerializeField]
    private int m_scoreValue = 100;

    /// <summary>
    /// Event called when a projectile hits the Score Target
    /// Delegates receive an integer representing the target's score value
    /// </summary>
    /// <param name="_scoreTargetValue"></param>
    public delegate void TargetHitEvent(int _scoreTargetValue);
    public static event TargetHitEvent TargetHit;

    /// <summary>
    /// If the Score Target is hit by a projectile, calls the TargetHit event
    /// The GameManager uses this event to determine if the player has cleared the level
    /// </summary>
    /// <param name="collision">Information about the collision</param>
    private void OnCollisionEnter(Collision collision)
    {
        /// Checks if the object that struck the target was marked as a projectile
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("Score Target Hit");

            TargetHit(m_scoreValue);
        }
    }
}