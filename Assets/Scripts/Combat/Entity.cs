using Main.Datas;
using Main.Misc;
using System.Collections;
using UnityEngine;

namespace Main.Combat
{
    public class Entity : MonoBehaviour, IDamageable
    {
        public EntityData data;
        public Transform healthBar;
        [SerializeField] private CircleCollider2D _collider;

        [SerializeField, ReadOnly] private float _health;
        [SerializeField, ReadOnly] private bool _died;
        [SerializeField, ReadOnly] protected bool _isInvulnerable;
        [SerializeField, ReadOnly] private Vector2 _spawnPosition;

        public Vector3 Position => transform.position;
        public float Size => _collider.radius;
        public bool Died { get => _died; set => _died = value; }



        protected virtual void Awake()
        {
            _spawnPosition = transform.position;
            Restart();
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0 || _died || _isInvulnerable) return;

            Utils.Log($"{gameObject.name} otrzymal damage {damage}");

            _health -= damage;
            Utils.UpdateBar(healthBar, Mathf.Max(0, _health / data.maxHealth));

            if (_health <= 0)
            {
                _died = true;
                StartCoroutine(OnDeath());
            }
        }

        public virtual IEnumerator OnDeath()
        {
            Destroy(gameObject);
            yield return null;
        }

        protected void Restart()
        {
            _health = data.maxHealth;
            Utils.UpdateBar(healthBar, Mathf.Max(0, _health / data.maxHealth));
            transform.position = _spawnPosition;
            _died = false;
        }
    }
}
