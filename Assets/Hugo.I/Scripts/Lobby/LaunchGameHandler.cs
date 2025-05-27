using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hugo.I.Scripts.Lobby
{
    public class LaunchGameHandler : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _timeToWait;
        [SerializeField] private List<Collider> _players = new List<Collider>();
        
        private Collider _collider;
        
        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _players.Add(other);

                if (_players.Count >= 4)
                {
                    Debug.Log("launch game");
                    StartCoroutine(LaunchGame());
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _players.Remove(other);
                StopCoroutine(LaunchGame());
            }
        }

        private IEnumerator LaunchGame()
        {
            yield return new WaitForSeconds(_timeToWait);
            SceneManager.LoadScene(2);
        }
    }
}
