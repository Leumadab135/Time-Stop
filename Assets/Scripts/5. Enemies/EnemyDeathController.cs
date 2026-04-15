using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(EnemyStateMachine))]
public class EnemyDeathController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _deathEffectPrefab;
    [SerializeField] private GameObject _visualRoot;

    private Health _health;
    private EnemyStateMachine _stateMachine;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _stateMachine = GetComponent<EnemyStateMachine>();
    }

    private void OnEnable()
    {
        _health.OnDied += HandleDeath;
    }

    private void OnDisable()
    {
        _health.OnDied -= HandleDeath;
    }

    private void HandleDeath()
    {
        _stateMachine.SetState(EnemyState.Dead);

        if (_deathEffectPrefab != null)
        {
            Instantiate(_deathEffectPrefab, transform.position + Vector3.up, Quaternion.identity);
        }

        if (_visualRoot != null)
        {
            _visualRoot.SetActive(false);
        }

        Destroy(gameObject, 0.1f);
    }
}