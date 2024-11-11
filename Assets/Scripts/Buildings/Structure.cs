using Main.Combat;
using Main.Datas;
using Main.Misc;
using Main.UI.Equipment;
using System;
using UnityEngine;

namespace Main.Buildings
{
    public abstract class Structure : MonoBehaviour, IDamageable
    {
        public Guid guid;
        public Vector2 size;
        public Vector2 trueSize;
        public Vector2 halfSize;
        public Vector2 placementPosition;

        [SerializeField] protected StructureData data;
        [SerializeField] protected ItemData itemData;

        [Header("Structure Components")]
        [SerializeField] private Transform _healthBar;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [Header("Structure Debug")]
        [SerializeField, ReadOnly] private float _health;
        [SerializeField, ReadOnly] private bool _died;
        [SerializeField, ReadOnly] protected bool _inPlacementMode;

        private Color _previousColor;

        public bool Died { get => _died; set => _died = value; }

        private int _maxHealth;


        protected virtual void Awake()
        {
            guid = Guid.NewGuid();

            halfSize = size / 2;

            _maxHealth = data.maxHealth;
            _health = _maxHealth;
        }

        public void OnDrawGizmos()
        {
            if (_inPlacementMode) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, size);
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0 || _died) return;

            _health -= damage;
            Utils.UpdateBar(_healthBar, Mathf.Max(0f, _health / _maxHealth));

            if (_health < 0)
            {
                _died = true;
                OnDeath();
            }
        }

        protected virtual void OnDeath() { }

        public virtual void EnterPlacementMode()
        {
            _previousColor = _spriteRenderer.color;
            _spriteRenderer.color = new Color(0, 0, 1, 0.5f);
            _collider.isTrigger = true;
            _inPlacementMode = true;
        }
        public virtual void ExitPlacementMode()
        {
            _spriteRenderer.color = _previousColor;
            _collider.isTrigger = false;
            _inPlacementMode = false;
        }

        public abstract void OpenGUI();
        public virtual void OnCollect(Inventory inventory)
        {
            inventory.AddToInventory(itemData, 1);
        }

        public bool Equals(Structure structure)
        {
            return guid.Equals(structure.guid);
        }
    }
}
