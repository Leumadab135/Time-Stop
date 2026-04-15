using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TimeStopCameraEffects : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TimeStopManager _timeStopManager;

    [Header("FOV")]
    [SerializeField] private float _normalFOV = 60f;
    [SerializeField] private float _timeStopFOV = 74f;
    [SerializeField] private float _fovLerpSpeed = 8f;

    private Camera _camera;
    private float _targetFOV;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _targetFOV = _normalFOV;
        _camera.fieldOfView = _normalFOV;
    }

    private void OnEnable()
    {
        if (_timeStopManager != null)
        {
            _timeStopManager.OnStateChanged += HandleStateChanged;
        }
    }

    private void OnDisable()
    {
        if (_timeStopManager != null)
        {
            _timeStopManager.OnStateChanged -= HandleStateChanged;
        }
    }

    private void Update()
    {
        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, _targetFOV, _fovLerpSpeed * Time.deltaTime);
    }

    private void HandleStateChanged(TimeStopState state)
    {
        switch (state)
        {
            case TimeStopState.Normal:
                _targetFOV = _normalFOV;
                break;

            case TimeStopState.Entering:
            case TimeStopState.Stopped:
            case TimeStopState.Exiting:
                _targetFOV = _timeStopFOV;
                break;
        }
    }
}