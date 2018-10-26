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
        if (Input.GetButtonDown("Fire1") && m_projectileInstance == null && GameManager.instance.RemainingProjectiles > 0 && GameManager.instance.EnablePlay)
        {
            m_projectileInstance = Instantiate(projectilePrefab, projectileStart.position, projectileStart.rotation) as Rigidbody;

            m_projectileInstance.AddForce(projectileStart.forward * launchVelocity);

            GameManager.instance.UseProjectile();
        }
    }
}
