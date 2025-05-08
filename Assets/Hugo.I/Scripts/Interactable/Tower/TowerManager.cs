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
            Debug.Log("Number player : " + GameManager.Players.Count);
            GameManager.IsPowerPlantRepairs = false;
            GameManager.ActualTowerGameObject = _towers[0].GetComponent<TowerHandler>();
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

            ActiveTower.GetComponent<TowerHandler>().CurrentEnergy = currentTower.GetComponent<TowerHandler>().CurrentEnergy;
            GameManager.ActualTowerGameObject = ActiveTower.GetComponent<TowerHandler>();
            
            // Camera dezoom
            DOTween.To(
                () => _cinemachineCamera.Lens.OrthographicSize,
                x => _cinemachineCamera.Lens.OrthographicSize = x,
                _cameraLens[index + 1],
                _duration
            ).SetEase(_curve);

            // Active le Heal si elle passe T2
            if (ActiveTower == _towers[2])
            {
                _healingZone.SetActive(true);
            }
        }

        private void ActiveReloadHealingZones()
        {
            _reloadZone.SetActive(true);
        }
    }
}