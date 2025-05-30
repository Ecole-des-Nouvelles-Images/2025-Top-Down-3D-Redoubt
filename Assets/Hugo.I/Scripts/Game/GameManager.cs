using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hugo.I.Scripts.Interactable.Tower;
using Hugo.I.Scripts.Player;
using Hugo.I.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hugo.I.Scripts.Game
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        // Players
        public List<GameObject> Players = new List<GameObject>();
        public List<PlayerBaseData> PlayersBaseData = new List<PlayerBaseData>();
        public List<Vector3> SpawnPointsInGame = new List<Vector3>()
        {
            new Vector3(5, 20, 12), new Vector3(7, 20, 10),new Vector3(9, 20, 12),new Vector3(7, 20, 14)
        };

        public List<Vector3> SpawnPointsLobby = new List<Vector3>()
        {
            new Vector3(-6, 20, 2), new Vector3(-2, 20, 2), new Vector3(2, 20, 2), new Vector3(6, 20, 2)
        };
        
        // Tower
        public bool IsPowerPlantRepairs;
        public TowerHandler ActualTowerGameObject;

        // Events
        public event Action OnPowerPlantRestore;
        public void PowerPlantRestore() => OnPowerPlantRestore?.Invoke();
        
        public event Action OnStart;
        public void Start() => OnStart?.Invoke();
        
        // Game over
        public void APlayerDie(GameObject player)
        {
            if (Players.Contains(player))
            {
                Players.Remove(player);
            }

            if (Players.Count == 0)
            {
                GameOver();
            }
        }

        public void GameOver()
        {
            Debug.Log("Game Over");

            foreach (GameObject player in Players)
            {
                player.GetComponent<PlayerInputHandler>().InputAreEnable = false;
            }
            
            ResetGame();
            ChangeScene(1, 2000);
        }

        public void WinGame()
        {
            Debug.Log("Game Win");
            
            foreach (GameObject player in Players)
            {
                player.GetComponent<PlayerInputHandler>().InputAreEnable = false;
            }
            
            ResetGame();
            ChangeScene(1, 5000);
        }

        private async void ChangeScene(int index, int delay)
        {
            await Task.Delay(delay);
            SceneManager.LoadScene(index);
        }

        public void ResetGame()
        {
            IsPowerPlantRepairs = false;

            foreach (var player in Players)
            {
                player.GetComponent<PlayerController>().ResetPlayer();
            }
        }
    }
}