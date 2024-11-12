using Main.Combat;
using Main.UI.Equipment;
using Main.Visual;
using UnityEngine;

namespace Main.Buildings
{
    public class Turret : Structure
    {
        private WavesController _wavesController;

        [Header("Components")]
        public SpriteRenderer rangeFieldRenderer;
        public Transform barrel;

        [Header("Values")]
        [SerializeField] private Projectile _bullet;
        public float range;
        public int layerMask;
        public int damage;
        public float projectileSpeed;
        public float shootingFrequency;

        [Header("Debug")]
        [SerializeField, ReadOnly] private IDamageable target;
        [SerializeField, ReadOnly] private Vector2 _aimDirection;
        [SerializeField, ReadOnly] private float _aimAngle;
        [SerializeField, ReadOnly] private float _shootingTimer;


        protected override void Awake()
        {
            base.Awake();

            rangeFieldRenderer.size = new Vector2(range * 2, range * 2);
        }
        protected override void Start()
        {
            base.Start();
            _wavesController = GameManager.instance.GetComponent<WavesController>();
        }

        private void Update()
        {
            if (_inPlacementMode) return;

            target ??= _wavesController.GetClosestEnemy(transform.position, range);

            if (target == null) return;
            if (target != null && target.Died)
            {
                target = null;
                return;
            }

            if (target != null && !IsTargetInDistance()) return;

            _aimDirection = (target.Position - transform.position).normalized;
            _aimAngle = Mathf.Atan2(_aimDirection.y, _aimDirection.x) * Mathf.Rad2Deg;

            barrel.transform.rotation = Quaternion.Euler(0, 0, _aimAngle);

            _shootingTimer += Time.deltaTime;
            if (_shootingTimer >= shootingFrequency)
            {
                _shootingTimer = 0;
                Shoot();
            }
        }

        private void Shoot()
        {
            if (!IsTargetInDistance()) return;

            var bullet = Instantiate(_bullet, transform.position, Quaternion.Euler(0f, 0f, _aimAngle));
            bullet.Setup(layerMask, Quaternion.Euler(0, 0, _aimAngle) * Vector2.right, projectileSpeed, damage);
        }

        private bool IsTargetInDistance()
        {
            float sqrDistance = (target.Position - transform.position).sqrMagnitude;
            return sqrDistance < range * range;
        }

        public override void OnCollect(Inventory inventory)
        {
            Destroy(gameObject);

            Popup.Create(transform.position, "Collected Turret", Color.black);
            base.OnCollect(inventory);
        }

        public override void OpenGUI() { }

        public override void EnterPlacementMode()
        {
            rangeFieldRenderer.gameObject.SetActive(true);
            base.EnterPlacementMode();
        }
        public override void ExitPlacementMode()
        {
            rangeFieldRenderer.gameObject.SetActive(false);
            base.ExitPlacementMode();
        }
    }
}
