using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerInputReader))]
public class PlayerTimeStopActivator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TimeStopManager _timeStopManager;
    [SerializeField] private PlayerTimeStopAnticipation _anticipationController;
    [SerializeField] private TimeStopAbilityAudio _abilityAudio;

    private PlayerInputReader _inputReader;
    private bool _isActivating;
    private bool _boomTriggeredThisActivation;

    private void Awake()
    {
        _inputReader = GetComponent<PlayerInputReader>();
    }

    private void Update()
    {
        if (_timeStopManager == null || _anticipationController == null)
        {
            return;
        }

        if (_inputReader.TimeStopPressedThisFrame && !_isActivating && _timeStopManager.CanActivateTimeStop)
        {
            StartCoroutine(ActivateRoutine());
        }
    }

    private IEnumerator ActivateRoutine()
    {
        _isActivating = true;
        _boomTriggeredThisActivation = false;

        if (_abilityAudio != null)
        {
            _abilityAudio.PlayAbilitySound();
        }

        yield return _anticipationController.PlayAnticipation(HandleBoomMoment);

        _isActivating = false;
    }

    private void HandleBoomMoment()
    {
        if (_boomTriggeredThisActivation)
        {
            return;
        }

        _boomTriggeredThisActivation = true;

        if (_timeStopManager != null)
        {
            _timeStopManager.ActivateTimeStop();
        }
    }
}