using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherAim : MonoBehaviour
{
    private Camera m_camera;

    private void Awake()
    {
        m_camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.EnablePlay)
        {
            Ray _ray = m_camera.ScreenPointToRay(InputManager.Instance.MousePos);

            RaycastHit _hit;

            if (Physics.Raycast(_ray, out _hit))
            {
                transform.LookAt(new Vector3(_hit.point.x, transform.position.y, _hit.point.z));
            }
        }
    }
}