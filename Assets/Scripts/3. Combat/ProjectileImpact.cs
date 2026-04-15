using UnityEngine;

public class ProjectileImpact : MonoBehaviour
{
    [Header("Collision")]
    [SerializeField] private LayerMask _impactMask;
    [SerializeField] private float _collisionRadius = 0.05f;

    [Header("Impact VFX")]
    [SerializeField] private GameObject _impactEffectPrefab;

    private Projectile _projectile;
    private Vector3 _lastPosition;
    private bool _isInitialized;

    public void Initialize(Projectile projectile)
    {
        _projectile = projectile;
        _lastPosition = transform.position;
        _isInitialized = true;
    }

    private void Update()
    {
        if (!_isInitialized)
        {
            return;
        }

        Vector3 currentPosition = transform.position;
        Vector3 direction = currentPosition - _lastPosition;
        float distance = direction.magnitude;

        if (distance > 0f)
        {
            direction.Normalize();

            if (Physics.SphereCast(
                    _lastPosition,
                    _collisionRadius,
                    direction,
                    out RaycastHit hit,
                    distance,
                    _impactMask,
                    QueryTriggerInteraction.Ignore))
            {
                HandleImpact(hit);
                return;
            }
        }

        _lastPosition = currentPosition;
    }

    private void HandleImpact(RaycastHit hit)
    {
        if (_projectile == null)
        {
            Destroy(gameObject);
            return;
        }

        Transform ownerRoot = _projectile.Owner != null ? _projectile.Owner.transform.root : null;
        Transform hitRoot = hit.collider.transform.root;

        if (ownerRoot != null && hitRoot == ownerRoot)
        {
            return;
        }

        IDamageable damageable = hit.collider.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            damageable.ReceiveDamage(_projectile.Damage);
        }

        SpawnImpactEffect(hit);

        Destroy(gameObject);
    }

    private void SpawnImpactEffect(RaycastHit hit)
    {
        if (_impactEffectPrefab == null)
        {
            return;
        }

        Quaternion impactRotation = Quaternion.LookRotation(hit.normal);
        Instantiate(_impactEffectPrefab, hit.point, impactRotation);
    }
}