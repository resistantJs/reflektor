using UnityEngine;

/* Interface for triggering obstacle functionality upon collision with a projectile
 */
public interface IObstacle
{
    /* Contains behaviour code to be triggered upon a projectile striking the obstacle
     * Receives reference to project to enable obstacle behaviour to effect it
     */
    void TriggerObstacleEffect(GameObject _projectile);
}
