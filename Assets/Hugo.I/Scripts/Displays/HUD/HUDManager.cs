using DG.Tweening;
using Hugo.I.Scripts.Game;
using Hugo.I.Scripts.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Hugo.I.Scripts.Displays.HUD
{
    public class HUDManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _playerHudPrefab;
        [SerializeField] private Transform _playerHudParent;
        [SerializeField] private Image _entrySceneImage;
        [SerializeField] private float _durationFade;
        [SerializeField] private AnimationCurve _fadeCurve;

        private void Start()
        {
            foreach (var player in GameManager.Instance.Players)
            {
                GameObject newPlayerHud = Instantiate(_playerHudPrefab, _playerHudParent);
                
                newPlayerHud.GetComponent<PlayerDisplayHUD>().PlayerController = player.GetComponent<PlayerController>();
            }
            
            Invoke(nameof(DoFadeEntryScene), 3f);
        }

        private void DoFadeEntryScene()
        {
            _entrySceneImage.DOFade(0, _durationFade).SetEase(_fadeCurve);
        }
    }
}
