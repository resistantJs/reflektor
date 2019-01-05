using UnityEngine;

/// <summary>
/// Class defining the projectile launcher behaviour of the Launcher
/// </summary>
public class LaunchProjectile : MonoBehaviour
{
    /// <summary>
    /// Reference to the projectile fired from the Launcher
    /// </summary>
    private Rigidbody m_projectileInstance = null;
    /// <summary>
    /// Reference to the projectile prefab, from launched projectile instances are created
    /// </summary>
    [SerializeField]
    private Rigidbody projectilePrefab = null;
    /// <summary>
    /// Reference to the position projectiles are launched from
    /// This position is just in front of the Launcher to prevent projectiles appearing inside it
    /// </summary>
    [SerializeField]
    public Transform projectileStart = null;
    /// <summary>
    /// The velocity at which projectiles are launched
    /// </summary>
    [SerializeField]
    private float launchVelocity = 5000.0f;

    /// <summary>
    /// Subscribes to relevant events
    /// </summary>
    private void OnEnable()
    {
        /// Subscribes to the Projectile.ProjectileStageChanged to be notified when a projectile is created or destroyed
        Projectile.ProjectileStateChanged += ClearProjectileInstance;
    }

    /// <summary>
    /// Unsubscribes from events the object is disabled
    /// </summary>
    private void OnDisable()
    {
        Projectile.ProjectileStateChanged -= ClearProjectileInstance;
    }

    /// <summary>
    /// Launches a projectile in the direction the Launcher is pointing, if the conditions are correct
    /// </summary>
    private void Update()
    {
        /// A projectile is launched if the player is pressing the Fire button, has projectiles remaining in their stock, and there is no active projectile
        if (InputManager.Instance.Fire && m_projectileInstance == null && GameManager.Instance.RemainingProjectiles > 0 && GameManager.Instance.EnablePlay)
        {
            /// Creates a new projectile instance from the prefab with the position and rotation of the projectile start position
            /// The projectile instance is cast to a RigidBody to enable force to be applied to it
            m_projectileInstance = Instantiate(projectilePrefab, projectileStart.position, projectileStart.rotation) as Rigidbody;

            /// Applies force to the projectile, launcing it in the direction the Launcher is pointing at the specified velocity
            /// Projectiles are not effected by drag, so will never lose their initial velocity
            m_projectileInstance.AddForce(projectileStart.forward * launchVelocity);

            /// Informs the GameManager that a projectile has been fired. The GameManager then decrements the player's projectile stock by 1
            GameManager.Instance.UseProjectile();

            /// Plays the Launcher firing sound effect
            AudioManager.Instance.Stop("LauncherFire");
            AudioManager.Instance.Play("LauncherFire");
        }
    }

    /// <summary>
    /// When a projectile has been destroyed, clears the reference to it in the Launcher
    /// </summary>
    /// <param name="_projectile"></param>
    private void ClearProjectileInstance(GameObject _projectile)
    {
        /// Before projectiles are destroyed, ProjectileStateChanged is called with an argument of null
        if (_projectile == null)
        {
            m_projectileInstance = null;
        }
    }
}
