using UnityEngine;

public class GlowObstacle : MonoBehaviour, IObstacle
{
    #region Properties

    private Animator m_animator = null;

    #endregion

    private void Awake()
    {
        m_animator = gameObject.GetComponent<Animator>();
    }

    public void TriggerObstacleEffect(GameObject _projectile)
    {
        Debug.Log("Glow obstacle collision enter");

        m_animator.SetBool("hit", true);

        Invoke("StopFlash", 0.05f);

        AudioManager.Instance.Stop("GlowObstacleHit");
        AudioManager.Instance.Play("GlowObstacleHit");
    }

    private void StopFlash()
    {
        Debug.Log("Stopping flash effect");
        m_animator.SetBool("hit", false);
        //AudioManager.Instance.Stop("GlowObstacleHit");
    }
}
