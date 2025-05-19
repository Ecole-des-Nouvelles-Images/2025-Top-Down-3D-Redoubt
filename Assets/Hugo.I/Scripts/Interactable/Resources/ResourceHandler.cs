using Hugo.I.Scripts.Displays.InGame_WorldSpace;
using Hugo.I.Scripts.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hugo.I.Scripts.Interactable.Resources
{
    public class ResourceHandler : MonoBehaviour, IInteractable
    {
        [Header("Settings")]
        [SerializeField] private ResourceData _resourceData;
        [SerializeField] private ResourceWorldSpaceDisplay _resourceWorldSpaceDisplay;
        [FormerlySerializedAs("_capacity")] [SerializeField] private int _maxCapacity;
        
        [Header("Reference")]
        [SerializeField] private GameObject _resourceEmptyObject;

        private int _currentCapacity;

        public int CurrentCapacity
        {
            get => _currentCapacity;
            set
            {
                _currentCapacity = value;
                if (_currentCapacity <= 0)
                {
                    Invoke(nameof(ResourceIsEmpty), 0.1f);
                }
            }
        }

        private void Awake()
        {
            CurrentCapacity = _maxCapacity;
            _resourceWorldSpaceDisplay.UpdateDisplay(CurrentCapacity);
        }

        public (ResourcesEnum, int) GetResources(int value)
        {
            int canCollect = Mathf.Min(CurrentCapacity, value);

            CurrentCapacity -= canCollect;
            
            _resourceWorldSpaceDisplay.UpdateDisplay(CurrentCapacity);
            
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

        private void ResourceIsEmpty()
        {
            _resourceEmptyObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
