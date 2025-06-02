using System.Collections.Generic;
using Hugo.I.Scripts.Displays.InGame_WorldSpace;
using Hugo.I.Scripts.Game;
using Hugo.I.Scripts.Utils;
using UnityEngine;

namespace Hugo.I.Scripts.Interactable.PowerPlant
{
    public class PowerPlantHandler : MonoBehaviour, IInteractable
    {
        [Header("Settings")]
        public bool IsRepaired;
        public int QteSize;
        [SerializeField] private int _repairedAdvancement;
        [SerializeField] private int _numberOfRepairs;
        
        [Header("References")]
        [SerializeField] private PowerPlantWorldSpaceDisplay _powerPlantWorldSpaceDisplay;
        [SerializeField] private List<GameObject> _visuals;
        [SerializeField] private UnityEngine.Terrain _terrain;
        
        [Header("Vfx Effects")]
        [SerializeField] private ParticleSystem _vfxUpgrade;
        
        private GameObject _currentVisual;

        private void Awake()
        {
            _powerPlantWorldSpaceDisplay.UpdateDisplay(_repairedAdvancement, _numberOfRepairs);
            
            _currentVisual = _visuals[0];
        }

        private void Start()
        {
            Invoke(nameof(SetPosition), 0.1f);
        }

        public void Repair()
        {
            _repairedAdvancement++;
            _powerPlantWorldSpaceDisplay.UpdateDisplay(_repairedAdvancement, _numberOfRepairs);

            if (_repairedAdvancement >= _numberOfRepairs)
            {
                IsRepaired = true;
                GameManager.Instance.IsPowerPlantRepairs = true;
                GameManager.Instance.PowerPlantRestore();
            }
            
            // Visual
            _currentVisual.SetActive(false);
            _currentVisual = _visuals[_repairedAdvancement];
            _currentVisual.SetActive(true);
            
            // Vfx
            _vfxUpgrade.Play();
            
            Debug.Log("Repair advancement : " + _repairedAdvancement + ". Is repaired : " + IsRepaired);
        }

        public void OnEnterZone()
        {
            _powerPlantWorldSpaceDisplay.gameObject.SetActive(true);
        }

        public void OnExitZone()
        {
            _powerPlantWorldSpaceDisplay.gameObject.SetActive(false);
        }
        
        private void SetPosition()
        {
            float posY = _terrain.SampleHeight(transform.position) + _terrain.GetPosition().y;
            transform.position = new Vector3(transform.position.x, posY, transform.position.z);
        }
    }
}
