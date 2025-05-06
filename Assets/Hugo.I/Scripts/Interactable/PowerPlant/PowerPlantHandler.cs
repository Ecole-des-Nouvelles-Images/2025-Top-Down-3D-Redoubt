using Hugo.I.Scripts.Game;
using Hugo.I.Scripts.Utils;
using UnityEngine;

namespace Hugo.I.Scripts.Interactable.PowerPlant
{
    public class PowerPlantHandler : MonoBehaviour, IInteractable
    {
        public bool IsRepaired;
        public int QteSize;
        [SerializeField] private int _repairedAdvancement;
        [SerializeField] private int _numberOfRepairs;

        public void Repair()
        {
            _repairedAdvancement++;

            if (_repairedAdvancement >= _numberOfRepairs)
            {
                IsRepaired = true;
                GameManager.IsPowerPlantRepairs = true;
                GameManager.TriggerAction();
            }
            
            Debug.Log("Repair advancement : " + _repairedAdvancement + ". Is repaired : " + IsRepaired);
        }

        public void OnEnterZone()
        {
            throw new System.NotImplementedException();
        }

        public void OnExitZone()
        {
            throw new System.NotImplementedException();
        }
    }
}
