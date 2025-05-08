using System.Collections.Generic;
using Hugo.I.Scripts.Game;
using Hugo.I.Scripts.Interactable.Tower;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hugo.I.Scripts.Displays.HUD
{
    public class TowerDisplayHUD : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private List<Sprite> _towerIcon;
        [SerializeField] private Image _towerHealthSlider;
        [SerializeField] private Image _towerEnergySlider;
        [SerializeField] private TextMeshProUGUI _towerCurrentStone;
        [SerializeField] private TextMeshProUGUI _towerNeededStone;
        [SerializeField] private TextMeshProUGUI _towerCurrentMetal;
        [SerializeField] private TextMeshProUGUI _towerNeededMetal;
        [SerializeField] private TextMeshProUGUI _towerCurrentCircuit;
        [SerializeField] private TextMeshProUGUI _towerNeededCircuit;

        private void Update()
        {
            if (GameManager.ActualTowerGameObject)
            {
                TowerHandler towerHandler = GameManager.ActualTowerGameObject.GetComponent<TowerHandler>();
                
                // FAIRE AVEC UNE IMAGE EN IMAGETYPE FILL
                
                float towerEnergyNormalized =
                    Mathf.Clamp01(towerHandler.CurrentEnergy / towerHandler.GetTowerLevelData().MaxEnergy);
                _towerEnergySlider.fillAmount = towerEnergyNormalized;
            }
        }
    }
}
