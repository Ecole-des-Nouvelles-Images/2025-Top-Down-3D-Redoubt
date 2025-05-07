using System.Collections.Generic;
using UnityEngine;

namespace Hugo.I.Scripts.Player
{
    public class TriggerCollider : MonoBehaviour
    {
        [SerializeField] private List<Collider> _colliders = new List<Collider>();

        private void OnTriggerEnter(Collider other)
        {
            _colliders.RemoveAll(collider => collider == null);
            _colliders.RemoveAll(collider => !collider.gameObject.activeSelf);
            
            if (other.CompareTag("Untagged") || other.CompareTag("Player")) return;
            _colliders.Add(other);
        }

        private void OnTriggerExit(Collider other)
        {
            _colliders.Remove(other);
            _colliders.RemoveAll(collider => collider == null);
            _colliders.RemoveAll(collider => !collider.gameObject.activeSelf);
        }

        public GameObject GetNearestObject()
        {
            _colliders.RemoveAll(collider => collider == null);
            _colliders.RemoveAll(collider => !collider.gameObject.activeSelf);

            GameObject nearestObject = null;
            float nearestDistance = Mathf.Infinity;

            foreach (Collider col in _colliders)
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestObject = col.gameObject;
                }
            }

            return nearestObject;
        }
        
        public List<GameObject> GetGameObjectsWithTag(string targetTag)
        {
            _colliders.RemoveAll(collider => collider == null);
            _colliders.RemoveAll(collider => !collider.gameObject.activeSelf);
            
            List<GameObject> gameObjects = new List<GameObject>();
            
            foreach (Collider col in _colliders)
            {
                if (col.CompareTag(targetTag))
                {
                    gameObjects.Add(col.gameObject);
                }
            }
            
            return gameObjects;
        }
    }
}
