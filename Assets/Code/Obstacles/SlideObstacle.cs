using UnityEngine;

/// <summary>
/// Class defining the behaviour of a SlideObstacle
/// SlideObstacles move back and forth between two points, a start position and an end position, represented by empty GameObjects
/// Requires components: Transform
/// </summary>
[RequireComponent(typeof(Transform))]
public class SlideObstacle : MonoBehaviour
{
    /// <summary>
    /// Reference to the Transform component of the object
    /// </summary>
    private Transform m_transform = null;
    /// <summary>
    /// Reference to the GameObject representing the obstacle's starting position
    /// Value set in the Inspector
    /// </summary>
    [SerializeField]
    private Transform m_startPos = null;
    /// <summary>
    /// Reference to the GameObject representing the obstacle's end position
    /// Value set in the Inspector
    /// </summary>
    [SerializeField]
    private Transform m_endPos = null;
    /// <summary>
    /// The object that that projectile is moving towards on its current cycle
    /// </summary>
    private Transform m_targetPos = null;
    /// <summary>
    /// The movement speed of the obstacle
    /// Will be multiplied by Time.deltatime before use
    /// </summary>
    [SerializeField]
    private float m_speed = 5.0f;

    /// <summary>
    /// Gets the reference to the object's Transform component
    /// </summary>
    private void Awake()
    {
        m_transform = GetComponent<Transform>();
    }

    /// <summary>
    /// Before beginning play, moves the obstacle's to its start position
    /// </summary>
    private void Start()
    {
        m_transform.position = m_startPos.position;
    }

    /// <summary>
    /// Moves the obstacle towards its target position by an amount every frame
    /// Changes its target position once it has reached it
    /// </summary>
    private void Update()
    {
        /// Sets the amount to move the obstacle by each frame, multiplying by Time.delta to make it framerate independent
        float _maxStepDistance = m_speed * Time.deltaTime;

        /// If the obstacle is at its start postion, change the target position to the end postion, and vice versa
        if (transform.position == m_startPos.position)
        {
            m_targetPos = m_endPos;
        }
        else if (transform.position == m_endPos.position)
        {
            m_targetPos = m_startPos;
        }

        /// Moves the obstacle towards its target position by the specified amount each frame
        m_transform.position = Vector3.MoveTowards(m_transform.position, m_targetPos.position, _maxStepDistance);
    }
}
