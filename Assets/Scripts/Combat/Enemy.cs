using System.Collections;
using UnityEngine;

namespace Main.Combat
{
    public class Enemy : Entity
    {
        private WavesController _wavesController;
        private MapManager _mapManager;
        private Rigidbody2D _rb;

        [Header("Components")]
        public Transform body;
        public Transform[] legs;
        public Transform[] bites;

        [Header("Values")]
        public float chaseMaxTime = 10f;

        [Header("Debug")]
        [SerializeField, ReadOnly] private Vector2 _direction;
        [SerializeField, ReadOnly] private float _angle;
        [SerializeField, ReadOnly] private float _distance;
        [SerializeField, ReadOnly] private bool _isAttacking;
        [SerializeField, ReadOnly] private bool _focusOnMainBase;
        [SerializeField, ReadOnly] private float _attackingTimer;
        [SerializeField, ReadOnly] private float _chaseTimer;
        private float _bitesTimer;

        private IDamageable _target;


        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _mapManager = GameManager.instance.mapManager;
        }
        public void Setup(WavesController wavesController, bool focusOnMainBase)
        {
            _wavesController = wavesController;
            _focusOnMainBase = focusOnMainBase;
        }

        private void Update()
        {
            if (_target != null && _target.Died)
            {
                _target = null;
            }

            if (_target == null) 
            {
                FindTarget();
            }

            if (_chaseTimer >= chaseMaxTime)
            {
                _chaseTimer = 0f;
                FindTarget(_target);
            }

            if (_target != null && _target.Died)
            {
                _target = null;
                return;
            }
            if (_target == null)
            {
                _direction = Vector2.zero;
                return;
            }

            _direction = (_target.Position - transform.position).normalized;
            _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            _distance = (_target.Position - transform.position).magnitude;

            body.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);

            _isAttacking = _distance <= _target.Size;

            if (_isAttacking)
            {
                _chaseTimer = 0f;
                Attack();
            }
            else
            {
                _chaseTimer += Time.deltaTime;
                AnimateLegs();
            }
        }

        private void FixedUpdate()
        {
            if (_isAttacking) return;

            _rb.MovePosition(_rb.position + data.speed * Time.fixedDeltaTime * _direction);
        }

        private void FindTarget(IDamageable exclude = null)
        {
            _target = _mapManager.GetClosestStructure(transform.position, exclude, _focusOnMainBase);
        }

        private void Attack()
        {
            _attackingTimer += Time.deltaTime;
            _bitesTimer += Time.deltaTime / data.attackTime;

            float angle = Mathf.Lerp(-30f, 30f, _bitesTimer);
            float angle2 = Mathf.Lerp(30f, -30f, _bitesTimer);
            bites[0].localRotation = Quaternion.Euler(0, 0, angle);
            bites[1].localRotation = Quaternion.Euler(0, 0, angle2);

            if (_attackingTimer >= data.attackTime)
            {
                _bitesTimer = 0f;
                bites[0].localRotation = Quaternion.Euler(0f, 0f, -30f);
                bites[1].localRotation = Quaternion.Euler(0f, 0f, 30f);
                _target.TakeDamage(data.damage);
                _attackingTimer = 0f;
            }
        }

        private void AnimateLegs()
        {
            float angle = 90f + Mathf.Sin(Time.time * 25f) * 20f;

            legs[0].localRotation = Quaternion.Euler(0f, 0f, angle);
            legs[1].localRotation = Quaternion.Euler(0f, 0f, -angle);
        }

        public override IEnumerator OnDeath()
        {
            _wavesController.RemoveEnemy(this);
            return base.OnDeath();
        }
    }
}
