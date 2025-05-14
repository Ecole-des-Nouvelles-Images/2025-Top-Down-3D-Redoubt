using DG.Tweening;
using UnityEngine;

namespace Hugo.I.Scripts.Shield
{
    public class ShieldZoneHandler : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Vector3 _startSize;
        [SerializeField] private Vector3 _endSize;
        [SerializeField] private float _duration;
        [SerializeField] private AnimationCurve _sizeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private void Start()
        {
            transform.localScale = _startSize;
            transform.DOScale(_endSize, _duration).SetEase(_sizeCurve);
        }
    }
}
