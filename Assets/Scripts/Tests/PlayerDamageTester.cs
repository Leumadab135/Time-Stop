using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDamageTester : MonoBehaviour
{
    [SerializeField] private Health _playerHealth;

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.kKey.wasPressedThisFrame)
        {
            _playerHealth.ReceiveDamage(1);
        }
    }
}