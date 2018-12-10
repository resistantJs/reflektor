using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class ProjectileTrail : MonoBehaviour 
{
    #region Properties

    [SerializeField]
    private GameObject m_projectile = null;

    private TrailRenderer m_trailRender = null;

    private float m_projectileRadius = 0.0f;

    #endregion

    private void Awake()
    {
        m_trailRender = GetComponent<TrailRenderer>();

        m_projectileRadius = m_projectile.GetComponent<SphereCollider>().radius * m_projectile.GetComponent<Transform>().localScale.z;

        m_trailRender.startWidth = m_projectileRadius * 2 * m_trailRender.gameObject.GetComponent<Transform>().localScale.z;
        m_trailRender.endWidth = m_projectileRadius * 2 * m_trailRender.gameObject.GetComponent<Transform>().localScale.z;
    }

}
