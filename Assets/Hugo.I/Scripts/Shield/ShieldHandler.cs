using UnityEngine;

namespace Hugo.I.Scripts.Shield
{
    public class ShieldHandler : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _offsetY;
        
        private bool _isCarried;
        private Transform _carrier;
        
        private Collider _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        private void Update()
        {
            if (!_isCarried) return;
            
            transform.position = _carrier.position;
        }

        public void Carrie(Transform carrier)
        {
            _collider.enabled = false;
            _isCarried = true;
            _carrier = carrier;
        }

        public void Drop()
        {
            _collider.enabled = true;
            _isCarried = false;
            _carrier = null;
            
            Ray ray = new Ray(transform.position, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                Vector3 pos = transform.position;
                pos.y = hit.point.y + _offsetY;
                transform.position = pos;
            }
        }
        
        public void Disappears()
        {
            Destroy(gameObject);
        }
    }
}
