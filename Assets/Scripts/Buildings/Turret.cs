using Main.UI.Equipment;
using Main.Visual;
using UnityEngine;

namespace Main.Buildings
{
    public class Turret : Structure
    {
        [Header("Components")]
        public SpriteRenderer rangeFieldRenderer;
        public Transform target;
        public Transform barrel;

        [Header("Values")]
        public float range;

        [Header("Debug")]
        [SerializeField, ReadOnly] private Vector2 _aimDirection;
        [SerializeField, ReadOnly] private float _aimAngle;


        protected override void Awake()
        {
            base.Awake();

            rangeFieldRenderer.gameObject.SetActive(true);
            rangeFieldRenderer.size = new Vector2(range * 2, range * 2);
        }
        private void Update()
        {
            _aimDirection = (target.position - transform.position).normalized;
            _aimAngle = Mathf.Atan2(_aimDirection.y, _aimDirection.x) * Mathf.Rad2Deg;

            if (IsTargetInDistance())
            {
                barrel.transform.rotation = Quaternion.Euler(0, 0, _aimAngle);
                Shoot();
            }
        }

        private void Shoot()
        {

        }

        private bool IsTargetInDistance()
        {
            float sqrDistance = (target.position - transform.position).sqrMagnitude;
            return sqrDistance < range * range;
        }

        public override void OnCollect(Inventory inventory)
        {
            Popup.Create(transform.position, "Collected Turret", Color.black);
            base.OnCollect(inventory);
        }

        public override void OpenGUI()
        {

        }


    }
}
