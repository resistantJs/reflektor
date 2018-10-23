using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectile : MonoBehaviour
{
    public Rigidbody projectilePrefab;
    public Transform projectileStart;
    public float launchVelocity = 5000.0f;

    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetButtonDown("Fire1"))
        {
            Rigidbody projectileInstance;
            projectileInstance = Instantiate(projectilePrefab, projectileStart.position, projectileStart.rotation) as Rigidbody;

            projectileInstance.AddForce(projectileStart.forward * launchVelocity);
        }
	}
}
