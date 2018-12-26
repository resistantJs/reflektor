using UnityEngine;

public class ShotObstacle : MonoBehaviour, IObstacle
{
    public void TriggerObstacleEffect(GameObject _projectile)
    {
        _projectile.GetComponent<Projectile>().RestoreBounces(1);

        Debug.Log("ShotObstacle: Restored one bounce");

        Destroy(gameObject);
    }
}
