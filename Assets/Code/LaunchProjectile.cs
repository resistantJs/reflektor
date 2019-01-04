using UnityEngine;

public class LaunchProjectile : MonoBehaviour
{
    private Rigidbody m_projectileInstance = null;

    [SerializeField]
    private Rigidbody projectilePrefab = null;

    [SerializeField]
    public Transform projectileStart = null;

    [SerializeField]
    private float launchVelocity = 5000.0f;

    private void OnEnable()
    {
        Projectile.ProjectileStateChanged += ClearProjectileInstance;
    }

    private void OnDisable()
    {
        Projectile.ProjectileStateChanged -= ClearProjectileInstance;
    }

    // Update is called once per frame
    private void Update()
    {
        if (InputManager.Instance.Fire && m_projectileInstance == null && GameManager.Instance.RemainingProjectiles > 0 && GameManager.Instance.EnablePlay)
        {
            m_projectileInstance = Instantiate(projectilePrefab, projectileStart.position, projectileStart.rotation) as Rigidbody;

            m_projectileInstance.AddForce(projectileStart.forward * launchVelocity);

            GameManager.Instance.UseProjectile();

            AudioManager.Instance.Stop("LauncherFire");
            AudioManager.Instance.Play("LauncherFire");
        }
    }

    private void ClearProjectileInstance(GameObject _projectile)
    {
        if (_projectile == null)
        {
            m_projectileInstance = null;
        }
    }
}
