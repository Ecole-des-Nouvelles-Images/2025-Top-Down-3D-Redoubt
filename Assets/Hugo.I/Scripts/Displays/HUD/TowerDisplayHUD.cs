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
        [SerializeField] private Image _towerHealthImage;
        [SerializeField] private Image _towerEnergyImage;
        [SerializeField] private TextMeshProUGUI _towerCurrentStoneText;
        [SerializeField] private TextMeshProUGUI _towerNeededStoneText;
        [SerializeField] private TextMeshProUGUI _towerCurrentMetalText;
        [SerializeField] private TextMeshProUGUI _towerNeededMetalText;
        [SerializeField] private TextMeshProUGUI _towerCurrentCircuitText;
        [SerializeField] private TextMeshProUGUI _towerNeededCircuitText;
        private TowerHandler _towerHandler;

        private void Start()
        {
            _towerHandler = GameManager.ActualTowerGameObject.GetComponent<TowerHandler>();
        }

        private void Update()
        {
            if (GameManager.ActualTowerGameObject)
            {
                (TowerLevelData towerLevelData, int currentStone, int currentmetal, int currentCircuit) towerData = _towerHandler.GetTowerData();
                
                // Health
                float towerHealthNormalized =
                    Mathf.Clamp01(_towerHandler.CurrentHealth / towerData.towerLevelData.MaxHealth);
                _towerHealthImage.fillAmount = towerHealthNormalized;
                
                // Energy
                float towerEnergyNormalized;
                if (towerData.towerLevelData.MaxEnergy == 0)
                {
                    towerEnergyNormalized = 0;
                }
                else
                {
                    towerEnergyNormalized =
                        Mathf.Clamp01(_towerHandler.CurrentEnergy / towerData.towerLevelData.MaxEnergy);
                }
                _towerEnergyImage.fillAmount = towerEnergyNormalized;
                
                // Stone
                _towerCurrentStoneText.text = towerData.currentStone.ToString();
                _towerNeededStoneText.text = towerData.towerLevelData.StoneToLevelUp.ToString();
                
                // Metal
                _towerCurrentMetalText.text = towerData.currentmetal.ToString();
                _towerNeededMetalText.text = towerData.towerLevelData.MetalToLevelUp.ToString();
                
                // Electrical circuit
                _towerCurrentCircuitText.text = towerData.currentCircuit.ToString();
                _towerNeededCircuitText.text = towerData.towerLevelData.ElectricalCircuitToLevelUp.ToString();
            }
        }
    }
}
