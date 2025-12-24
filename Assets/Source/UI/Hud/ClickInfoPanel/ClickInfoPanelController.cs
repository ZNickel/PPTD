using Source.Event;
using UnityEngine;

namespace Source.UI.Hud.ClickInfoPanel
{
    public class ClickInfoPanelController : MonoBehaviour
    {
        [SerializeField] private RectTransform filler;

        private float _max;
        
        private void Awake()
        {
            UIEventBus.Instance.EventClickPowerChanged.AddListener(Upd);
        }

        private void Start()
        {
            _max = filler.sizeDelta.y;
        }

        private void Upd(float v)
        {
            filler.anchoredPosition =  new Vector2(filler.anchoredPosition.x, v * _max);
        }

        public void OnClick()
        {
            UIEventBus.Instance.Trigger_EventMouseClick();
        }
    }
}