using Hugo.I.Scripts.Utils;
using UnityEngine;

namespace Hugo.I.Scripts.Interactable.Resources
{
    public class ResourceHandler : MonoBehaviour, IInteractable
    {
        [Header("Settings")]
        [SerializeField] private ResourceData _resourceData;
        [SerializeField] private int _capacity;

        public int CurrentCapacity;

        private void Awake()
        {
            CurrentCapacity = _capacity;
        }

        public (ResourcesEnum, int) GetResources(int value)
        {
            int canCollect = Mathf.Min(CurrentCapacity, value);

            CurrentCapacity -= value;
            
            return (_resourceData.ResourceEnumType, canCollect);
        }

        public int ResourcesICanCollect()
        {
            int canCollect = Mathf.Min(CurrentCapacity, _resourceData.MaxCollectableAtOnce);
            
            return canCollect;
        }

        public void OnEnterZone()
        {
            // Afficher le type et le nombre de ressource dispo
        }

        public void OnExitZone()
        {
            // Cacher le type et le nombre de ressource dispo
        }
    }
}
