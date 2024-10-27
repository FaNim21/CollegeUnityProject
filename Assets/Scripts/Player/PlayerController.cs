using Main.Combat;
using Main.Misc;
using Main.UI;
using Main.UI.Equipment;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Main.Player
{
    public enum Mode
    {
        Normal,
        Building,
    }

    public class PlayerController : Entity
    {
        private Rigidbody2D _rb;
        private SpriteRenderer _spriteRenderer;

        [Header("Components")]
        public CanvasHandle canvasHandle;
        public Inventory inventory;
        public Transform shootingOffset;

        [Header("Values")]
        public float speed;
        public int layerMask;

        [Header("Debug")]
        [SerializeField, ReadOnly] private Mode _mode;
        [SerializeField, ReadOnly] private Vector2 _inputDirection;
        [SerializeField, ReadOnly] private float _currentSpeed;
        [SerializeField, ReadOnly] private float _aimAngle;
        [SerializeField, ReadOnly] private Vector2 _mousePosition;
        [SerializeField, ReadOnly] private Vector2 _aimDirection;


        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            _mousePosition = Utils.GetMouseWorldPosition();
            _aimDirection = (_mousePosition - (Vector2)shootingOffset.position).normalized;
            _aimAngle = Mathf.Atan2(_aimDirection.y, _aimDirection.x) * Mathf.Rad2Deg;

            if (Mouse.current.leftButton.wasPressedThisFrame && !canvasHandle.isCanvasEnabled) Shoot();
        }

        private void FixedUpdate()
        {
            _rb.MovePosition(_rb.position + speed * Time.fixedDeltaTime * _inputDirection);
        }

        private void Shoot()
        {
            var projectile = Instantiate(GameManager.instance.projectile, shootingOffset.position, Quaternion.Euler(0, 0, _aimAngle));
            projectile.Setup(layerMask, Quaternion.Euler(0, 0, _aimAngle) * Vector2.right, 10, 10);
        }


        public void OnMove(InputAction.CallbackContext context)
        {
            _inputDirection = context.ReadValue<Vector2>();
        }
    }
}