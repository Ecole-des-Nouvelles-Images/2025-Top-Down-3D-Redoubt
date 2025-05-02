using System.Collections.Generic;
using UnityEngine;

namespace Hugo.I.Scripts.Player
{
    // EN gros dans PlayerController il y a la ref d'un QteSystem et ca en créé un quand on appuie sur le bouton et le stock dedans.
    // Ici ca créé une liste de vecteur 2 (qui est le QTE).
    // Et ensuite le player passe en mode QTE et à chaque appuie sur le pad ca lance une methode ici qui vérifie si c'est la bonne touche.
    // Et donc petit à petit ca vérifie le nombre de juste et la methode de lancement de QTE return un int qui est le nombre de ressource que le joueur va récolter.
    
    
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
        
        public (bool, int) CheckQte(Vector2 readValue)
        {
            bool isFinished = false;
            
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
                isFinished = true;
            }
            
            _advancement++;
            
            return (isFinished, Score);
        }
    }
}
