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
            
            if (playerInput.playerIndex == 0)
            {
                playerInput.gameObject.GetComponent<PlayerController>().PlayerId = 0;
            }
            if (playerInput.playerIndex == 1)
            {
                playerInput.gameObject.GetComponent<PlayerController>().PlayerId = 1;
            }
            if (playerInput.playerIndex == 2)
            {
                playerInput.gameObject.GetComponent<PlayerController>().PlayerId = 2;
            }
            if (playerInput.playerIndex == 3)
            {
                playerInput.gameObject.GetComponent<PlayerController>().PlayerId = 3;
            }
        }
    }
}
