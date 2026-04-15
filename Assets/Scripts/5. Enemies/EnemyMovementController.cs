using UnityEngine;

[RequireComponent(typeof(EnemyStateMachine))]
public class EnemyMovementController : MonoBehaviour, ITimeAffectable
{
    [Header("References")]
    [SerializeField] private Transform _playerTarget;
    [SerializeField] private Transform _visualRoot;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _preferredDistance = 9f;
    [SerializeField] private float _repositionTolerance = 1.5f;

    private EnemyStateMachine _stateMachine;
    private TimeStopManager _timeStopManager;

    private float _timeFactor = 1f;

    private void Awake()
    {
        _stateMachine = GetComponent<EnemyStateMachine>();
    }

    private void OnEnable()
    {
        _timeStopManager = FindFirstObjectByType<TimeStopManager>();

        if (_timeStopManager != null)
        {
            _timeStopManager.RegisterAffectable(this);
        }
    }

    private void OnDisable()
    {
        if (_timeStopManager != null)
        {
            _timeStopManager.UnregisterAffectable(this);
        }
    }

    public void SetTimeFactor(float timeFactor)
    {
        _timeFactor = timeFactor;
    }

    private void Update()
    {
        if (_stateMachine.IsDead || _playerTarget == null)
        {
            return;
        }

        if (_stateMachine.CurrentState != EnemyState.Alert && _stateMachine.CurrentState != EnemyState.Attack)
        {
            return;
        }

        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 toPlayer = _playerTarget.position - transform.position;
        toPlayer.y = 0f;

        float distance = toPlayer.magnitude;

        if (distance < 0.001f)
        {
            return;
        }

        Vector3 direction = Vector3.zero;

        if (distance > _preferredDistance + _repositionTolerance)
        {
            direction = toPlayer.normalized;
        }
        else if (distance < _preferredDistance - _repositionTolerance)
        {
            direction = -toPlayer.normalized;
        }

        transform.position += direction * _moveSpeed * Time.deltaTime * _timeFactor;
    }
}