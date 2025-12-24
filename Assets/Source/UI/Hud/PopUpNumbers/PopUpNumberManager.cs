using System.Collections.Generic;
using Source.Event;
using Source.Game.Entity;
using UnityEngine;

namespace Source.UI.Hud.PopUpNumbers
{
    public class PopUpNumberManager : MonoBehaviour
    {
        [SerializeField] private GameObject popUpNumberPrefab;
        [SerializeField, Range(0.1f, 10f)] private float showTime = 1f;
        [SerializeField] private Vector2 posOffset = new(0f, 0f);

        private readonly Stack<PopUpNumber> _free = new();
        
        private Vector2 _offset;
        private Camera _cam;

        private void Awake()
        {
            UIEventBus.Instance.ShowPopupNumber.AddListener(Show);
        }

        private void Start()
        {
            _cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            _offset = popUpNumberPrefab.GetComponent<RectTransform>().sizeDelta / -2f + posOffset;
        }
        
        private void Show(int value, Enemy e)
        {
            if (e == null) return;
            var contains = _free.TryPop(out var result);
            var popUp = contains ? result : CreateNew();
            var wPos = _cam.WorldToScreenPoint(e.transform.position);
            
            popUp.Rt.anchoredPosition = new Vector2(wPos.x, wPos.y) + _offset;
            popUp.Show(value, _free, showTime);
        }

        private PopUpNumber CreateNew()
        {
            return Instantiate(popUpNumberPrefab, transform).GetComponent<PopUpNumber>();
        }
    }
}