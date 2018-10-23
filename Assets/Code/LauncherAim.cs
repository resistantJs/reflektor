using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherAim : MonoBehaviour
{
    [SerializeField]
    private Camera m_camera;
    private Transform m_launcherTransform;

    private void Awake()
    {
        m_launcherTransform = GameObject.Find("Launcher").GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update()
    {
        Ray _ray = m_camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit _hit;
        if (Physics.Raycast(_ray, out _hit))
        {
            transform.LookAt(new Vector3(_hit.point.x, transform.position.y, _hit.point.z));
        }
    }
}
