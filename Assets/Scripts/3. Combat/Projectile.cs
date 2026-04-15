using UnityEngine;

[RequireComponent(typeof(ProjectileMovement))]
[RequireComponent(typeof(ProjectileImpact))]
public class Projectile : MonoBehaviour
{
    private ProjectileMovement _movement;
    private ProjectileImpact _impact;

    public GameObject Owner { get; private set; }
    public int Damage { get; private set; }

    private void Awake()
    {
        _movement = GetComponent<ProjectileMovement>();
        _impact = GetComponent<ProjectileImpact>();
    }

    public void Initialize(Vector3 direction, float speed, int damage, float lifetime, GameObject owner)
    {
        Damage = damage;
        Owner = owner;

        _movement.Initialize(direction, speed, lifetime);
        _impact.Initialize(this);
    }
}