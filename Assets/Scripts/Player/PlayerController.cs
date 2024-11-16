using Main.Combat;
using Main.Misc;
using Main.UI;
using Main.UI.Equipment;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Main.Player
{
    public class PlayerController : Entity
    {
        private Rigidbody2D _rb;
        private SpriteRenderer _spriteRenderer;

        [Header("Components")]
        public GameObject body;
        public CameraController cameraController;
        public MapManager mapManager;
        public CanvasHandle canvasHandle;
        public Inventory inventory;
        public Transform shootingOffset;

        [Header("Values")]
        public int layerMask;
        public int projectileSpeed;
        public float projectileLifetime;
        public int damage;

        [Header("Debug")]
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
            if (Died) return;

            _mousePosition = Utils.GetMouseWorldPosition();
            _aimDirection = (_mousePosition - (Vector2)shootingOffset.position).normalized;
            _aimAngle = Mathf.Atan2(_aimDirection.y, _aimDirection.x) * Mathf.Rad2Deg;

            if (Keyboard.current.spaceKey.wasPressedThisFrame) Shoot();
        }
        private void FixedUpdate()
        {
            if (Died) return;

            Vector2 newPosition = _rb.position + data.speed * Time.fixedDeltaTime * _inputDirection;
            Vector2 adjustedDirection = _inputDirection;

            if (!mapManager.IsInBounds(new Vector2(newPosition.x, _rb.position.y), cameraController.boundsOffset))
            {
                adjustedDirection.x = 0;
            }
            if (!mapManager.IsInBounds(new Vector2(_rb.position.x, newPosition.y), cameraController.boundsOffset))
            {
                adjustedDirection.y = 0;
            }

            _rb.MovePosition(_rb.position + data.speed * Time.fixedDeltaTime * adjustedDirection);
        }

        private void Shoot()
        {
            if (canvasHandle.isCanvasEnabled || canvasHandle.isPointerOverGameObject || Died) return;

            var projectile = Instantiate(GameManager.instance.projectile, shootingOffset.position, Quaternion.Euler(0, 0, _aimAngle));
            projectile.Setup(layerMask, Quaternion.Euler(0, 0, _aimAngle) * Vector2.right, projectileSpeed, damage, projectileLifetime);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _inputDirection = context.ReadValue<Vector2>();
        }

        public override IEnumerator OnDeath()
        {
            body.SetActive(false);
            yield return new WaitForSeconds(3f);
            Restart();
            _isInvulnerable = true;
            body.SetActive(true);
            yield return new WaitForSeconds(1f);
            _isInvulnerable = false;
        }
    }
}