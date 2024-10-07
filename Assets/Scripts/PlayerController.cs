using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace Main
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private SpriteRenderer _spriteRenderer;

        [Header("Values")]
        public float speed;

        [Header("Values")]
        public Vector2 movement;


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
            _rb.MovePosition(_rb.position + movement * speed * Time.fixedDeltaTime);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            movement = context.ReadValue<Vector2>();
        }
    }
}
