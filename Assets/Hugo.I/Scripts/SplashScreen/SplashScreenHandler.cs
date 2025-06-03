using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Hugo.I.Scripts.SplashScreen
{
    public class SplashScreenHandler : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _fadeInDuration;
        [SerializeField] private float _fadeOutDuration;
        [SerializeField] private float _timeScreenEnsiStay;
        [SerializeField] private float _timeScreenLoadingStay;
        
        [Header("References")]
        [SerializeField] private Image _screenEnsi;
        [SerializeField] private Image _screenLeading;

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            _screenEnsi.DOFade(1f, _fadeInDuration);
            Invoke(nameof(FadeOutEnsi), _timeScreenEnsiStay);
        }

        private void FadeOutEnsi()
        {
            _screenEnsi.DOFade(0f, _fadeOutDuration);
            Invoke(nameof(FadeInLoadingScreen), _fadeOutDuration);
        }

        private void FadeInLoadingScreen()
        {
            _screenLeading.DOFade(1f, _fadeInDuration);
            Invoke(nameof(ChangeScene), _timeScreenLoadingStay);
        }

        private void ChangeScene()
        {
            SceneManager.LoadScene(1);
        }
    }
}
