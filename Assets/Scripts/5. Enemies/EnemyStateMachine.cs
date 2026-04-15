using System;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnemyStateMachine : MonoBehaviour
{
    public EnemyState CurrentState { get; private set; } = EnemyState.Idle;

    public event Action<EnemyState> OnStateChanged;

    public bool IsDead => CurrentState == EnemyState.Dead;

    public void SetState(EnemyState newState)
    {
        if (CurrentState == newState)
        {
            return;
        }

        CurrentState = newState;
        OnStateChanged?.Invoke(CurrentState);
    }
}