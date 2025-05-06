using System.Collections.Generic;
using DG.Tweening;
using Hugo.I.Scripts.Game;
using Unity.Cinemachine;
using UnityEngine;

namespace Hugo.I.Scripts.Interactable.Tower
{
    public class TowerManager : MonoBehaviour
    {
        public GameObject ActiveTower;
        
        [Header("Settings Tower")]
        [SerializeField] private List<GameObject> _towers;
        [SerializeField] private GameObject _reloadZone;
        [SerializeField] private GameObject _healingZone;
        
        [Header("Settings Camera")]
        [SerializeField] private CinemachineCamera _cinemachineCamera;
        [SerializeField] private List<int> _cameraLens;
        [SerializeField] private float _duration;
        [SerializeField] private AnimationCurve _curve;

        private void Awake()
        {
            GameManager.IsPowerPlantRepairs = false;
        }

        private void OnEnable()
        {
            GameManager.OnTriggerActive += ActiveReloadHealingZones;
        }
        
        private void OnDisable()
        {
            GameManager.OnTriggerActive -= ActiveReloadHealingZones;
        }

        public void UpgradeTower(GameObject currentTower)
        {
            if (_towers[1].activeSelf && !GameManager.IsPowerPlantRepairs) return;
            
            // Tower
            int index = _towers.IndexOf(currentTower);
            Debug.Log(index);
            _towers[index].gameObject.SetActive(false);
            ActiveTower = _towers[index + 1].gameObject;
            _towers[index + 1].gameObject.SetActive(true);

            ActiveTower.GetComponent<TowerHandler>().CurrentCapacity = currentTower.GetComponent<TowerHandler>().CurrentCapacity;
            
            // Camera dezoom
            DOTween.To(
                () => _cinemachineCamera.Lens.OrthographicSize,
                x => _cinemachineCamera.Lens.OrthographicSize = x,
                _cameraLens[index + 1],
                _duration
            ).SetEase(_curve);
        }

        private void ActiveReloadHealingZones()
        {
            _reloadZone.SetActive(true);
            _healingZone.SetActive(true);
        }
    }
}