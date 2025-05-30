using Hugo.I.Scripts.Game;
using Hugo.I.Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hugo.I.Scripts.Displays.PausePanel
{
    public class CanvasPauseHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _panelPause;
        
        private void OnEnable()
        {
            GameManager.Instance.OnStart += PlayerStart;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnStart -= PlayerStart;
            
        }

        private void PlayerStart()
        {
            _panelPause.SetActive(true);
            Time.timeScale = 0;

            foreach (var player in GameManager.Instance.Players)
            {
                player.GetComponent<PlayerController>().SetInput(false);
            }
        }

        public void Resum()
        {
            _panelPause.SetActive(false);
            Time.timeScale = 1;
            
            foreach (var player in GameManager.Instance.Players)
            {
                player.GetComponent<PlayerController>().SetInput(true);
            }
        }

        public void BackLobby()
        {
            SceneManager.LoadScene(1);
            
            GameManager.Instance.ResetGame();
            
            foreach (var player in GameManager.Instance.Players)
            {
                player.GetComponent<PlayerController>().SetInput(true);
            }
        }

        public void BackMenu()
        {
            SceneManager.LoadScene(0);
            
            GameManager.Instance.ResetGame();
            
            foreach (var player in GameManager.Instance.Players)
            {
                Destroy(player);
            }
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
