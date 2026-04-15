using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopManager : MonoBehaviour
{
    [Header("Durations")]
    [SerializeField] private float _enterDuration = 1.2f;
    [SerializeField] private float _stoppedDuration = 5f;
    [SerializeField] private float _exitDuration = 0.5f;

    public float CurrentTimeFactor { get; private set; } = 1f;
    public TimeStopState CurrentState { get; private set; } = TimeStopState.Normal;

    public event Action<TimeStopState> OnStateChanged;
    public event Action<float> OnTimeFactorChanged;

    private readonly List<ITimeAffectable> _registeredAffectables = new();

    private float _stateTimer;

    public bool CanActivateTimeStop => CurrentState == TimeStopState.Normal;

    public void RegisterAffectable(ITimeAffectable affectable)
    {
        if (affectable == null)
        {
            return;
        }

        if (_registeredAffectables.Contains(affectable))
        {
            return;
        }

        _registeredAffectables.Add(affectable);
        affectable.SetTimeFactor(CurrentTimeFactor);
    }

    public void UnregisterAffectable(ITimeAffectable affectable)
    {
        if (affectable == null)
        {
            return;
        }

        _registeredAffectables.Remove(affectable);
    }

    public void ActivateTimeStop()
    {
        if (!CanActivateTimeStop)
        {
            return;
        }

        ChangeState(TimeStopState.Entering);
        _stateTimer = 0f;
    }

    private void Update()
    {
        UpdateStateMachine();
        ApplyCurrentTimeFactor();
    }

    private void UpdateStateMachine()
    {
        switch (CurrentState)
        {
            case TimeStopState.Normal:
                SetCurrentTimeFactor(1f);
                break;

            case TimeStopState.Entering:
                UpdateEntering();
                break;

            case TimeStopState.Stopped:
                UpdateStopped();
                break;

            case TimeStopState.Exiting:
                UpdateExiting();
                break;
        }
    }

    private void UpdateEntering()
    {
        _stateTimer += Time.deltaTime;

        float normalizedTime = _enterDuration > 0f ? Mathf.Clamp01(_stateTimer / _enterDuration) : 1f;
        float factor = Mathf.Lerp(1f, 0f, normalizedTime);

        SetCurrentTimeFactor(factor);

        if (normalizedTime >= 1f)
        {
            ChangeState(TimeStopState.Stopped);
            _stateTimer = 0f;
            SetCurrentTimeFactor(0f);
        }
    }

    private void UpdateStopped()
    {
        _stateTimer += Time.deltaTime;
        SetCurrentTimeFactor(0f);

        if (_stateTimer >= _stoppedDuration)
        {
            ChangeState(TimeStopState.Exiting);
            _stateTimer = 0f;
        }
    }

    private void UpdateExiting()
    {
        _stateTimer += Time.deltaTime;

        float normalizedTime = _exitDuration > 0f ? Mathf.Clamp01(_stateTimer / _exitDuration) : 1f;
        float factor = Mathf.Lerp(0f, 1f, normalizedTime);

        SetCurrentTimeFactor(factor);

        if (normalizedTime >= 1f)
        {
            ChangeState(TimeStopState.Normal);
            _stateTimer = 0f;
            SetCurrentTimeFactor(1f);
        }
    }

    private void SetCurrentTimeFactor(float newFactor)
    {
        if (Mathf.Approximately(CurrentTimeFactor, newFactor))
        {
            return;
        }

        CurrentTimeFactor = newFactor;
        OnTimeFactorChanged?.Invoke(CurrentTimeFactor);
    }

    private void ChangeState(TimeStopState newState)
    {
        if (CurrentState == newState)
        {
            return;
        }

        CurrentState = newState;
        OnStateChanged?.Invoke(CurrentState);
    }

    private void ApplyCurrentTimeFactor()
    {
        for (int i = _registeredAffectables.Count - 1; i >= 0; i--)
        {
            ITimeAffectable affectable = _registeredAffectables[i];

            if (affectable == null)
            {
                _registeredAffectables.RemoveAt(i);
                continue;
            }

            affectable.SetTimeFactor(CurrentTimeFactor);
        }
    }
}