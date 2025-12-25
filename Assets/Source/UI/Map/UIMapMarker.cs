using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.UI.Map
{
    public class UIMapMarker : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private Image enabledImage;
        
        private UIMapController _controller;
        private LevelData _data;
        private bool _enabled;
        
        public void Setup(LevelData data, UIMapController controller, Vector2 pos)
        {
            var rt = GetComponent<RectTransform>();
            rt.anchoredPosition = pos + rt.sizeDelta / -2f;
            
            _controller = controller;
            _enabled = data != null;
            enabledImage.gameObject.SetActive(_enabled);
            _data = data;
            text.text = !_enabled ? "-" : $"{data.Index + 1}";
        }

        public void OnClick()
        {
            if (!_enabled) return;
            _controller.Select(_data);
        }
    }
}