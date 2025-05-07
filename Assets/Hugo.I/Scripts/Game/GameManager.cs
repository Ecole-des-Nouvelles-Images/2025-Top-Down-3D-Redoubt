using System;
using System.Collections.Generic;
using Hugo.I.Scripts.Interactable.Tower;
using UnityEngine;

namespace Hugo.I.Scripts.Game
{
    public static class GameManager
    {
        // Players
        public static List<GameObject> Players = new List<GameObject>();
        public static List<Vector3> SpawnPoints = new List<Vector3>()
        {
            new Vector3(5, 0, 12), new Vector3(7, 0, 10),new Vector3(9, 0, 12),new Vector3(7, 0, 14),
        };
        
        // Tower
        public static bool IsPowerPlantRepairs;
        public static TowerHandler ActualTowerGameObject;

        public static event Action OnTriggerActive;
        public static void TriggerAction()
        {
            OnTriggerActive?.Invoke();
        }
    }
}