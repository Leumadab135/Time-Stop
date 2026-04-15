using UnityEngine;

[RequireComponent(typeof(EnemyStateMachine))]
public class EnemyAttackController : MonoBehaviour, ITimeAffectable
{
    [Header("References")]
    [SerializeField] private Transform _playerTarget;
    [SerializeField] private Transform _weaponMuzzle;
    [SerializeField] private Projectile _projectilePrefab;

    [Header("Attack")]
    [SerializeField] private float _fireRate = 1.2f;
    [SerializeField] private float _projectileSpeed = 22f;
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _projectileLifetime = 5f;
    [SerializeField] private float _attackRange = 14f;

    [Header("Aim")]
    [SerializeField] private Transform _visualRoot;
    [SerializeField] private float _rotationSpeed = 8f;

    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _shotClip;

    private EnemyStateMachine _stateMachine;
    private TimeStopManager _timeStopManager;

    private float _shotTimer;
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

        float distanceToPlayer = Vector3.Distance(transform.position, _playerTarget.position);

        if (distanceToPlayer > _attackRange)
        {
            if (_stateMachine.CurrentState == EnemyState.Attack)
            {
                _stateMachine.SetState(EnemyState.Alert);
            }

            return;
        }

        RotateTowardsPlayer();

        if (_stateMachine.CurrentState == EnemyState.Alert)
        {
            _stateMachine.SetState(EnemyState.Attack);
        }

        if (_stateMachine.CurrentState != EnemyState.Attack)
        {
            return;
        }

        _shotTimer += Time.deltaTime * _timeFactor;

        float fireInterval = 1f / _fireRate;

        if (_shotTimer >= fireInterval)
        {
            _shotTimer = 0f;
            FireAtPlayer();
        }
    }

    private void RotateTowardsPlayer()
    {
        if (_visualRoot == null)
        {
            return;
        }

        Vector3 direction = _playerTarget.position - _visualRoot.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        _visualRoot.rotation = Quaternion.Slerp(
            _visualRoot.rotation,
            targetRotation,
            _rotationSpeed * Time.deltaTime * Mathf.Max(_timeFactor, 0.001f)
        );
    }

    private void FireAtPlayer()
    {
        if (_projectilePrefab == null || _weaponMuzzle == null)
        {
            return;
        }

        Vector3 targetPosition = _playerTarget.position + Vector3.up * 1.1f;
        Vector3 shotDirection = (targetPosition - _weaponMuzzle.position).normalized;

        Projectile projectileInstance = Instantiate(
            _projectilePrefab,
            _weaponMuzzle.position,
            Quaternion.LookRotation(shotDirection)
        );

        projectileInstance.Initialize(
            shotDirection,
            _projectileSpeed,
            _damage,
            _projectileLifetime,
            gameObject
        );

        PlayShotSound();
    }

    private void PlayShotSound()
    {
        if (_audioSource == null || _shotClip == null)
        {
            return;
        }

        _audioSource.PlayOneShot(_shotClip);
    }
}