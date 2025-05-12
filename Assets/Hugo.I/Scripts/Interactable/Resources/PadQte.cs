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
        
        public (int advancement, bool isCorrect, bool isFinished) CheckQte(Vector2 readValue)
        {
            bool isCorrect = false;
            bool isFinished = false;
            
            if (readValue == Qte[_advancement])
            {
                Score++;
                isCorrect = true;
            }
            
            if (_advancement == Qte.Count - 1)
            {
                isFinished = true;
            }
            
            _advancement++;
            
            return (_advancement, isCorrect, isFinished);
        }
    }
}
