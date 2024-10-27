using UnityEngine;

namespace Main.Combat
{
    public class Turret : Structure
    {
        public Transform target;
        public Transform barrel;

        [Header("Debug")]
        [SerializeField, ReadOnly] private Vector2 _aimDirection;
        [SerializeField, ReadOnly] private float _aimAngle;


        private void Awake()
        {
            
        }
        private void Update()
        {
            _aimDirection = (target.position - transform.position).normalized;
            _aimAngle = Mathf.Atan2(_aimDirection.y, _aimDirection.x) * Mathf.Rad2Deg;

            barrel.transform.rotation = Quaternion.Euler(0, 0, _aimAngle);
        }

        public override void OnCollect()
        {

        }

        public override void OpenGUI()
        {

        }


    }
}
