using System.Collections;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    private Transform _camera;
    public Transform startPosition;
    private static bool _shaking;
    private const float Power = 1f;
    private float _duration;
    private Coroutine _shakeRoutine;
    private const float InitialDuration = 0.5f;
    private const float Deceleration = 0.75f;
    
    private void Awake()
    {
        InitialiseVariables();
    }

    private void InitialiseVariables()
    {
        _shaking = false;
        _camera = transform;
        _duration = InitialDuration;
        startPosition.position = _camera.position;
    }

    public void StopShake()
    {
        if (_shakeRoutine != null)
        {
            StopCoroutine(_shakeRoutine);
        }
        _shaking = false;
    }

    public void Shake()
    {
        _shakeRoutine = StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        if (_shaking) yield return null;

        _shaking = true;
        
        while (_shaking)
        {
            _camera.localPosition = startPosition.localPosition + Random.insideUnitSphere * Power;
            _duration -= Time.deltaTime * Deceleration;
            if (_duration <= 0)
            {
                _shaking = false;
            }
            yield return null;
        }
        
        OnShakeStop();
    }

    private void OnShakeStop()
    {
        _shaking = false;
        Reset();
    }

    private void Reset()
    {
        _camera.localPosition = startPosition.localPosition;
        _duration = InitialDuration;
    }
}
