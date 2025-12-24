using Source.Data;
using Source.Event;
using UnityEngine;

namespace Source.Game.Controllers
{
    public class ClickController : MonoBehaviour
    {
        [Header("Click settings")]
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private float falloff = .002f;
        [SerializeField] private float increment = 0.3f;
        
        private float _clickPower;

        private float ClickPower
        {
            get => _clickPower;
            set
            {
                _clickPower = Mathf.Clamp(value, 0f, 1f);
                UIEventBus.Instance.Trigger_EventClickPowerChanged(_clickPower);
            }
        }

        private void Awake()
        {
            _clickPower = 0f;
            UIEventBus.Instance.EventMouseClick.AddListener(Click);
        }
        
        private void Click()
        {
            ClickPower += curve.Evaluate(ClickPower) * increment;
        }
        
        private void FixedUpdate()
        {
            ClickPower = Mathf.Max(0, ClickPower - falloff);
        }
    }
}