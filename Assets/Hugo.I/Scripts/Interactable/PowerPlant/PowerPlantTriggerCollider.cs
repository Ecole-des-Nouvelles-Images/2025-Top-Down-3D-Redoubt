using System.Collections.Generic;
using UnityEngine;

namespace Hugo.I.Scripts.Interactable.PowerPlant
{
    public class PowerPlantTriggerCollider : MonoBehaviour
    {
        [SerializeField] private PowerPlantHandler _powerPlantHandler;

        private List<GameObject> _players = new List<GameObject>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _powerPlantHandler.OnEnterZone();
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
                    _powerPlantHandler.OnExitZone();
                }
            }
        }
    }
}
