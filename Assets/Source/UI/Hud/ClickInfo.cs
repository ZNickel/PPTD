using Source.Event;
using UnityEngine;
using UnityEngine.UI;

namespace Source.UI.Hud
{
    public class ClickInfo : MonoBehaviour
    {
        [Header("Struct")] [SerializeField] private Image[] fillers;
        [SerializeField] private GameObject icon;

        [Space(10), Header("Colors")] [SerializeField]
        private Color lowColor = Color.greenYellow;
        [SerializeField] private Color highColor = Color.red;

        private Image _iconImage;
        private RectTransform _imageRect;
        private float _value;
        private float _lastClickTime;


        private void Start()
        {
            _iconImage = icon.GetComponent<Image>();
            _imageRect = icon.GetComponent<RectTransform>();
            UIEventBus.Instance.EventClickPowerChanged.AddListener(PowerChanged);
            UIEventBus.Instance.EventClick.AddListener(Click);
            
            SetColor();
        }

        private void PowerChanged(float v)
        {
            SetColor();
            _value = v;
        }

        private void Update()
        {
            ValidateIcon();
        }

        private void SetColor()
        {
            var filledCount = Mathf.RoundToInt(fillers.Length * _value);
            for (var i = 0; i < fillers.Length; i++)
            {
                fillers[i].color = i <= filledCount
                    ? Color.Lerp(lowColor, highColor, i / (float) fillers.Length)
                    : new Color(0, 0, 0, 0);
            }
        }

        private void ValidateIcon()
        {
            var t = Mathf.Clamp((Time.time - _lastClickTime) * 4, 0f, 1f);
            _imageRect.localScale = Vector3.Slerp(new Vector3(0.75f, 0.75f, 1f), new Vector3(1f, 1f, 1f), t);
            _iconImage.color = Color.Lerp(lowColor, highColor, _value);
        }

        public void Click() => _lastClickTime = Time.time;
    }
}