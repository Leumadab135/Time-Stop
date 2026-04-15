using UnityEngine;

public class WeaponShooter : MonoBehaviour
{
    [SerializeField] private Weapon _equippedWeapon;

    public Weapon EquippedWeapon => _equippedWeapon;

    public void Shoot(Vector3 targetPoint)
    {
        if (_equippedWeapon == null || _equippedWeapon.ProjectilePrefab == null || _equippedWeapon.Muzzle == null)
        {
            return;
        }

        Vector3 muzzlePosition = _equippedWeapon.Muzzle.position;
        Vector3 shotDirection = (targetPoint - muzzlePosition).normalized;

        Projectile projectileInstance = Instantiate(
            _equippedWeapon.ProjectilePrefab,
            muzzlePosition,
            Quaternion.LookRotation(shotDirection)
        );

        projectileInstance.Initialize(
            shotDirection,
            _equippedWeapon.ProjectileSpeed,
            _equippedWeapon.Damage,
            _equippedWeapon.ProjectileLifetime,
            gameObject
        );

        PlayShotSound();
    }

    private void PlayShotSound()
    {
        if (_equippedWeapon.AudioSource == null || _equippedWeapon.ShotClip == null)
        {
            return;
        }

        _equippedWeapon.AudioSource.PlayOneShot(_equippedWeapon.ShotClip);
    }
}