using System.Collections;
using System.Collections.Generic;
using Hugo.I.Scripts.Game;
using Hugo.I.Scripts.Player;
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
        private Coroutine _launchGameCoroutine;
        
        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _players.Add(other);
                
                if (_players.Count == GameManager.Instance.Players.Count)
                {
                    Debug.Log("launch game");
                    _launchGameCoroutine = StartCoroutine(LaunchGame());
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _players.Remove(other);
                StopMyCoroutine();
            }
        }

        private IEnumerator LaunchGame()
        {
            yield return new WaitForSeconds(_timeToWait);
            foreach (var player in GameManager.Instance.Players)
            {
                player.GetComponent<PlayerController>().ResetPlayer();
            }
            SceneManager.LoadScene(2);
        }

        public void StopMyCoroutine()
        {
            if (_launchGameCoroutine != null)
            {
                Debug.Log("stop launch game");
                StopCoroutine(_launchGameCoroutine);
                _launchGameCoroutine = null;
            }
        }
    }
}
