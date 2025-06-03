using UnityEngine;

namespace Hugo.I.Scripts.GenerationTerrain
{
    public class RandomRotationY : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int _minRotation;
        [SerializeField] private int _maxRotation;
        
        private void Awake()
        {
            var rotation = transform.rotation;
            rotation.y = Random.Range(_minRotation, _maxRotation);
            transform.rotation = rotation;
        }
    }
}