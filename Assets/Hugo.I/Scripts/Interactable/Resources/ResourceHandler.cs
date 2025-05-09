using Hugo.I.Scripts.Displays.InGame_WorldSpace;
using Hugo.I.Scripts.Utils;
using UnityEngine;

namespace Hugo.I.Scripts.Interactable.Resources
{
    public class ResourceHandler : MonoBehaviour, IInteractable
    {
        [Header("Settings")]
        [SerializeField] private ResourceData _resourceData;
        [SerializeField] private ResourceWorldSpaceDisplay _resourceWorldSpaceDisplay;
        [SerializeField] private int _capacity;

        public int CurrentCapacity;

        private void Awake()
        {
            CurrentCapacity = _capacity;
            _resourceWorldSpaceDisplay.UpdateDisplay(CurrentCapacity);
        }

        public (ResourcesEnum, int) GetResources(int value)
        {
            int canCollect = Mathf.Min(CurrentCapacity, value);

            CurrentCapacity -= value;
            
            _resourceWorldSpaceDisplay.UpdateDisplay(canCollect);
            
            return (_resourceData.ResourceEnumType, canCollect);
        }

        public (ResourcesEnum, int) ResourcesICanCollect()
        {
            int canCollect = Mathf.Min(CurrentCapacity, _resourceData.MaxCollectableAtOnce);
            
            return (_resourceData.ResourceEnumType, canCollect);
        }

        public void OnEnterZone()
        {
            _resourceWorldSpaceDisplay.gameObject.SetActive(true);
        }

        public void OnExitZone()
        {
            _resourceWorldSpaceDisplay.gameObject.SetActive(false);
        }
    }
}
