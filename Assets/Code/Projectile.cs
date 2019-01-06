using System.Collections;
using UnityEngine;

/// <summary>
/// Class defining the behaviour of a projectile
/// Projectiles are fired from the Launcher
/// The Player's goal in each level to hit the Score Target using a projectile
/// Requires components: SphereCollider
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class Projectile : MonoBehaviour
{
    /// <summary>
    /// The maximum number of bounces a projectile may have
    /// Checked when restoring bounces to ensure it does not gain more than this amount
    /// </summary>
    private readonly int m_maxRemainingBounces = 5;
    /// <summary>
    /// Stores the number of bounces remaining for this projectile
    /// One bounce is consumed each time the projectile strikes a surface
    /// </summary>
    private int m_remainingBounces = 5;
    /// <summary>
    /// The amount of time (seconds) that the projectile instance has been instantiated
    /// </summary>
    private float m_lifeTime = 0.0f;
    /// <summary>
    /// The minimum amount of time the projectile must have been instantiated for before it can be destroyed by the player
    /// </summary>
    [SerializeField]
    private float m_minimumLifeTime = 0.15f;

    /// <summary>
    /// The time to wait (seconds) before the projectile instance is destroyed after it flies out of the level
    /// Upon hit a surface a sixth time, the projectile loses collision and flies out of the level
    /// </summary>
    [SerializeField]
    private float m_noBouncesDestroyDelay = 3f;

    /// <summary>
    /// Event called when a projectile is created or destroyed
    /// Delegates receive a reference to the projectile if it is active, or null if it has been destroyed
    /// </summary>
    /// <param name="_projectile"></param>
    public delegate void ProjectileStateChangeEvent(GameObject _projectile);
    public static event ProjectileStateChangeEvent ProjectileStateChanged;

    private void Awake()
    {
        Debug.Log(gameObject + " Projectile Created");
    }

    /// <summary>
    /// Calls the ProjectileStateChanged event
    /// Updates the GUI to display the number of bounces remaining for this projectile
    /// </summary>
    private void Start()
    {
        ProjectileStateChanged(gameObject);
        UIManager.Instance.SetTxtRemainBounces(RemainingBounces, true);
    }

    /// <summary>
    /// Counts up the lifetime of this projetile instance
    /// Checks if the player is attempting to destroy this projectile prematurely
    /// </summary>
    private void Update()
    {
        m_lifeTime += Time.deltaTime;
        PrematureDestroy();
    }

    /// <summary>
    /// When the projectile strikes an object, consumes a bounce if it has any remaining and, if the object is an obstacle, triggers any on-collision effects it has
    /// </summary>
    /// <param name="collision">Information about the collision</param>
    private void OnCollisionEnter(Collision collision)
    {
        /// If the object that the projectile struck was marked as collidable
        /// Certain objects, such as the Launcher, cannot be collided with
        if (collision.collider.tag == "Collidable")
        {
            /// If the projectile has no bounces remaining...
            if (RemainingBounces <= 0)
            {
                /// Destroys the projectile after a delay
                DestroyProjectile(m_noBouncesDestroyDelay);
            }
            /// If the projectile does have bounces remaining...
            else
            {
                Debug.Log("Hit collidable object");

                /// If the struck object was a instance of IObstacle (the interface for all obstacles), triggers its effect
                TriggerObstacleEffect(collision);

                /// Decrements the projectile's remaining bounces
                DecrementBounces();

                /// Updates the GUI to display the projectiles remaining bounces after reduction
                UIManager.Instance.SetTxtRemainBounces(RemainingBounces, true);

                Debug.Log("Playing projectile collision sound");

                /// Players the projectile collision sound effect
                AudioManager.Instance.Stop("ProjectileBounce");
                AudioManager.Instance.Play("ProjectileBounce");
            }
        }
        /// The projectile is destroyed instantly if it hits the Score Target
        else if (collision.collider.tag == "Target")
        {
            DestroyProjectile(0);
        }
    }

    /// <summary>
    /// Reduces the projectile's remaining bounces by 1
    /// </summary>
    private void DecrementBounces()
    {
        m_remainingBounces--;
    }

    /// <summary>
    /// If the object struck by the projctile was an obstacle (i.e. was an instance of IObstacle), triggers its on-collision effect
    /// </summary>
    /// <param name="_collision">Information about the collision</param>
    private void TriggerObstacleEffect(Collision _collision)
    {
        /// Checks if the struck object has component that implements IObstacle
        if (_collision.collider.gameObject.GetComponent(typeof(IObstacle)) as IObstacle != null)
        {
            /// Gets and stores a reference to the componentr that implements IObstacle
            IObstacle _obstacleScript = _collision.collider.gameObject.GetComponent(typeof(IObstacle)) as IObstacle;
            
            /// Calls the TriggerObstacleEffect method of IObstacle on the component that implements the interface, passing a reference to the projectile
            _obstacleScript.TriggerObstacleEffect(gameObject);

            Debug.Log("Hit object was an IObstacle");
        }
        else
        {
            Debug.Log("Hit object was not an IObstacle");
        }
    }

    /// <summary>
    /// Destroys the currently active projectile on command if the enough time has passed since its instantiation
    /// </summary>
    private void PrematureDestroy()
    {
        /// If the player is pressing the Destroy Projectile button and projectile's current life time is greater than the minimum, destroys it instantly
        if (InputManager.Instance.DestroyProjectile && m_lifeTime >= m_minimumLifeTime)
        {
            Debug.Log("Destroy command accepted");
            DestroyProjectile(0);
        }
    }

    /// <summary>
    /// Starts the DestroyAferDelay coroutine, passing in the provided values
    /// </summary>
    /// <param name="_delay">The amount of time to wait (seconds) before destroyed the projectile</param>
    private void DestroyProjectile(float _delay)
    {
        /// Notifies delegates that the projectile has been destroyed
        /// Even if its instance technically persists untill the delay period has passed, it is of no more use to reference holders
        ProjectileStateChanged(null);

        /// Disables the SphereCollider component of the projectile, letting it fly out of the level and off the screen
        GetComponent<SphereCollider>().enabled = false;

        /// Starts the DestroyAfterDelay coroutine using the provided delay value
        StartCoroutine(DestroyAfterDelay(_delay));
    }

    /// <summary>
    /// Destroys the projectile after the specified amount of time (seconds) has passed
    /// </summary>
    /// <param name="_delay">The amount of time to wait (seconds) before destroyed the projectile</param>
    /// <returns>WaitForSecondsRealtime instruction</returns>
    private IEnumerator DestroyAfterDelay(float _delay)
    {
        /// Waits the specified amount of time before resuming, effectively delaying the destruction of the projectile
        yield return new WaitForSecondsRealtime(_delay);
        
        /// Destroys the projectile
        Destroy(gameObject);
    }

    /// <summary>
    /// Provides public to the DestroyProjectile method
    /// </summary>
    /// <param name="_delay">The amount of time to wait (seconds) before destroyed the projectile</param>
    public void DestroyProjectileExternal(float _delay)
    {
        DestroyProjectile(_delay);
    }

    /// <summary>
    /// Restores bounces to the projectile
    /// </summary>
    /// <param name="_amount">The number of bounces to restore. Projectile may not exceed 5 bounces</param>
    public void RestoreBounces(int _amount)
    {
        /// Amount to restore cannot be less than 1
        if (_amount > 0)
        {
            /// Prevents the projectile from being restored more than the maximum number of bounces
            if (!(m_remainingBounces + _amount <= m_remainingBounces))
            {
                m_remainingBounces = m_maxRemainingBounces;
            }
            else
            {
                m_remainingBounces += _amount;
            }
        }
    }

    /// <summary>
    /// Property to access the number of bounces this projectile has remaining
    /// </summary>
    /// <value>Publicly gets and privately sets the value of int m_remainingBounces</value>
    public int RemainingBounces
    {
        get
        {
            return m_remainingBounces;
        }

        private set
        {
            m_remainingBounces = value;
        }
    }
}
