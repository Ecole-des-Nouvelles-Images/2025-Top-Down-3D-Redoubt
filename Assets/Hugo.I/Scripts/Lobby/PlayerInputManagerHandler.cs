using Hugo.I.Scripts.Game;
using Hugo.I.Scripts.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hugo.I.Scripts.Lobby
{
    public class PlayerInputManagerHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LaunchGameHandler _launchGameHandler;
        
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
            GameManager.Instance.Players.Add(playerInput.gameObject);
            int index = playerInput.playerIndex;
            playerInput.gameObject.GetComponent<PlayerController>().SetUp(index, GameManager.Instance.PlayersBaseData[index]);
            
            _launchGameHandler.StopMyCoroutine();
        }
    }
}