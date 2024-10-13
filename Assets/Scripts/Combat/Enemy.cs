using UnityEngine;

namespace Main.Combar
{
    public class Enemy : MonoBehaviour
    {
        private Rigidbody2D _rb;

        [Header("Components")]
        public Transform body;
        public Transform[] legs;

        [Header("Values")]
        public float speed;

        [Header("Debug")]
        [SerializeField, ReadOnly] private Transform _target;
        [SerializeField, ReadOnly] private Vector2 _direction;
        [SerializeField, ReadOnly] private float _angle;
        [SerializeField, ReadOnly] private float _distance;
        [SerializeField, ReadOnly] private bool _isAttacking;


        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _target = GameManager.instance.GetPlayer();
        }

        private void Update()
        {
            _direction = (_target.position - transform.position).normalized;
            _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            _distance = (_target.position - transform.position).magnitude;

            body.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);

            _isAttacking = _distance <= 1;
            if (!_isAttacking) AnimateLegs();
        }

        private void FixedUpdate()
        {
            if (_isAttacking) return;

            _rb.MovePosition(_rb.position + speed * Time.fixedDeltaTime * _direction);
        }

        private void AnimateLegs()
        {
            float angle = 90f + Mathf.Sin(Time.time * 25f) * 20f;

            legs[0].localRotation = Quaternion.Euler(0f, 0f, angle);
            legs[1].localRotation = Quaternion.Euler(0f, 0f, -angle);
        }
    }
}
