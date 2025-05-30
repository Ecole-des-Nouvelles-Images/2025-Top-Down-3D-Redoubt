using System.Collections.Generic;
using Hugo.I.Scripts.Player;
using Hugo.I.Scripts.Utils;
using Hugo.I.Scripts.Weapon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hugo.I.Scripts.Displays.HUD
{
    public class PlayerDisplayHUD : MonoBehaviour
    {
        public PlayerController PlayerController;
        
        [Header("Refrences")]
        [SerializeField] private Image _playerIcon;
        [SerializeField] private Image _playerNumber;
        [SerializeField] private Image _playerHealthImage;
        [SerializeField] private Image _playerEnergyBackGroundImage;
        [SerializeField] private Image _playerEnergyFillImage;
        [SerializeField] private TextMeshProUGUI _playerCurrentStoneText;
        [SerializeField] private TextMeshProUGUI _playerMaxStoneText;
        [SerializeField] private TextMeshProUGUI _playerCurrentMetalText;
        [SerializeField] private TextMeshProUGUI _playerMaxMetalText;
        [SerializeField] private TextMeshProUGUI _playerCurrentCircuitText;
        [SerializeField] private TextMeshProUGUI _playerMaxCircuitText;
        
        [Header("Sprites")]
        [SerializeField] private List<Sprite> _playerIconSprites;
        [SerializeField] private List<Sprite> _playerNumbersSprite;
        [SerializeField] private Sprite _revolverBackground;
        [SerializeField] private Sprite _revolverFill;
        [SerializeField] private Sprite _riffleBackground;
        [SerializeField] private Sprite _riffleFill;

        private void Update()
        {
            if (!PlayerController) return;
            
            (int playerId, float maxHealth, float currentHealth, WeaponHandler equippedWeapon, 
                Dictionary<ResourcesEnum, int> inventory, int maxStone, int maxMetal, int maxCircuit) playerData
                    = PlayerController.GetCanvasHudData();
            
            // Icon and Number
            _playerIcon.sprite = _playerIconSprites[playerData.playerId];
            _playerNumber.sprite = _playerNumbersSprite[playerData.playerId];
            
            // Health
            float playerHealthNormalized =
                Mathf.Clamp01(playerData.currentHealth / playerData.maxHealth);
            _playerHealthImage.fillAmount = playerHealthNormalized;
            
            // Energy
            if (playerData.equippedWeapon.WeaponData._weaponTypesEnum == WeaponTypesEnum.Revolver)
            {
                _playerEnergyBackGroundImage.sprite = _revolverBackground;
                _playerEnergyFillImage.sprite = _revolverFill;
            }
            else
            {
                _playerEnergyBackGroundImage.sprite = _riffleBackground;
                _playerEnergyFillImage.sprite = _riffleFill;
            }
            
            float playerEnergyNormalized =
                Mathf.Clamp01(playerData.equippedWeapon.CurrentCapacity / playerData.equippedWeapon.WeaponData.Capacity);
            _playerEnergyFillImage.fillAmount = playerEnergyNormalized;
            
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
