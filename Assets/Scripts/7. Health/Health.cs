using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int _maxHealth = 3;
    [SerializeField] private bool _destroyOnDeath = false;

    public int MaxHealth => _maxHealth;
    public int CurrentHealth { get; private set; }
    public bool IsDead { get; private set; }

    public event Action<int, int> OnHealthChanged;
    public event Action<int> OnDamaged;
    public event Action OnDied;

    private void Awake()
    {
        CurrentHealth = _maxHealth;
        NotifyHealthChanged();
    }

    public void ReceiveDamage(int damageAmount)
    {
        if (IsDead)
        {
            return;
        }

        if (damageAmount <= 0)
        {
            return;
        }

        CurrentHealth = Mathf.Max(CurrentHealth - damageAmount, 0);

        OnDamaged?.Invoke(damageAmount);
        NotifyHealthChanged();

        if (CurrentHealth == 0)
        {
            Die();
        }
    }

    public void RestoreFullHealth()
    {
        IsDead = false;
        CurrentHealth = _maxHealth;
        NotifyHealthChanged();
    }

    private void Die()
    {
        if (IsDead)
        {
            return;
        }

        IsDead = true;
        OnDied?.Invoke();

        if (_destroyOnDeath)
        {
            Destroy(gameObject);
        }
    }

    private void NotifyHealthChanged()
    {
        OnHealthChanged?.Invoke(CurrentHealth, _maxHealth);
    }
}