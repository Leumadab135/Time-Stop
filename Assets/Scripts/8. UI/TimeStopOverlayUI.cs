using UnityEngine;
using UnityEngine.UI;

public class TimeStopOverlayUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TimeStopManager _timeStopManager;
    [SerializeField] private Image _overlayImage;

    [Header("Overlay")]
    [SerializeField] private float _targetAlpha = 0.14f;
    [SerializeField] private float _fadeSpeed = 6f;

    private float _desiredAlpha;

    private void Awake()
    {
        if (_overlayImage != null)
        {
            Color color = _overlayImage.color;
            color.a = 0f;
            _overlayImage.color = color;
        }
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
        if (_overlayImage == null)
        {
            return;
        }

        Color color = _overlayImage.color;
        color.a = Mathf.Lerp(color.a, _desiredAlpha, _fadeSpeed * Time.deltaTime);
        _overlayImage.color = color;
    }

    private void HandleStateChanged(TimeStopState state)
    {
        switch (state)
        {
            case TimeStopState.Normal:
                _desiredAlpha = 0f;
                break;

            case TimeStopState.Entering:
            case TimeStopState.Stopped:
            case TimeStopState.Exiting:
                _desiredAlpha = _targetAlpha;
                break;
        }
    }
}