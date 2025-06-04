using DG.Tweening;
using UnityEngine;

namespace Hugo.I.Scripts.Lights
{
    public class DirectionalLightHandler : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Vector3 _rotationSpeed;
        [SerializeField] private float _rotationDuration;

        private void Update()
        {
            transform.DORotate(_rotationSpeed, _rotationDuration);
        }
    }
}
