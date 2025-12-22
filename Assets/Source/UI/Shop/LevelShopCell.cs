using System;
using Source.Data.Towers;
using Source.Event;
using Source.Game.Entity;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Source.UI.Shop
{
    public class LevelShopCell : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image baseImg;
        [SerializeField] private Image headImg;
        [SerializeField] private TMP_Text text;

        private Image _bg;
        private CanvasGroup _canvasGroup;
            
        private bool _selected;
        private bool _available;
        private LevelShop _shop;

        public TowerData Data { get; private set; }

        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                _bg.color = _selected ? new Color(0.5f, 0.5f, 0.5f) : new Color(1.0f, 1.0f, 1.0f);
            }
        }

        private void Awake()
        {
            _bg = GetComponent<Image>();
            _canvasGroup = GetComponent<CanvasGroup>();
            UIEventBus.Instance.EventCoinCountChanged.AddListener(ValidateAvailable);
        }

        private void ValidateAvailable(int coins)
        {
            if (Data == null) return;
            
            _available = Data.Price <= coins;
            _canvasGroup.alpha = !_available ? 0.5f : 1.0f;
            if (Selected && !_available) _shop.Select(this);
        }

        public void Setup(LevelShop shop, TowerData data)
        {
            _shop = shop;
            Data = data;
            headImg.sprite = data.HeadSprites[0];
            text.text = data.Price.ToString();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_available) return;
            _shop.Select(this);
        }
    }
}