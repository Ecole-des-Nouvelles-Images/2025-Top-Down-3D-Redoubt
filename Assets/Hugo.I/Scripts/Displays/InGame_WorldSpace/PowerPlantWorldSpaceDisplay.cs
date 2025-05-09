using TMPro;
using UnityEngine;

namespace Hugo.I.Scripts.Displays.InGame_WorldSpace
{
    public class PowerPlantWorldSpaceDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currentAdvancement;
        [SerializeField] private TextMeshProUGUI _maxAdvancement;
        
        private Transform _mainCameraTransform;
        
        private void Start()
        {
            if (UnityEngine.Camera.main != null) _mainCameraTransform = UnityEngine.Camera.main.transform;
            
            float xAngle = Quaternion.LookRotation(transform.position - _mainCameraTransform.position).eulerAngles.x;
            float yAngle = _mainCameraTransform.eulerAngles.y;

            transform.rotation = Quaternion.Euler(xAngle, yAngle, 0f);
        }
        
        public void UpdateDisplay(int currentAdvancement, int maxAdvancement)
        {
            _currentAdvancement.text = currentAdvancement.ToString();
            _maxAdvancement.text = "/ " + maxAdvancement;
        }
    }
}
