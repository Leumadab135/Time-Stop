using UnityEngine;

public class ProjectileMovement : MonoBehaviour, ITimeAffectable
{
    private Vector3 _direction;
    private float _speed;
    private float _remainingLifetime;
    private float _timeFactor = 1f;
    private bool _isInitialized;

    private TimeStopManager _timeStopManager;

    public void Initialize(Vector3 direction, float speed, float lifetime)
    {
        _direction = direction.normalized;
        _speed = speed;
        _remainingLifetime = lifetime;
        _isInitialized = true;

        _timeStopManager = FindFirstObjectByType<TimeStopManager>();

        if (_timeStopManager != null)
        {
            _timeStopManager.RegisterAffectable(this);
        }
    }

    public void SetTimeFactor(float timeFactor)
    {
        _timeFactor = timeFactor;
    }

    private void Update()
    {
        if (!_isInitialized)
        {
            return;
        }

        float scaledDeltaTime = Time.deltaTime * _timeFactor;

        transform.position += _direction * _speed * scaledDeltaTime;
        _remainingLifetime -= scaledDeltaTime;

        if (_remainingLifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (_timeStopManager != null)
        {
            _timeStopManager.UnregisterAffectable(this);
        }
    }
}