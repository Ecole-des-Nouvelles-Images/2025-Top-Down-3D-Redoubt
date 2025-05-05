using System.Collections.Generic;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

namespace Hugo.I.Scripts.Interactable.Tower
{
    public class TowerLevelsManager : MonoBehaviour
    {
        [Header("Settings Tower")]
        [SerializeField] private List<GameObject> _towers;
        
        [Header("Settings Camera")]
        [SerializeField] private CinemachineCamera _cinemachineCamera;
        [SerializeField] private List<int> _cameraLens;
        [SerializeField] private float _duration;
        [SerializeField] private AnimationCurve _curve;

        public void UpgradeTower(GameObject currentTower)
        {
            // Tower
            int index = _towers.IndexOf(currentTower);
            Debug.Log(index);
            _towers[index].gameObject.SetActive(false);
            _towers[index + 1].gameObject.SetActive(true);
            
            // Camera dezoom
            DOTween.To(
                () => _cinemachineCamera.Lens.OrthographicSize,
                x => _cinemachineCamera.Lens.OrthographicSize = x,
                _cameraLens[index + 1],
                _duration
            ).SetEase(_curve);
        }
    }
}
