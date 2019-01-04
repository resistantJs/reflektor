using UnityEngine;

/// <summary>
/// Class defining the behaviour of a DeadObstacle
/// DeadObstacles instantly destroy any projectile's that collide with them
/// </summary>
public class DeadObstacle : MonoBehaviour, IObstacle
{
    /// <summary>
    /// Contains behaviour code to be triggered upon a projectile striking the obstacle
    /// Receives reference to project to enable obstacle behaviour to effect it
    /// </summary>
    /// <param name="_projectile">The projectile that struck this obstacle</param>
    public void TriggerObstacleEffect(GameObject _projectile)
    {
        Debug.Log("DeadObstacle effect triggered");
        /// Uses the projectile reference to call DestroyProjectileExternal with a delay of 0, causing the projectile that struck the DeadObstacle to be instantly destroyed
        _projectile.GetComponent<Projectile>().DestroyProjectileExternal(0);
    }
}
