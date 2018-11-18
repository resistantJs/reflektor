using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour {

    private static EffectsManager m_instance = null;

    [SerializeField]
    private Camera m_mainCamera = null;

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
        StartCoroutine(Shake(_duration, _magnitude, 0.15f));
    }

    private IEnumerator Shake(float _duration, float _magnitude, float _perShakeReduction)
    {
        Vector3 _originalPos = m_mainCamera.transform.localPosition;

        float _elapsedTime = 0.0f;

        float _shakeMultiplier = 1.0f;

        float _shakeReduction = _magnitude / _duration;

        while (_elapsedTime < _duration)
        {
            float _x = Random.Range(-1f, 1f) * _magnitude * _shakeMultiplier;
            float _z = Random.Range(-1f, 1f) * _magnitude * _shakeMultiplier;

            m_mainCamera.transform.localPosition = new Vector3(_x, _originalPos.y, _z);

            _elapsedTime += Time.deltaTime;

            if (_magnitude - _shakeReduction * Time.deltaTime < 0)
            {
                _magnitude = 0;
            }
            else
            {
                _magnitude -= _shakeReduction * Time.deltaTime;

            }

            //_shakeMultiplier -= _perShakeReduction * Time.deltaTime;

            Debug.Log("Magnitude: " + _magnitude);

            yield return null;
        }

        m_mainCamera.transform.localPosition = _originalPos;
    }
}
