using UnityEngine;

namespace Hugo.I.Scripts.GenerationTerrain
{
    public class DecalHandler : MonoBehaviour
    {
        private void Awake()
        {
            var rotation = transform.rotation;
            rotation.y = Random.Range(0, 360);
            transform.rotation = rotation;
        }
    }
}