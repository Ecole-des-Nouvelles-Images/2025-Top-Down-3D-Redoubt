using System.Collections.Generic;
using DG.Tweening;
using Hugo.I.Scripts.Enemies;
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
        [SerializeField] private EnemySpawnerManager _enemySpawnPoints;
        [SerializeField] private GameObject _shieldZonePrefab;
        [SerializeField] private Transform _shieldZoneSpawnPoint;
        
        [Header("Settings Camera")]
        [SerializeField] private CinemachineCamera _cinemachineCamera;
        [SerializeField] private List<int> _cameraLens;
        [SerializeField] private float _duration;
        [SerializeField] private AnimationCurve _curve;
        
        [Header("Enemies")]
        [SerializeField] private EnemySpawnerManager _enemySpawnerManager;

        private void Awake()
        {
            GameManager.Instance.ActualTowerGameObject = _towers[0].GetComponent<TowerHandler>();
        }

        private void OnEnable()
        {
            GameManager.Instance.OnTriggerActive += ActiveReloadZone;
        }
        
        private void OnDisable()
        {
            GameManager.Instance.OnTriggerActive -= ActiveReloadZone;
        }

        public void UpgradeTower(GameObject currentTower)
        {
            if (_towers[1].activeSelf && !GameManager.Instance.IsPowerPlantRepairs) return;
            if (_towers[3].activeSelf) return;
            
            // Tower
            int index = _towers.IndexOf(currentTower);
            _towers[index].gameObject.SetActive(false);
            ActiveTower = _towers[index + 1].gameObject;
            _towers[index + 1].gameObject.SetActive(true);

            ActiveTower.GetComponent<TowerHandler>().CurrentEnergy = currentTower.GetComponent<TowerHandler>().CurrentEnergy;
            ActiveTower.GetComponent<TowerHandler>().CurrentHealth = currentTower.GetComponent<TowerHandler>().CurrentHealth;
            GameManager.Instance.ActualTowerGameObject = ActiveTower.GetComponent<TowerHandler>();
            
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
            
            // Change enemies spawn
            _enemySpawnerManager.ChangeSpawnPoints(index + 1);
            _enemySpawnerManager.UpdateTowerHandler();
        }

        public void TowerReceiveShield()
        {
            _towers[2].gameObject.SetActive(false);
            _towers[3].gameObject.SetActive(true);
            ActiveTower = _towers[3].gameObject;
            GameManager.Instance.ActualTowerGameObject = ActiveTower.GetComponent<TowerHandler>();
            
            // Lance la win
            Instantiate(_shieldZonePrefab, _shieldZoneSpawnPoint.position, Quaternion.identity);
            GameManager.Instance.WinGame();
        }

        private void ActiveReloadZone()
        {
            _reloadZone.SetActive(true);
        }
    }
}