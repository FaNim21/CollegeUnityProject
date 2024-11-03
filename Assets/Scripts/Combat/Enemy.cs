using UnityEngine;

namespace Main.Combat
{
    public class Enemy : Entity
    {
        private Rigidbody2D _rb;

        [Header("Components")]
        public Transform body;
        public Transform[] legs;
        public Transform[] bites;

        [Header("Debug")]
        [SerializeField, ReadOnly] private Transform _target;
        [SerializeField, ReadOnly] private Vector2 _direction;
        [SerializeField, ReadOnly] private float _angle;
        [SerializeField, ReadOnly] private float _distance;
        [SerializeField, ReadOnly] private bool _isAttacking;
        [SerializeField, ReadOnly] private float _attackingTimer;
        private float _bitesTimer;

        public IDamageable damageableTarget;


        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _target = GameManager.instance.GetPlayer();
            damageableTarget = _target.GetComponent<IDamageable>();
        }

        private void Update()
        {
            _direction = (_target.position - transform.position).normalized;
            _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            _distance = (_target.position - transform.position).magnitude;

            body.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);

            _isAttacking = _distance <= 1;

            if (damageableTarget.Died)
            {
                FindOtherTarget();
                return;
            }

            if (_isAttacking)
            {
                Attack();
            }
            else
            {
                AnimateLegs();
            }
        }

        private void FixedUpdate()
        {
            if (_isAttacking) return;

            _rb.MovePosition(_rb.position + data.speed * Time.fixedDeltaTime * _direction);
        }

        private void FindOtherTarget()
        {

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
                damageableTarget.TakeDamage(data.damage);
                _attackingTimer = 0f;
            }
        }

        private void AnimateLegs()
        {
            float angle = 90f + Mathf.Sin(Time.time * 25f) * 20f;

            legs[0].localRotation = Quaternion.Euler(0f, 0f, angle);
            legs[1].localRotation = Quaternion.Euler(0f, 0f, -angle);
        }
    }
}
