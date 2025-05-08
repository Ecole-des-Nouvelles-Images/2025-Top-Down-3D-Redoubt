using System.Collections.Generic;
using Hugo.I.Scripts.Game;
using Hugo.I.Scripts.Player;
using Hugo.I.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hugo.I.Scripts.Displays.HUD
{
    public class PlayerDisplayHUD : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int _playerId;
        [SerializeField] private Sprite _playerIcon;
        [SerializeField] private Image _playerHealthImage;
        [SerializeField] private Image _playerEnergyImage;
        [SerializeField] private TextMeshProUGUI _playerCurrentStoneText;
        [SerializeField] private TextMeshProUGUI _playerMaxStoneText;
        [SerializeField] private TextMeshProUGUI _playerCurrentMetalText;
        [SerializeField] private TextMeshProUGUI _playerMaxMetalText;
        [SerializeField] private TextMeshProUGUI _playerCurrentCircuitText;
        [SerializeField] private TextMeshProUGUI _playerMaxCircuitText;
        private PlayerController _playerController;
        
        private void Start()
        {
            if (GameManager.Players.Count - 1 >= _playerId)
            {
                _playerController = GameManager.Players[_playerId].GetComponent<PlayerController>();
            }
        }

        private void Update()
        {
            if (!_playerController) return;
            
            (float maxHealth, float currentHealth, float capacity, float currentCapacity, 
                Dictionary<ResourcesEnum, int> inventory, int maxStone, int maxMetal, int maxCircuit) playerData
                    = _playerController.GetHudData();
            
            // Health
            float playerHealthNormalized =
                Mathf.Clamp01(playerData.currentHealth / playerData.maxHealth);
            _playerHealthImage.fillAmount = playerHealthNormalized;
            
            // Energy
            float playerEnergyNormalized =
                Mathf.Clamp01(playerData.currentCapacity / playerData.capacity);
            _playerEnergyImage.fillAmount = playerEnergyNormalized;
            
            // Stone
            _playerCurrentStoneText.text = playerData.inventory[ResourcesEnum.Stone].ToString();
            _playerMaxStoneText.text = "/ " + playerData.maxStone;
            
            // Metal
            _playerCurrentMetalText.text = playerData.inventory[ResourcesEnum.Metal].ToString();
            _playerMaxMetalText.text = "/ " + playerData.maxMetal;
            
            // Electrical circuit
            _playerCurrentCircuitText.text = playerData.inventory[ResourcesEnum.ElectricalCircuit].ToString();
            _playerMaxCircuitText.text = "/ " + playerData.maxCircuit;
        }
    }
}
