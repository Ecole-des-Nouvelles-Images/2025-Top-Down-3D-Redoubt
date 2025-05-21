using TMPro;
using UnityEngine;

namespace Hugo.I.Scripts.Displays.InGame_WorldSpace
{
    public class ResourceWorldSpaceDisplay : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private TextMeshProUGUI _currentResourceText;

        public void UpdateDisplay(int currentResource)
        {
            _currentResourceText.text = currentResource.ToString();
        }
    }
}
