using System.Collections;
using UnityEngine;

public class PlayerTimeStopAnticipation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _playerVisual;

    [Header("Timing")]
    [SerializeField] private float _boomTime = 0.6f;
    [SerializeField] private float _downDuration = 0.22f;
    [SerializeField] private float _recoverDurationAfterBoom = 0.14f;

    [Header("Animation")]
    [SerializeField] private float _downTiltAngle = 18f;

    private Quaternion _initialLocalRotation;
    private bool _isPlaying;

    public bool IsPlaying => _isPlaying;
    public float BoomTime => _boomTime;

    private void Awake()
    {
        if (_playerVisual != null)
        {
            _initialLocalRotation = _playerVisual.localRotation;
        }
    }

    public IEnumerator PlayAnticipation(System.Action onBoom)
    {
        if (_playerVisual == null || _isPlaying)
        {
            yield break;
        }

        _isPlaying = true;

        Quaternion downRotation = _initialLocalRotation * Quaternion.Euler(_downTiltAngle, 0f, 0f);

        float upDurationBeforeBoom = Mathf.Max(0.01f, _boomTime - _downDuration);

        float timer = 0f;

        while (timer < _downDuration)
        {
            timer += Time.deltaTime;
            float t = _downDuration > 0f ? Mathf.Clamp01(timer / _downDuration) : 1f;
            _playerVisual.localRotation = Quaternion.Slerp(_initialLocalRotation, downRotation, t);
            yield return null;
        }

        timer = 0f;

        while (timer < upDurationBeforeBoom)
        {
            timer += Time.deltaTime;
            float t = upDurationBeforeBoom > 0f ? Mathf.Clamp01(timer / upDurationBeforeBoom) : 1f;
            _playerVisual.localRotation = Quaternion.Slerp(downRotation, _initialLocalRotation, t);
            yield return null;
        }

        _playerVisual.localRotation = _initialLocalRotation;
        onBoom?.Invoke();

        timer = 0f;

        while (timer < _recoverDurationAfterBoom)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        _playerVisual.localRotation = _initialLocalRotation;
        _isPlaying = false;
    }
}