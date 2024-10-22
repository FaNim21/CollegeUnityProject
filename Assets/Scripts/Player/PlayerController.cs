using Main.Misc;
using Main.UI;
using Main.UI.Equipment;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Main.Player
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

        [Header("Components")]
        public CanvasHandle canvasHandle;
        public Inventory inventory;

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
            HandleInputs();
        }

        private void HandleInputs()
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