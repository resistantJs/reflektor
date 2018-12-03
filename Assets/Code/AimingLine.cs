using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AimingLine : MonoBehaviour {

    private LineRenderer m_lineRenderer;
    private float m_projectileRadius = 0.0f;

    [SerializeField]
    private GameObject m_projectile;
    [SerializeField]
    private LayerMask m_layers;
    [SerializeField]
    private float m_maxDistance = 1000f;
    [SerializeField]
    private float m_lengthMultiplier = 0.25f;

    private void Awake()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
        m_projectileRadius = m_projectile.GetComponent<SphereCollider>().radius * m_projectile.GetComponent<Transform>().localScale.z;
    }

    private void Start()
    {
        m_lineRenderer.startWidth = m_projectileRadius * 2 * m_lineRenderer.gameObject.GetComponent<Transform>().localScale.z;
        m_lineRenderer.endWidth = m_projectileRadius * 2 * m_lineRenderer.gameObject.GetComponent<Transform>().localScale.z;
    }

    // Update is called once per frame
    void Update()
    {
        DrawAimLine();
        Debug.Log("Sphere collider radius: " + m_projectileRadius);
	}

    void DrawAimLine()
    {
        RaycastHit _hit;

        if (Physics.SphereCast(transform.position, m_projectileRadius, transform.forward, out _hit, m_maxDistance, m_layers))
        {
            Vector3 _incomingVec = _hit.point - transform.position;

            Vector3 _reflectVec = Vector3.Reflect(_incomingVec, _hit.normal);

            Debug.DrawLine(transform.position, _hit.point, Color.red);
            Debug.DrawRay(_hit.point, _reflectVec);

            m_lineRenderer.SetPosition(0, transform.position);
            m_lineRenderer.SetPosition(1, _hit.point);
            m_lineRenderer.SetPosition(2, _hit.point + _reflectVec * m_lengthMultiplier);
        }
    }
}
