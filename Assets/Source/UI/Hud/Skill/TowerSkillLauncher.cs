using System;
using Source.Event;
using Source.Game.Entity;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Source.UI.Hud.Skill
{
    public class TowerSkillLauncher : MonoBehaviour
    {
        private static readonly int HashShow = Animator.StringToHash("Show");
        private static readonly int HashClick = Animator.StringToHash("Click");

        [Serializable]
        private class Bar
        {
            [Header("Colors")]
            [SerializeField] private Color bad = new (0.8f, 0.2f, 0.1f);
            [SerializeField] private Color good = new (0.1f, 0.8f, 0.2f);
            [Header("Struct")]
            [SerializeField] private TMP_Text text;
            [SerializeField] private GameObject filler;
            
            private Image _image;
            private RectTransform _rt;
            private float _max;

            public void Setup()
            {
                _rt = filler.GetComponent<RectTransform>();
                _max = -_rt.sizeDelta.x;
                _image = filler.GetComponent<Image>();
            }
            
            public void Update(float t, string s)
            {
                text.text = s;
                t = Math.Clamp(t, 0f, 1f);
                var x = Mathf.Lerp(0, _max, t);
                _rt.anchoredPosition = new Vector2(x, 0f);
                _image.color = Color.Lerp(bad, good, t);
            }
        }

        [SerializeField] private Animator mainAnimator;
        [SerializeField] private Animator btnAnimator;
        
        [SerializeField] private Bar timeBar;
        [SerializeField] private Bar clickBar;

        private CanvasGroup _cg;
        private RectTransform _rt;
        
        private Camera _cam;
        private Tower _tower;
        private int _clicks;
        private float _elapsed;
        private bool _isLaunched;
        
        private void Awake()
        {
            _rt = GetComponent<RectTransform>();
            _cg = GetComponent<CanvasGroup>();
            UIEventBus.Instance.EnableSkillLauncher.AddListener(Launch);
        }

        private void Start()
        {
            _cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            timeBar.Setup();
            clickBar.Setup();
        }

        private UnityAction<(Tower, float)> _finAct;
        
        private void Launch(Tower t, UnityAction<(Tower, float)> finAct)
        {
            _tower = t;
            _finAct = finAct;
            _clicks = 0;
            _elapsed = _tower.Data.Skill.ClickTime;
            _cg.blocksRaycasts = _isLaunched = true;

            Vector3 offset = _rt.sizeDelta / -2f;
            _rt.anchoredPosition = _cam.WorldToScreenPoint(t.transform.position) + offset;
            
            mainAnimator.SetBool(HashShow, true);
            
            timeBar.Update(0f, $"{_tower.Data.Skill.ClickTime:F1}");
            clickBar.Update(1f, $"{0}/{t.Data.Skill.ClickCount}");
        }

        private void Update()
        {
            if (!_isLaunched) return;
            
            _elapsed -= Time.deltaTime;

            var clamped = Mathf.Max(_elapsed, 0f);
            timeBar.Update(1f - clamped / _tower.Data.Skill.ClickTime, $"{clamped:F1}");
            
            if (_elapsed <= 0f) Finish();
        }

        public void Click()
        {
            if (!_isLaunched) return;
            var maxClicks = _tower.Data.Skill.ClickCount;
            _clicks++;
            btnAnimator.SetTrigger(HashClick);
            var f = Mathf.Min(_clicks, maxClicks) / (float) maxClicks;
            clickBar.Update(1f - f, $"{_clicks}/{maxClicks}");
            if (_clicks >= maxClicks) Finish();
        }
        
        private void Finish()
        {
            _cg.blocksRaycasts = _isLaunched = false;
            mainAnimator.SetBool(HashShow, false);
            var perf = Mathf.Clamp(_clicks / (float)_tower.Data.Skill.ClickCount, 0f, 1f);
            _finAct.Invoke((_tower, perf));
        }
    }
}