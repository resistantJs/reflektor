﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherAim : MonoBehaviour
{
    [SerializeField]
    private Camera m_camera;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.EnablePlay)
        {
            Ray _ray = m_camera.ScreenPointToRay(InputManager.Instance.MousePos);

            RaycastHit _hit;

            if (Physics.Raycast(_ray, out _hit))
            {
                transform.LookAt(new Vector3(_hit.point.x, transform.position.y, _hit.point.z));
                //Debug.Log("Mouse X: " + _hit.point.x + " Mouse Z: " + _hit.point.z);
            }
        }
    }
}