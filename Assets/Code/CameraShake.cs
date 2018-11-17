using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float _duration, float _magnitude, float _perShakeReduction)
    {
        Vector3 _originalPos = transform.localPosition;

        float _elapsedTime = 0.0f;

        float _shakeMultiplier = 1.0f;

        float _shakeReduction = _magnitude / _duration;

        while (_elapsedTime < _duration)
        {
            float _x = Random.Range(-1f, 1f) * _magnitude * _shakeMultiplier;
            float _z = Random.Range(-1f, 1f) * _magnitude * _shakeMultiplier;

            transform.localPosition = new Vector3(_x, _originalPos.y, _z);

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

        transform.localPosition = _originalPos;
    }
}
