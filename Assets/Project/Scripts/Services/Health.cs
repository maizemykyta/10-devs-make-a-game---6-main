using Bonjoura.Player;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Bonjoura.Services
{
    public abstract class Health : MonoBehaviour
    {
        [SerializeField] private bool _isPlayer = false;

        [Header("Health")]
        [SerializeField] private int maximumHealth;
        [SerializeField] private int currentHealth;
        
        [SerializeField] private float cooldownHeal;
        [SerializeField] private float cooldownDamage;

        [SerializeField] private Slider _healthBar;
        [SerializeField] private GameObject _losePanle;

        private bool _isCanHeal = true;
        private bool _isCanDamage = true;

        private float _cooldownHealDelay;
        private float _cooldownDamageDelay;

        public int CurrentHealth => currentHealth;
        public int MaximumHealth => maximumHealth;
        
        public event Action OnHealEvent;
        public event Action OnDamageEvent;
        public event Action OnDieEvent;
        public event Action OnValueChange;

        private void Start()
        {
            _healthBar.maxValue = MaximumHealth;
            _healthBar.value = currentHealth;
        }

        public bool Heal(int value)
        {
            if (currentHealth >= maximumHealth) return false;
            if (!_isCanHeal) return false;
            if (!Timer.SimpleTimer(_cooldownHealDelay, cooldownHeal)) return false;
            _cooldownHealDelay = Time.time;
            currentHealth += value;
            currentHealth = Mathf.Clamp(currentHealth, 0, maximumHealth);
            OnHealEvent?.Invoke();
            OnValueChange?.Invoke();
                _isCanHeal = false;
            _healthBar.value = currentHealth;

            return true;
        }


        public bool Damage(int value)
        {
            if (!_isCanDamage) return false;
            if (!Timer.SimpleTimer(_cooldownDamageDelay, cooldownDamage)) return false;
            _cooldownDamageDelay = Time.time;
            currentHealth -= value;
            _healthBar.value = currentHealth;
            currentHealth = Mathf.Clamp(currentHealth, 0, maximumHealth);
            OnDamageEvent?.Invoke();
            OnValueChange?.Invoke();
            if (currentHealth > 0) return true;
            OnDieEvent?.Invoke();
            _isCanDamage = false;

            if (_isPlayer)
            {
                if (currentHealth <= 0)
                {
                    gameObject.GetComponent<PlayerHealth>().PlayerDeth(_losePanle);
                }
            }

            return true;
        }

        public void SetMaximumHealth(int value)
        {
            maximumHealth = value;
            currentHealth = value;
        }
    }
}