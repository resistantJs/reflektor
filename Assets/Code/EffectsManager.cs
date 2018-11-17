using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour {

    private static EffectsManager m_instance = null;

    [SerializeField]
    private CameraShake m_cameraShake = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public static EffectsManager Instance
    {
        get
        {
            return m_instance;
        }

        private set
        {
            m_instance = value;
        }
    }

    public void ShakeScreen(float _duration, float _magnitude, float _perShakeReduction)
    {
        StartCoroutine(m_cameraShake.Shake(_duration, _magnitude, 0.15f));
    }
}
