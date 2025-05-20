using TMPro;
using UnityEngine;

namespace Hugo.I.Scripts.Displays.InGame_WorldSpace
{
    public class ResourceWorldSpaceDisplay : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private TextMeshProUGUI _currentResourceText;
        
        // private Transform _mainCameraTransform;

        // private void Start()
        // {
        //     if (UnityEngine.Camera.main != null) _mainCameraTransform = UnityEngine.Camera.main.transform;
        //     
        //     float xAngle = Quaternion.LookRotation(transform.position - _mainCameraTransform.position).eulerAngles.x;
        //     float yAngle = _mainCameraTransform.eulerAngles.y;
        //
        //     transform.rotation = Quaternion.Euler(xAngle, yAngle, 0f);
        // }

        public void UpdateDisplay(int currentResource)
        {
            _currentResourceText.text = currentResource.ToString();
        }
    }
}
