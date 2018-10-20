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

    // Use this for initialization
    void Start()
    {

	}
	
	// Update is called once per frame
	void Update()
    {
        Vector3 _mouseWorldPos;

        _mouseWorldPos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        //_mouseWorldPos.y = m_launcherTransform.position.y;
        Debug.Log("Mouse pos in world space: " + _mouseWorldPos);

        //m_launcherTransform.LookAt(_mouseWorldPos);
	}
}
