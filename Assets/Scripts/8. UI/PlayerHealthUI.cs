using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Health _playerHealth;
    [SerializeField] private Image[] _healthSegments;

    [Header("Visuals")]
    [SerializeField] private Color _activeColor = Color.white;
    [SerializeField] private Color _inactiveColor = new Color(1f, 1f, 1f, 0.2f);

    private void OnEnable()
    {
        if (_playerHealth != null)
        {
            _playerHealth.OnHealthChanged += UpdateHealthUI;
        }

        RefreshImmediately();
    }

    private void OnDisable()
    {
        if (_playerHealth != null)
        {
            _playerHealth.OnHealthChanged -= UpdateHealthUI;
        }
    }

    private void RefreshImmediately()
    {
        if (_playerHealth == null)
        {
            return;
        }

        UpdateHealthUI(_playerHealth.CurrentHealth, _playerHealth.MaxHealth);
    }

    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        for (int i = 0; i < _healthSegments.Length; i++)
        {
            if (_healthSegments[i] == null)
            {
                continue;
            }

            _healthSegments[i].color = i < currentHealth ? _activeColor : _inactiveColor;
        }
    }
}