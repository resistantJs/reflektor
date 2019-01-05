using UnityEngine;

/// <summary>
/// Class defining the aiming behaviour of the Launcher
/// The Launcher is able to rotate around its Y axis, turning to face the position of the player's mouse cursor
/// </summary>
public class LauncherAim : MonoBehaviour
{
    /// <summary>
    /// Reference to the Main Camera in the scene
    /// The Main Camera is used to calculate the mouse cursor's position on screen using Camera.ScreenPointToRay
    /// </summary>
    private Camera m_camera = null;

    /// <summary>
    /// Gets the reference to the Main Camera in the scene
    /// </summary>
    private void Awake()
    {
        m_camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    /// <summary>
    /// If play is enabled, gets the position of the mouse cursor on screen and turns the Launcher to face it
    /// </summary>
    private void Update()
    {
        /// Checks if play is enabled in the GameManager
        /// If play is disabled, the player cannot aim the launcher
        if (GameManager.Instance.EnablePlay)
        {
            /// Fires a ray from the camera through a point on the screen defined by the position of the mouse cursor
            Ray _ray = m_camera.ScreenPointToRay(InputManager.Instance.MousePos);

            /// Stores information about objects hit by the ray
            RaycastHit _hit;

            /// If the ray hits an object, turns the launcher to face it
            if (Physics.Raycast(_ray, out _hit))
            {
                /// Gets a Vector3 representing the position of the player's mouse cursor on screen converted to world space
                /// Uses LookAt to turn the Launcher to face this point
                transform.LookAt(new Vector3(_hit.point.x, transform.position.y, _hit.point.z));
            }
        }
    }
}