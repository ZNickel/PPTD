using System.Collections;
using Source.Event;
using Source.Game.Controllers;
using TMPro;
using UnityEngine;

namespace Source.UI.Hud
{
    public class WaveIndicator : MonoBehaviour
    {
        private static readonly int HashShow = Animator.StringToHash("Show");
        [SerializeField] private TMP_Text timeText;

        private Animator _animator;
        private RectTransform _rt;
        private Camera _cam;
        private WaveController _waveController;
        private float _time;
        private Vector3 _offset;
        private bool _clicked;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rt = GetComponent<RectTransform>();
            UIEventBus.Instance.ShowWaveIndicator.AddListener(Show);
        }

        private void Start()
        {
            _cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            _offset = _rt.sizeDelta / -2f;
        }

        private void Show(WaveController wc, Vector3 pos)
        {
            _rt.anchoredPosition = _cam.WorldToScreenPoint(pos) + _offset;
            _waveController = wc;
            _animator.SetBool(HashShow, true);
            _time = wc.WaveDelay;
            _clicked = false;
        }

        private void Update()
        {
            if (_clicked) return;
            _time -= Time.deltaTime;
            timeText.text = Mathf.CeilToInt(_time) + "";
            
            if (_time <= 0)
            {
                _clicked = true;
                _animator.SetBool(HashShow, false);
            }
        }

        public void OnClick()
        {
            if (_clicked) return;
            _clicked = true;
            _waveController.ForcedLaunch();
            _animator.SetBool(HashShow, false);
        }
    }
}