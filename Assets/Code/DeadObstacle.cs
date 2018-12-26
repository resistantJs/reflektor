using UnityEngine;

public class DeadObstacle : MonoBehaviour, IObstacle
{
    public void TriggerObstacleEffect(GameObject _projectile)
    {
        Debug.Log("DeadObstacle effect triggered");
        _projectile.GetComponent<Projectile>().DestroyProjectileExternal(0);
    }
}
