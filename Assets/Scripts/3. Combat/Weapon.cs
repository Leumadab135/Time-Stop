using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private Transform _muzzle;

    [Header("Stats")]
    [SerializeField] private float _fireRate = 4f;
    [SerializeField] private float _projectileSpeed = 30f;
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _projectileLifetime = 5f;

    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _shotClip;

    public Projectile ProjectilePrefab => _projectilePrefab;
    public Transform Muzzle => _muzzle;
    public float FireRate => _fireRate;
    public float ProjectileSpeed => _projectileSpeed;
    public int Damage => _damage;
    public float ProjectileLifetime => _projectileLifetime;
    public AudioSource AudioSource => _audioSource;
    public AudioClip ShotClip => _shotClip;
}