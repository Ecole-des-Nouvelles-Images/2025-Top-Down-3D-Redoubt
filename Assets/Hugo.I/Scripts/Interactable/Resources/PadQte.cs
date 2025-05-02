using System.Collections.Generic;
using UnityEngine;

namespace Hugo.I.Scripts.Interactable.Resources
{
    public struct PadQte
    {
        public List<Vector2> Qte;
        private int _advancement;
        public int Score;
        
        public PadQte(int size)
        {
            Qte = new List<Vector2>();
            _advancement = 0;
            Score = 0;
            
            Qte = GenerateQte(size);
        }

        private List<Vector2> GenerateQte(int size)
        {
            List<Vector2> qte = new List<Vector2>();
            List<Vector2> qtePossibilities = new List<Vector2>
            {
                new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0)
            };

            for (int i = 0; i < size; i++)
            {
                qte.Add(qtePossibilities[Random.Range(0, qtePossibilities.Count)]);
            }
            
            return qte;
        }
        
        public bool CheckQte(Vector2 readValue)
        {
            if (readValue == Qte[_advancement])
            {
                Debug.Log("Succes");
                Score++;
            }
            else
            {
                Debug.Log("Fail");
            }
            
            if (_advancement == Qte.Count - 1)
            {
                Debug.Log("QTE fini, score : " + Score);
                return true;
            }
            
            _advancement++;
            
            return false;
        }
    }
}
