using UnityEngine;

[RequireComponent(typeof(PlayerInputReader))]
public class PlayerShootController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerAimController _aimController;
    [SerializeField] private WeaponShooter _weaponShooter;

    private PlayerInputReader _inputReader;
    private float _nextAllowedShotTime;

    private void Awake()
    {
        _inputReader = GetComponent<PlayerInputReader>();
    }

    private void Update()
    {
        HandleShooting();
    }

    private void HandleShooting()
    {
        if (_weaponShooter == null || _weaponShooter.EquippedWeapon == null)
        {
            return;
        }

        if (!_inputReader.IsFireHeld)
        {
            return;
        }

        if (Time.time < _nextAllowedShotTime)
        {
            return;
        }

        _weaponShooter.Shoot(_aimController.CurrentAimPoint);

        float fireInterval = 1f / _weaponShooter.EquippedWeapon.FireRate;
        _nextAllowedShotTime = Time.time + fireInterval;
    }
}