using UnityEngine;
/// <summary>
/// Class defining the behaviour of a GlowObstacle
/// GlowObstacles visually change upon being struck, reverting to their normal state shortly afterwards
/// </summary>
public class GlowObstacle : MonoBehaviour, IObstacle
{
    /// <summary>
    /// Reference to the object's Animator component
    /// Enables to playing and stopping of GlowObstacle's animation
    /// </summary>
    private Animator m_animator = null;

    /// <summary>
    /// Gets the reference to object's Animator object
    /// </summary>
    private void Awake()
    {
        m_animator = gameObject.GetComponent<Animator>();
    }

    /// <summary>
    /// Contains behaviour code to be triggered upon a projectile striking the obstacle
    /// Receives reference to project to enable obstacle behaviour to effect it
    /// </summary>
    /// <param name="_projectile">The projectile that struck this obstacle</param>
    public void TriggerObstacleEffect(GameObject _projectile)
    {

        Debug.Log("Glow obstacle collision enter");

        /// Sets the value of the hit variable in the GlowObstacle's Animator Controller to true, causing the GlowObstacleHit animation to play
        m_animator.SetBool("hit", true);

        /// After a delay, invokes the StopFlash method to stop the GlowObstacleHit animation from playing
        Invoke("StopFlash", 0.05f);

        /// Stops the GlowObstacleHit sound effect (stopping any existing instances of it to prevent overlap), and then plays it
        AudioManager.Instance.Stop("GlowObstacleHit");
        AudioManager.Instance.Play("GlowObstacleHit");
    }

    /// <summary>
    /// Sets the value of the hit variable in the GlowObstacle Animator Controller to false, causing the GlowObstacleHit animation to stop playing
    /// </summary>
    private void StopFlash()
    {
        Debug.Log("Stopping flash effect");
        m_animator.SetBool("hit", false);
    }
}
