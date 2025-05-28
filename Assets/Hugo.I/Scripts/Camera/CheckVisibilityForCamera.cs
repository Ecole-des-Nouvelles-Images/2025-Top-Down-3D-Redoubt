using Hugo.I.Scripts.Player;
using UnityEngine;

namespace Hugo.I.Scripts.Camera
{
    public class CheckVisibilityForCamera : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _fogDamage;
        [SerializeField] private float _fogDamageInterval;
        
        private PlayerController _playerController;
        private UnityEngine.Camera _camera;

        private float _timer;

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
        }

        private void Update()
        {
            if (!_camera || _camera != UnityEngine.Camera.main)
            {
                _camera = UnityEngine.Camera.main;
            }
            
            _timer += Time.deltaTime;
            
            if (!IsVisible() && _timer >= _fogDamageInterval)
            {
                _timer = 0;
                _playerController.TakeDamage(_fogDamage);
            }
        }

        private bool IsVisible()
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_camera);

            foreach (var plane in planes)
            {
                if (plane.GetDistanceToPoint(transform.position) < 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
