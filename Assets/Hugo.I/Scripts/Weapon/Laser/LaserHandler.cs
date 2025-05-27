using UnityEngine;

namespace Hugo.I.Scripts.Weapon.Laser
{
    public class LaserHandler : MonoBehaviour
    {
        [Header("Line Settings")]
        [SerializeField] private Vector3 _axis = Vector3.right;
        [SerializeField] private float _length = 1f;
        
        private LineRenderer _lineRenderer;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = 2;
        }

        private void Update()
        {
            Vector3 start = transform.position;
            Vector3 end = start + transform.TransformDirection(_axis.normalized) * _length;

            _lineRenderer.SetPosition(0, start);
            _lineRenderer.SetPosition(1, end);
        }
    }
}
