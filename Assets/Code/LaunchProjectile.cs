using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectile : MonoBehaviour
{
    public Rigidbody projectilePrefab;
    public Transform projectileStart;
    private Rigidbody m_projectileInstance;

    public float launchVelocity = 5000.0f;

    // Update is called once per frame
    void Update()
    {
        if (InputManager.Instance.Fire && m_projectileInstance == null && GameManager.Instance.RemainingProjectiles > 0 && GameManager.Instance.EnablePlay)
        {
            m_projectileInstance = Instantiate(projectilePrefab, projectileStart.position, projectileStart.rotation) as Rigidbody;

            m_projectileInstance.AddForce(projectileStart.forward * launchVelocity);

            GameManager.Instance.UseProjectile();
        }
    }
}
