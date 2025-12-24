using Source.Data.Towers;
using Source.Event;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Source.UI.Shop
{
    public class LevelShopCell : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Color selectedColor = new (0.75f, 0.75f, 0.75f);
        [SerializeField] private Color normalColor = new (1.0f, 1.0f, 1.0f);
        [SerializeField] private Image baseImg;
        [SerializeField] private Image headImg;
        [SerializeField] private TMP_Text text;

        [SerializeField] private Image[] bg;

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
                BgSetColor(_selected ? selectedColor : normalColor);
            }
        }

        private void Awake()
        {
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

        private void BgSetColor(Color c)
        {
            foreach (var bgElem in bg)
                bgElem.color = c;
        }
    }
}