using UnityEngine;

[RequireComponent(typeof(Health))]
public class PlayerDeathHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MonoBehaviour[] _componentsToDisableOnDeath;

    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
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
        for (int i = 0; i < _componentsToDisableOnDeath.Length; i++)
        {
            if (_componentsToDisableOnDeath[i] != null)
            {
                _componentsToDisableOnDeath[i].enabled = false;
            }
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Player muerto.");
    }
}