using Hugo.I.Scripts.Game;
using Hugo.I.Scripts.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hugo.I.Scripts.Lobby
{
    public class PlayerInputManagerHandler : MonoBehaviour
    {
        private PlayerInputManager _playerInputManager;

        private void Awake()
        {
            _playerInputManager = GetComponent<PlayerInputManager>();
        }

        private void OnEnable()
        {
            _playerInputManager.onPlayerJoined += OnPlayerJoined;
        }

        private void OnDisable()
        {
            _playerInputManager.onPlayerJoined -= OnPlayerJoined;
        }

        private void OnPlayerJoined(PlayerInput playerInput)
        {
            GameManager.Players.Add(playerInput.gameObject);
            playerInput.gameObject.GetComponent<PlayerController>().PlayerId = playerInput.playerIndex;
        }
    }
}
