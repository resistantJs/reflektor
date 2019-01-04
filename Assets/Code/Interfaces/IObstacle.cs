using UnityEngine;

/// <summary>
/// Interface for triggering obstacle functionality upon collision with a projectile
/// </summary>
public interface IObstacle
{
    /// <summary>
    /// Contains behaviour code to be triggered upon a projectile striking the obstacle
    /// Receives reference to project to enable obstacle behaviour to effect it
    /// </summary>
    /// <param name="_projectile">The projectile that struck this obstacle</param>
    void TriggerObstacleEffect(GameObject _projectile);
}
