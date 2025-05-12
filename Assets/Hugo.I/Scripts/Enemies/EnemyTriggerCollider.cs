using System.Collections.Generic;
using Hugo.I.Scripts.Interactable.PowerPlant;
using Hugo.I.Scripts.Player;
using UnityEngine;

namespace Hugo.I.Scripts.Enemies
{
    public class EnemyTriggerCollider : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private EnemyData _enemyData;
        [Header("Settings")]
        [SerializeField] private List<Collider> _colliders = new List<Collider>();
        
        private void OnTriggerEnter(Collider other)
        {
            _colliders.RemoveAll(collider => collider == null);
            _colliders.RemoveAll(collider => !collider.gameObject.activeSelf);
            
            if (other.CompareTag("Player"))
            {
                _enemyData.PlayerController = other.GetComponent<PlayerController>();
                _colliders.Add(other);
            }
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
