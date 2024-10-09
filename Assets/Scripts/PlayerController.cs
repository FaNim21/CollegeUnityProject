using UnityEngine;
using UnityEngine.InputSystem;

namespace Main
{
    public enum Mode
    {
        Normal,
        Building,
    }

    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private SpriteRenderer _spriteRenderer;

        [Header("Values")]
        public float speed;

        [Header("Debug")]
        [SerializeField, ReadOnly] private Mode _mode;
        [SerializeField, ReadOnly] private Vector2 _movement;


        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {

        }

        private void FixedUpdate()
        {
            _rb.MovePosition(_rb.position + speed * Time.fixedDeltaTime * _movement);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _movement = context.ReadValue<Vector2>();
        }
    }
}
