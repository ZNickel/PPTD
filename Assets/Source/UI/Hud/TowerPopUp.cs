using System;
using Source.Event;
using Source.Game.Controllers;
using Source.Game.Entity;
using UnityEngine;

namespace Source.UI.Hud
{
    public class TowerPopUp : MonoBehaviour
    {
        private static readonly int AHashShow = Animator.StringToHash("Show");
        
        private Animator _animator;
        private RectTransform _rectTransform;
        
        private TowerController _tc;
        private Tower _t;
        
        private bool _isVisible = false;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rectTransform = GetComponent<RectTransform>();
            _animator.SetBool(AHashShow, false);
            UIEventBus.Instance.EventShowTowerPopup.AddListener(Show);
            UIEventBus.Instance.EventHideTowerPopup.AddListener(Hide);
        }

        private void Show(TowerController tc, float x, float y, Tower t)
        {
            if (_isVisible) return;
            if (_tc) _tc.ShowenMenu = true;
            if (t == _t)
            {
                Hide();
                return;
            }
                
            _isVisible = true;
            _rectTransform.anchoredPosition = new Vector2(x, y);
            _tc = tc;
            _t = t;
            _animator.SetBool(AHashShow, true);
        }

        private void Hide()
        {
            if (_tc) _tc.ShowenMenu = false;
            _isVisible = false;
            _tc = null;
            _t = null;
            _animator.SetBool(AHashShow, false);
        }

        public void Handle_Sell()
        {
            _tc.ShowenMenu = false;
            UIEventBus.Instance.Trigger_EventHideTowerPopup();
        }

        public void Handle_LevelUp()
        {
            Debug.Log("LvlUp s1");
            if (!_t || !_tc || !_isVisible) return;
            Debug.Log("LvlUp s2");
            _tc.LevelUp(_t);
            Debug.Log("LvlUp s3");
        }

        public void Handle_Skill()
        {
            if (!_t || !_tc || !_isVisible) return;
            _tc.Skill(_t);
        }

        private void Handle_HideEnd()
        {
            _rectTransform.anchoredPosition = new Vector2(-10000, -10000);
        }
    }
}