using System;
using UnityEngine;

namespace Hugo.I.Scripts.Displays.InGame_WorldSpace
{
    public class CanvasLookCameraHandler : MonoBehaviour
    {
        private Transform _mainCameraTransform;

        private void Start()
        {
            if (UnityEngine.Camera.main != null) _mainCameraTransform = UnityEngine.Camera.main.transform;
        }

        private void Update()
        {
            float xAngle = Quaternion.LookRotation(transform.position - _mainCameraTransform.position).eulerAngles.x;
            float yAngle = _mainCameraTransform.eulerAngles.y;

            transform.rotation = Quaternion.Euler(xAngle, yAngle, 0f);
        }

        public void OnSceneLoaded()
        {
            if (UnityEngine.Camera.main != null) _mainCameraTransform = UnityEngine.Camera.main.transform;
        }
    }
}
