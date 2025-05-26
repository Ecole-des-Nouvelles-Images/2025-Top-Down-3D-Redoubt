using DG.Tweening;
using UnityEngine;

namespace Hugo.I.Scripts.Player
{
    public class CircleImageHandler : MonoBehaviour
    {
        [Header("Scale Settings")]
        [SerializeField] private Vector3 _startScale;
        [SerializeField] private Vector3 _endScale;
        [SerializeField] private float _scaleDuration;

        [Header("Tween Settings")]
        [SerializeField] private Ease _ease = Ease.InOutSine;

        private void Start()
        {
            transform.DOScale(_endScale, _scaleDuration).SetEase(_ease).SetLoops(-1, LoopType.Yoyo);
        }
    }
}