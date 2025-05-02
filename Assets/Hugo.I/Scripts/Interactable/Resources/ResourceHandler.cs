using Hugo.I.Scripts.Utils;
using UnityEngine;

namespace Hugo.I.Scripts.Interactable.Resources
{
    public class ResourceHandler : MonoBehaviour, IInteractable
    {
        [Header("Settings")]
        [SerializeField] private ResourceData _resourceData;
        
        
        public Utils.Resources GetResourceType()
        {
            Debug.Log("Interact with " + _resourceData.ResourceType);
            
            return _resourceData.ResourceType;
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
