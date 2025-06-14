using UnityEngine;

public class SunMover : MonoBehaviour
{
    private TimeController _timeController;
    private float _playweekDuration = 300f;
    private float _currentAngle = 0f;

    public void Init(TimeController timeController)
    {
        _timeController = timeController;
    }

    private void Update()
    {
        if (_timeController == null || _timeController.IsTimePaused)
            return;

        float rotationPerSecond = 360f / _playweekDuration;
        float deltaRotation = rotationPerSecond * Time.deltaTime;

        transform.Rotate(Vector3.right, deltaRotation);
        _currentAngle += deltaRotation;
    }
}


