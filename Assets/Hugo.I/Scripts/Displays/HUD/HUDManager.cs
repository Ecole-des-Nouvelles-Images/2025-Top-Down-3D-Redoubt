using Hugo.I.Scripts.Game;
using Hugo.I.Scripts.Player;
using UnityEngine;

namespace Hugo.I.Scripts.Displays.HUD
{
    public class HUDManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _playerHudPrefab;
        [SerializeField] private Transform _playerHudParent;

        private void Start()
        {
            foreach (var player in GameManager.Instance.Players)
            {
                GameObject newPlayerHud = Instantiate(_playerHudPrefab, _playerHudParent);
                
                newPlayerHud.GetComponent<PlayerDisplayHUD>().PlayerController = player.GetComponent<PlayerController>();
            }
        }
    }
}
