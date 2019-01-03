using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(BoxCollider))]
public class BlinkObstacle : MonoBehaviour, IObstacle
{
    private MeshRenderer m_meshRenderer = null;
    private BoxCollider m_boxCollider = null;
    [SerializeField]
    private float m_cyclePeriod = 1.0f;
    private float m_counter = 0.0f;
    private bool m_invisible = false;

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

    public void TriggerObstacleEffect(GameObject _projectile)
    {
        Debug.Log("BlinkObstacle: No strike effect");
    }

    private void Awake()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
        m_boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    private void Update()
    {
        m_counter += Time.deltaTime;

        Cycle();
    }

    private void Cycle()
    {
        if (m_counter >= m_cyclePeriod)
        {
            m_invisible = !m_invisible;
            m_counter = 0.0f;
        }

        if (m_invisible)
        {
            m_meshRenderer.enabled = false;
            m_boxCollider.enabled = false;
        }
        else
        {
            m_meshRenderer.enabled = true;
            m_boxCollider.enabled = true;
        }
    }

}
