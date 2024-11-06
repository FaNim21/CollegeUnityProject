using UnityEngine;

namespace Main.Player
{
    public class CameraController : MonoBehaviour
    {
        private Camera _camera;

        [Header("Values")]
        [SerializeField] private Vector2 zoomSize;
        [SerializeField] private float boundsLength;
        [SerializeField] private Vector2 boundsOffset;
        [SerializeField] private Vector2 _offset;

        [Header("Debug")]
        [SerializeField, ReadOnly] private bool isTargeting;
        [SerializeField, ReadOnly] private Vector2 bounds;
        [SerializeField, ReadOnly] private Transform target;
        [SerializeField, ReadOnly] private float _zoom;
        [SerializeField, Range(0f, 1f)] private float smoothSpeed = 0.125f;

        private float _halfBoundsLength;
        private Vector3 _velocity = Vector3.zero;


        private void Start()
        {
            _camera = GetComponent<Camera>();
            target = GameManager.instance.GetPlayer();

            _zoom = _camera.orthographicSize;
            transform.position = new Vector3(target.position.x + _offset.x, target.position.y + _offset.y, -10);
            _halfBoundsLength = boundsLength / 2;
        }

        private void Update()
        {
            bounds.x = boundsLength / 2f - _zoom * _camera.aspect;
            bounds.y = boundsLength / 2f - _zoom;
        }

        private void LateUpdate()
        {
            if (!isTargeting || target == null) return;

            Vector2 desiredPosition = new(target.position.x + _offset.x, target.position.y + _offset.y);
            float clampedX = Mathf.Clamp(desiredPosition.x, -bounds.x, bounds.x);
            float clampedY = Mathf.Clamp(desiredPosition.y, -bounds.y, bounds.y);
            Vector3 clampedPosition = new(clampedX, clampedY, -10);

            Vector3 positionSmoothed = Vector3.SmoothDamp(transform.position, clampedPosition, ref _velocity, smoothSpeed);
            transform.position = positionSmoothed;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Vector2.zero, new Vector2(boundsLength, boundsLength));
        }

        public bool IsInBounds(Vector2 position)
        {
            return position.x + boundsOffset.x < _halfBoundsLength && 
                   position.x - boundsOffset.x > -_halfBoundsLength &&
                   position.y + boundsOffset.y < _halfBoundsLength &&
                   position.y - boundsOffset.y > -_halfBoundsLength;
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
