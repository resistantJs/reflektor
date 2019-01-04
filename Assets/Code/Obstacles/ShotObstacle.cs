using UnityEngine;

/// <summary>
/// Class defining the behaviour of a ShotObstacle
/// ShotObstacles are destroyed upon contact with a projectile, but restore one bounce to that projectile
/// </summary>
public class ShotObstacle : MonoBehaviour, IObstacle
{
    /// <summary>
    /// Contains behaviour code to be triggered upon a projectile striking the obstacle
    /// Receives reference to project to enable obstacle behaviour to effect it
    /// </summary>
    /// <param name="_projectile">The projectile that struck this obstacle</param>
    public void TriggerObstacleEffect(GameObject _projectile)
    {
        /// Uses the projectile reference to call its RestoreBounces method, restoring 1 bounce to it
        _projectile.GetComponent<Projectile>().RestoreBounces(1);

        Debug.Log("ShotObstacle: Restored one bounce");

        /// Destroys this obstacle
        Destroy(gameObject);
    }
}
