using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        EffectsManager.Instance.ShakeScreen(0.5f, 1f, 25f);
    }
}
