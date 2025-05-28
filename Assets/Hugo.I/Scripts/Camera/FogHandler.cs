using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Hugo.I.Scripts.Camera
{
    public class FogHandler : MonoBehaviour
    {
        [Header("Fog Settings")]
        [SerializeField] private List<Vector3> _listScaleFog;
        [SerializeField] private float _duration;
        [SerializeField] private float _delay;
        [SerializeField] private AnimationCurve _easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private int _index;

        public void InvokeFogBackOff(int index)
        {
            _index = index;
            Invoke(nameof(FogBackOff), _delay);
        }

        private void FogBackOff()
        {
            transform.DOScale(_listScaleFog[_index], _duration).SetEase(_easeCurve);
        }
    }
}