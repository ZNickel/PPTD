using Source.Event;
using Source.Game.Controllers;
using Source.Game.Entity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.UI.Hud
{
    public class TowerPopUp : MonoBehaviour
    {
        [SerializeField] private TMP_Text upgradePrice;
        [SerializeField] private Image upgradeImage;
        [SerializeField] private TMP_Text sellPrice;
        [SerializeField] private TMP_Text level;

        [SerializeField] private Color normal;
        [SerializeField] private Color inactive;
        [SerializeField] private Color error;
        
        private static readonly int HashShow = Animator.StringToHash("Show");

        private ResourceController _resourceController;
        private RectTransform _rt;
        private Animator _animator;
        private Camera _cam;

        private Vector3 _offset;
        private bool _isOpened;

        private TowerController _towerController;
        private Tower _target;
        
        private void Awake()
        {
            _rt = GetComponent<RectTransform>();
            _animator = GetComponent<Animator>();
            _animator.SetBool(HashShow, false);
            UIEventBus.Instance.EventShowTowerPopup.AddListener(Show);
            UIEventBus.Instance.EventHideTowerPopup.AddListener(Hide);
        }

        private void Start()
        {
            _resourceController = GameObject.FindWithTag("GameController").GetComponent<GameController>().CResource;
            _cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            _offset = _rt.sizeDelta / -2f;
        }

        private void Show(TowerController tc, Tower t)
        {
            _towerController = tc;
            
            if (t == null || _target == t)
            {
                Hide();
                return;
            }
            
            _isOpened = true;

            _rt.anchoredPosition = _cam.WorldToScreenPoint(t.transform.position) + _offset;
            _target = t;
            
            _animator.SetBool(HashShow, true);
            UpdateTextValues(false);
        }

        private void LateUpdate() => UpdateTextValues(true);

        private void Hide()
        {
            if (!_isOpened) return;
            _isOpened = false;
            _target = null;
            _animator.SetBool(HashShow, false);
        }
        
        public void UseSkill()
        {
            if (!_target) return;
            _towerController.Skill(_target);
            Hide();
        }
        
        public void LevelUp()
        {
            if (!_target) return;
            _towerController.LevelUp(_target);
            UpdateTextValues(false);
        }
        
        public void Sell()
        {
            if (!_target) return;
            _towerController.Sell(_target);
            Hide();
        }

        private void UpdateTextValues(bool upgradeOnly)
        {
            if (!_target) return;

            var coins = _resourceController.Coins;
            var price = _target.Data.LvlUpPrice(_target.Level + 1);
            
            var isMaxLevel = _target.Level >= _target.Data.LevelCount;

            upgradePrice.text = isMaxLevel ? "---" : $"-{price}";
            upgradePrice.color = upgradeImage.color = isMaxLevel ? inactive : price <= coins ? normal : error;

            if (upgradeOnly) return;
            level.text = $"{_target.Level}";
            sellPrice.text = $"+{_target.Data.SellPrice(_target.Level)}";
        }
    }
}