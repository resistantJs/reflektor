using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EffectsManager : Manager
{
    private static EffectsManager m_instance = null;

    private Camera m_mainCamera = null;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += UpdateReferences;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= UpdateReferences;
    }

    private void Awake()
    {
        SetInstance();
        SetReferences();
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
        if (m_mainCamera != null)
        {
            StartCoroutine(Shake(_duration, _magnitude, _perShakeReduction));
        }
        else
        {
            Debug.Log("No Main Camera in scene");
        }
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

    protected override void UpdateReferences(Scene _scene, LoadSceneMode _mode)
    {
        SetReferences();
    }

    protected override void SetInstance()
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

    protected override void SetReferences()
    {
        m_mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
}
