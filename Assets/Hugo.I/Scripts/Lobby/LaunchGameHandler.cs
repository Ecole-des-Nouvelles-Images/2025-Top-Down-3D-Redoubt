using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Hugo.I.Scripts.Game;
using Hugo.I.Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Hugo.I.Scripts.Lobby
{
    public class LaunchGameHandler : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _timeToWait;
        [SerializeField] private List<Collider> _players = new List<Collider>();
        
        [Header("References")]
        [SerializeField] private Image _fadeInImage;
        [SerializeField] private float _fadeDuration;
        [SerializeField] private AnimationCurve _fadeCurve;
        
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
                    _fadeInImage.DOFade(1f, _fadeDuration).SetEase(_fadeCurve);
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
            SceneManager.LoadScene(3);
            
            foreach (var player in GameManager.Instance.Players)
            {
                player.GetComponent<PlayerController>().ResetPlayer();
            }
        }

        public void StopMyCoroutine()
        {
            if (_launchGameCoroutine != null)
            {
                Debug.Log("stop launch game");
                StopCoroutine(_launchGameCoroutine);
                _launchGameCoroutine = null;
                _fadeInImage.DOFade(0f, _fadeDuration).SetEase(_fadeCurve);
            }
        }
    }
}
