using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hugo.I.Scripts.Interactable.Tower;
using Hugo.I.Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hugo.I.Scripts.Game
{
    public static class GameManager
    {
        // Players
        public static List<GameObject> Players = new List<GameObject>();
        public static List<Vector3> SpawnPointsInGame = new List<Vector3>()
        {
            new Vector3(5, 0, 12), new Vector3(7, 0, 10),new Vector3(9, 0, 12),new Vector3(7, 0, 14)
        };

        public static List<Vector3> SpawnPointsLobby = new List<Vector3>()
        {
            new Vector3(5, 0, 12), new Vector3(7, 0, 10), new Vector3(9, 0, 12), new Vector3(7, 0, 14)
        };
        
        // Tower
        public static bool IsPowerPlantRepairs;
        public static TowerHandler ActualTowerGameObject;

        // Events
        public static event Action OnTriggerActive;
        public static void TriggerAction()
        {
            OnTriggerActive?.Invoke();
        }
        
        // Game over
        public static void APlayerDie(GameObject player)
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

        public static void GameOver()
        {
            Debug.Log("Game Over");

            foreach (GameObject player in Players)
            {
                player.GetComponent<PlayerInputHandler>().InputAreEnable = false;
            }
            
            ResetGame();
            ChangeScene(1, 2000);
        }

        public static void WinGame()
        {
            Debug.Log("Game Win");
            
            foreach (GameObject player in Players)
            {
                player.GetComponent<PlayerInputHandler>().InputAreEnable = false;
            }
            
            ResetGame();
            ChangeScene(1, 5000);
        }

        private static async void ChangeScene(int index, int delay)
        {
            await Task.Delay(delay);
            SceneManager.LoadScene(index);
        }

        private static void ResetGame()
        {
            IsPowerPlantRepairs = false;
        }
    }
}