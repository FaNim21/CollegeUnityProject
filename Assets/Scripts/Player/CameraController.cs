using UnityEngine;

namespace Main.Player
{
    public class CameraController : MonoBehaviour
    {
        private Camera _camera;

        [Header("Values")]
        public Vector2 zoomSize;

        [Header("Debug")]
        public bool isTargeting;
        [SerializeField, ReadOnly] private Transform target;
        [SerializeField, ReadOnly] private float _zoom;
        [SerializeField, Range(0f, 1f)] private float smoothSpeed = 0.125f;
        [SerializeField] private Vector2 _offset;

        private Vector3 _velocity = Vector3.zero;


        private void Start()
        {
            _camera = GetComponent<Camera>();
            target = GameManager.instance.GetPlayer();

            _zoom = _camera.orthographicSize;
            transform.position = new Vector3(target.position.x + _offset.x, target.position.y + _offset.y, -10);
        }

        private void LateUpdate()
        {
            if (!isTargeting || target == null) return;

            Vector3 desiredPosition = new(target.position.x + _offset.x, target.position.y + _offset.y, -10);
            Vector3 positionSmoothed = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, smoothSpeed);

            transform.position = positionSmoothed;
        }

        public void ChangeTarget(Transform newTarget)
        {
            target = newTarget;
        }

        public void ChangeZoom(float value)
        {
            _zoom -= value / 2;
            _zoom = Mathf.Clamp(_zoom, zoomSize.x, zoomSize.y);

            _camera.orthographicSize = _zoom;
        }
    }
}
