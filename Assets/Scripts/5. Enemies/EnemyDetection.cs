using System;
using UnityEngine;

[RequireComponent(typeof(EnemyStateMachine))]
public class EnemyDetection : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _eyePoint;
    [SerializeField] private Transform _playerTarget;

    [Header("Detection")]
    [SerializeField] private float _detectionRange = 14f;
    [SerializeField] private LayerMask _lineOfSightMask;
    [SerializeField] private bool _requireLineOfSight = true;

    public event Action OnPlayerDetected;

    private EnemyStateMachine _stateMachine;
    private bool _hasDetectedPlayer;

    private void Awake()
    {
        _stateMachine = GetComponent<EnemyStateMachine>();
    }

    private void Update()
    {
        if (_stateMachine.IsDead)
        {
            return;
        }

        EvaluateDetection();
    }

    private void EvaluateDetection()
    {
        if (_playerTarget == null || _eyePoint == null)
        {
            return;
        }

        Vector3 toPlayer = _playerTarget.position - _eyePoint.position;
        float distanceToPlayer = toPlayer.magnitude;

        if (distanceToPlayer > _detectionRange)
        {
            _hasDetectedPlayer = false;

            if (_stateMachine.CurrentState != EnemyState.Idle)
            {
                _stateMachine.SetState(EnemyState.Idle);
            }

            return;
        }

        bool canSeePlayer = true;

        if (_requireLineOfSight)
        {
            Vector3 direction = toPlayer.normalized;

            if (Physics.Raycast(
                    _eyePoint.position,
                    direction,
                    out RaycastHit hit,
                    _detectionRange,
                    _lineOfSightMask,
                    QueryTriggerInteraction.Ignore))
            {
                Transform hitRoot = hit.collider.transform.root;
                canSeePlayer = hitRoot == _playerTarget.root;
            }
            else
            {
                canSeePlayer = false;
            }
        }

        if (!canSeePlayer)
        {
            _hasDetectedPlayer = false;

            if (_stateMachine.CurrentState != EnemyState.Idle)
            {
                _stateMachine.SetState(EnemyState.Idle);
            }

            return;
        }

        if (!_hasDetectedPlayer)
        {
            _hasDetectedPlayer = true;
            OnPlayerDetected?.Invoke();
        }

        if (_stateMachine.CurrentState == EnemyState.Idle)
        {
            _stateMachine.SetState(EnemyState.Alert);
        }
    }
}