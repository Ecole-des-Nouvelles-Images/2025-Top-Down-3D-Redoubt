using System.Collections.Generic;
using Hugo.I.Scripts.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Hugo.I.Scripts.Displays.InGame_WorldSpace
{
    public class PlayerWorldSpaceDisplayInteractions : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private GameObject _panelInteractions;
        [SerializeField] private Image _fillInteractionButtonImage;
        [SerializeField] private GameObject _panelQte;
        [SerializeField] private GameObject _qtePrefabGameObject;
        [SerializeField] private Sprite _topSprite;
        [SerializeField] private Sprite _botSprite;
        [SerializeField] private Sprite _leftSprite;
        [SerializeField] private Sprite _rightSprite;
        
        private List<GameObject> _qteGameObjects = new List<GameObject>();

        public void DisplayInteractionsButton()
        {
            _fillInteractionButtonImage.fillAmount = 0;
            _panelInteractions.SetActive(true);
        }

        public void HideInteractionsButton()
        {
            _panelInteractions.SetActive(false);
        }

        public void UpdateInteractionButtonFill(float value, int maxValue)
        {
            float valueNormalized = Mathf.Clamp01(value / maxValue);
            _fillInteractionButtonImage.fillAmount = valueNormalized;
        }

        public void DisplayQteButton(List<Vector2> qteList)
        {
            foreach (var qte in qteList)
            {
                GameObject newQte = Instantiate(_qtePrefabGameObject, _panelQte.transform.position, Quaternion.identity, _panelQte.transform);
                newQte.transform.rotation = new Quaternion(0, 0, 0, 0);
                if (qte == new Vector2(-1, 0))
                {
                    newQte.GetComponent<Image>().sprite = _leftSprite;
                }
                if (qte == new Vector2(1, 0))
                {
                    newQte.GetComponent<Image>().sprite = _rightSprite;
                }
                if (qte == new Vector2(0, -1))
                {
                    newQte.GetComponent<Image>().sprite = _botSprite;
                }
                if (qte == new Vector2(0, 1))
                {
                    newQte.GetComponent<Image>().sprite = _topSprite;
                }
                _qteGameObjects.Add(newQte);
            }
            
            _panelQte.SetActive(true);
        }
        
        public void HideQteButton()
        {
            foreach (var qte in _qteGameObjects)
            {
                Destroy(qte);
            }
            _qteGameObjects.Clear();
            _panelQte.SetActive(false);
        }

        public void DisplayQteAdvancement(int advancement, bool isCorrect)
        {
            _qteGameObjects[advancement - 1].GetComponent<Image>().color = isCorrect ? Color.green : Color.red;
        }
    }
}
