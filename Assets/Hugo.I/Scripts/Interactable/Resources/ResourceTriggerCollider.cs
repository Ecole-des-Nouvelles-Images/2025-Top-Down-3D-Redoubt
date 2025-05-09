using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hugo.I.Scripts.Interactable.Resources
{
    public class ResourceTriggerCollider : MonoBehaviour
    {
        [SerializeField] private ResourceHandler _resourceHandler;

        private List<GameObject> _players = new List<GameObject>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _resourceHandler.OnEnterZone();
                _players.Add(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _players.Remove(other.gameObject);

                if (_players.Count == 0)
                {
                    _resourceHandler.OnExitZone();
                }
            }
        }
    }
}
