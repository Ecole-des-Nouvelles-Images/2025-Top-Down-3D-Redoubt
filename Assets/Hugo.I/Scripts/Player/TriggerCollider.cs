using System.Collections.Generic;
using UnityEngine;

namespace Hugo.I.Scripts.Player
{
    public class TriggerCollider : MonoBehaviour
    {
        [SerializeField] private List<Collider> _colliders = new List<Collider>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Resource") || other.CompareTag("Tower") || other.CompareTag("Reload") || other.CompareTag("Heal")
                    || other.CompareTag("PowerPlant") || other.CompareTag("Shield") || other.CompareTag("Lobby"))
            {
                _colliders.Add(other);
            }
            _colliders.RemoveAll(collider => collider == null);
            _colliders.RemoveAll(collider => !collider.gameObject.activeSelf);
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
    }
}
