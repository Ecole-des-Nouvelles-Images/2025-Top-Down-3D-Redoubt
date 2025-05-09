using System;
using Hugo.I.Scripts.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Hugo.I.Scripts.Displays.InGame_WorldSpace
{
    public class PlayerWorldSpaceDisplay : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private Image _playerHealthImage;
        [SerializeField] private Image _playerOverheatingImage;

        private void Update()
        {
            (float maxHealth, float currentHealth, float overheatingLimit, float currentOverheating) playerData
                = _playerController.GetCanvasWorldSpaceData();
            
            // Health
            float playerHealthNormalized =
                Mathf.Clamp01(playerData.currentHealth / playerData.maxHealth);
            _playerHealthImage.fillAmount = playerHealthNormalized;
            
            // Overheating
            float playerOverheatingNormalized = Mathf.Clamp01(playerData.currentOverheating / playerData.overheatingLimit);
            _playerOverheatingImage.fillAmount = playerOverheatingNormalized;
        }
    }
}
