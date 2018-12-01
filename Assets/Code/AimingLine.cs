using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AimingLine : MonoBehaviour {

    private LineRenderer lineRenderer;

    [SerializeField]
    private LayerMask m_layers;
    [SerializeField]
    private float m_maxDistance = 1000f;
    [SerializeField]
    private float m_lengthMultiplier = 0.25f;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
	
	// Update is called once per frame
	void Update()
    {
        DrawAimLine();
	}

    void DrawAimLine()
    {
        RaycastHit _hit;

        if (Physics.Raycast(transform.position, transform.forward, out _hit, m_maxDistance, m_layers))
        {
            Vector3 _incomingVec = _hit.point - transform.position;

            Vector3 _reflectVec = Vector3.Reflect(_incomingVec, _hit.normal);

            Debug.DrawLine(transform.position, _hit.point, Color.red);
            Debug.DrawRay(_hit.point, _reflectVec);

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, _hit.point);
            lineRenderer.SetPosition(2, _hit.point + _reflectVec * m_lengthMultiplier);
        }
    }
}
