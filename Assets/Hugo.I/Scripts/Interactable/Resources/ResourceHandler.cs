using Hugo.I.Scripts.Utils;
using UnityEngine;

namespace Hugo.I.Scripts.Interactable.Resources
{
    public class ResourceHandler : MonoBehaviour, IInteractable
    {
        [Header("Settings")]
        [SerializeField] private ResourceData _resourceData;
        
        public Utils.ResourcesEnum GetResourceType()
        {
            Debug.Log("Interact with " + _resourceData._resourceEnumType);
            
            return _resourceData._resourceEnumType;
        }

        public int GetResourceMaxCollectable()
        {
            return _resourceData.MaxCollectable;
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
