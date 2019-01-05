using UnityEngine;
/// <summary>
/// Class defining the behaviour of a BlinkObstacle
/// BlinkObstacles transition between two states, visible and invisible, after a short period. While invisible, the obstacle cannot be seen or collided with.
/// Requires components: MeshRender, BoxCollider
/// </summary>
[RequireComponent(typeof(MeshRenderer), typeof(BoxCollider))]
public class BlinkObstacle : MonoBehaviour, IObstacle
{
    /// <summary>
    /// Reference to the object's MeshRenderer component
    /// Enables code to affect the MeshRenderer
    /// </summary>
    private MeshRenderer m_meshRenderer = null;
    /// <summary>
    /// Reference to the object's BoxCollider component
    /// Enables code to affect the BoxCollider
    /// </summary>
    private BoxCollider m_boxCollider = null;
    /// <summary>
    /// Stores the amount of time (seconds) to wait between each cycle
    /// </summary>
    [SerializeField]
    private float m_cyclePeriod = 1.0f;
    /// <summary>
    /// Stores the amount of time that has passed (seconds) since the last cycle
    /// </summary>
    private float m_counter = 0.0f;
    /// <summary>
    /// Represents whether the BlinkObstacle is currently invisible
    /// </summary>
    private bool m_invisible = false;

    /// <summary>
    /// Property for accessing obstacle's visibility state
    /// </summary>]
    /// <value>Publicly gets and privately sets the value of bool m_invisible</value>
    public bool Invisible
    {
        get
        {
            return m_invisible;
        }

        private set
        {
            m_invisible = value;
        }
    }

    /// <summary>
    /// Contains behaviour code to be triggered upon a projectile striking the obstacle
    /// Receives reference to project to enable obstacle behaviour to effect it
    /// </summary>
    /// <param name="_projectile">The projectile that struck this obstacle</param>
    public void TriggerObstacleEffect(GameObject _projectile)
    {
        Debug.Log("BlinkObstacle: No strike effect");
    }

    /// <summary>
    /// Calls initialisation methods for the manager
    /// </summary>
    private void Awake()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
        m_boxCollider = GetComponent<BoxCollider>();
    }
    
    /// <summary>
    /// Adds to the counter
    /// Calls Cycle, causing the BlinkObstacle to become invisible if the counter is equal to the cycle period
    /// </summary>
    private void Update()
    {
        /// Adds the time taken to complete last frame to the counter
        m_counter += Time.deltaTime;

        Cycle();
    }

    /// <summary>
    /// Changes the BlinkObstacle between visible and invisible after an amount of time equal to the cycle period has passed
    /// </summary>
    private void Cycle()
    {
        /// Checks if the amount of time specified by the cycle period has passed since the last cycle
        if (m_counter >= m_cyclePeriod)
        {
            /// Inverts the value of m_invisible
            /// e.g. if true, becomes false, and vice versa
            m_invisible = !m_invisible;
            /// Resets the counter after, enabling a new cycle to begin
            m_counter = 0.0f;
        }

        /// If the BlinkObstacle is currently invisible, determined by the value of m_invisible, makes it visible, and vice versa
        if (m_invisible)
        {
            /// While invisible, the obstacle's MeshRenderer and BoxCollider are disabled, making the obstacle invisible on screen and have no collision
            m_meshRenderer.enabled = false;
            m_boxCollider.enabled = false;
        }
        else
        {
            /// While visible, the obstacle's MeshRenderer and BoxCollider are enabling, making it visible and enabling objects (e.g. projectiles) to collide with it
            m_meshRenderer.enabled = true;
            m_boxCollider.enabled = true;
        }
    }
}
