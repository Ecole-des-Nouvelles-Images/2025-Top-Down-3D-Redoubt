using UnityEngine;

namespace Hugo.I.Scripts.Utils
{
    public class FixPositionRotation : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Vector3 _position;
        [SerializeField] private Vector3 _rotation;

        private void Update()
        {
            transform.localPosition = _position;
            transform.localRotation = Quaternion.Euler(_rotation);
        }
    }
}
