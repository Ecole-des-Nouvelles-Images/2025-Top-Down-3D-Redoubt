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
            Debug.Log("Interact with " + _resourceData._resourceEnumType);
            
            int canCollect = Mathf.Min(CurrentCapacity, value);

            Debug.Log("There are : " + CurrentCapacity + " and I collect : " + canCollect);
            CurrentCapacity -= value;
            Debug.Log("Left : " + CurrentCapacity);
            
            return (_resourceData._resourceEnumType, canCollect);
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
