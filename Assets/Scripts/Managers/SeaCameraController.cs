using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class SeaCameraController : IDisposable
{
    private Camera _camera;
    private Vector3 _rootPosition;
    private Quaternion _rootRotation;

    private readonly float _swayAmplitude = 0.3f;
    private readonly float _swaySpeed = 0.3f;
    private readonly float _tiltAmplitude = 0.7f;
    private readonly float _tiltSpeed = 0.5f;

    private CancellationTokenSource _cancellationTokenSource;

    public SeaCameraController()
    {
        
    }

    public void Init()
    {
        _camera = Camera.main;

        if (_camera == null)
            Debug.LogWarning("Camera not found!");

        _rootPosition = _camera.transform.position;
        _rootRotation = _camera.transform.rotation;

        _cancellationTokenSource = new CancellationTokenSource();
        AnimateLoop(_cancellationTokenSource.Token);
    }

    private async void AnimateLoop(CancellationToken token)
    {
        float time = UnityEngine.Random.Range(0f, 100f);

        while (!token.IsCancellationRequested)
        {
            time += Time.unscaledDeltaTime; 

            float xOffset = Mathf.Sin(time * _swaySpeed) * _swayAmplitude;
            float tiltAngle = Mathf.Sin(time * _tiltSpeed) * _tiltAmplitude;

            if (_camera != null)
            {
                _camera.transform.position = _rootPosition + _camera.transform.right * xOffset;
                _camera.transform.rotation = _rootRotation * Quaternion.Euler(0f, 0f, tiltAngle);
            }

            await Task.Yield(); 
        }
    }

    public void Disable()
    {
        if (_cancellationTokenSource?.IsCancellationRequested == false)
            _cancellationTokenSource?.Cancel();
    }

    public void Dispose()
    {
        Disable();
        _cancellationTokenSource?.Dispose();
    }
}

