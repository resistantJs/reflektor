using UnityEngine;

[RequireComponent(typeof(Transform))]
public class SlideObstacle : MonoBehaviour 
{
    private Transform m_transform = null;
    [SerializeField]
    private Transform m_startPos = null;
    [SerializeField]
    private Transform m_endPos = null;
    private Transform m_targetPos = null;
    [SerializeField]
    private float m_speed = 5.0f;

    private void Awake()
    {
        m_transform = GetComponent<Transform>();
    }

    private void Start()
    {
        m_transform.position = m_startPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        float _maxStepDistance = m_speed * Time.deltaTime;

        if (transform.position == m_startPos.position)
        {
            m_targetPos = m_endPos;
        }
        else if (transform.position == m_endPos.position)
        {
            m_targetPos = m_startPos;
        }

        m_transform.position = Vector3.MoveTowards(m_transform.position, m_targetPos.position, _maxStepDistance);
    }
}
