using Main.Combat;
using Main.Datas;
using System;
using UnityEngine;

namespace Main.Buildings
{
    public abstract class Structure : MonoBehaviour, IDamageable
    {
        public Guid guid;
        public StructureData data;

        [Header("Structure Components")]
        [SerializeField] private Transform _healthBar;

        [Header("Structure Debug")]
        [SerializeField, ReadOnly] private float _health;
        [SerializeField, ReadOnly] private bool _died;

        private int _maxHealth;


        protected virtual void Awake()
        {
            guid = Guid.NewGuid();

            _maxHealth = data.maxHealth;
            _health = _maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0 || _died) return;

            _health -= damage;
            UpdateBar(_healthBar, _health / _maxHealth);

            if (_health < 0)
            {
                _died = true;
                OnDeath();
            }
        }

        protected void UpdateBar(Transform bar, float proportion)
        {
            Vector3 barScale = bar.localScale;
            barScale.x = proportion;
            bar.localScale = barScale;
        }

        protected virtual void OnDeath() { }

        public abstract void OpenGUI();
        public abstract void OnCollect();
    }
}
