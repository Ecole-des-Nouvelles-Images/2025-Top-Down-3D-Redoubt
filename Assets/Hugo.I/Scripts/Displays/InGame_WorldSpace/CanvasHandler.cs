using UnityEngine;

namespace Hugo.I.Scripts.Displays.InGame_WorldSpace
{
    public class CanvasHandler : MonoBehaviour
    {
        [Header("Settings Size")]
        [SerializeField] private bool _sizeAccordingToDistance;
        [SerializeField] private float _factorSize;
        [SerializeField] private Vector3 _minCanvasSize;
        [SerializeField] private Vector3 _maxCanvasSize;
        
        [Header("Settings Y Offset")]
        [SerializeField] private bool _yOffsetAccordingToSize;
        [SerializeField] private float _yOffsetFactor;
        [SerializeField] private Vector3 _minCanvasPos;
        [SerializeField] private Vector3 _maxCanvasPos;
        
        private Transform _mainCameraTransform;

        private void Start()
        {
            if (UnityEngine.Camera.main != null) _mainCameraTransform = UnityEngine.Camera.main.transform;
        }

        private void Update()
        {
            // Look
            float xAngle = Quaternion.LookRotation(transform.position - _mainCameraTransform.position).eulerAngles.x;
            float yAngle = _mainCameraTransform.eulerAngles.y;

            transform.rotation = Quaternion.Euler(xAngle, yAngle, 0f);
            
            // Size and yOffset
            if (_sizeAccordingToDistance)
            {
                // Size
                float cameraSize = _mainCameraTransform.GetComponent<UnityEngine.Camera>().orthographicSize;
                Vector3 canvasSize = Vector3.one * (cameraSize * _factorSize);
                transform.localScale = Vector3.Lerp(_minCanvasSize, _maxCanvasSize, canvasSize.x / _maxCanvasSize.x);
                
                // Offset
                if (_yOffsetAccordingToSize)
                {
                    float yOffset = canvasSize.y * _yOffsetFactor;
                    transform.localPosition = Vector3.Lerp(_minCanvasPos, _maxCanvasPos, yOffset / _maxCanvasPos.y);
                }
            }
        }

        public void OnSceneLoaded()
        {
            if (UnityEngine.Camera.main != null) _mainCameraTransform = UnityEngine.Camera.main.transform;
        }
    }
}
