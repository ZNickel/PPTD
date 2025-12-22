using System;
using System.Collections;
using Source.Data.Towers;
using Source.Event;
using Source.Game.Entity;
using UnityEngine;

namespace Source.UI.Shop
{
    public class LevelShop : MonoBehaviour
    {
        [SerializeField] private GameObject container;
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private TowerData[] towers;

        private RectTransform _rect;
        
        private LevelShopCell[] _cells;

        public TowerData SelectedTowerData { get; private set; }
        public bool Opened { get; private set; }

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            _cells = GenerateCells();
        }

        private LevelShopCell[] GenerateCells()
        {
            var cells = new LevelShopCell[towers.Length];
            for (var i = 0; i < towers.Length; i++)
            {
                var o = Instantiate(cellPrefab, container.transform);
                var s = o.GetComponent<LevelShopCell>();
                s.Setup(this, towers[i]);
                cells[i] = s;
            }

            return cells;
        }

        public void Select(LevelShopCell cell)
        {
            foreach (var c in _cells)
            {
                if (c.Selected)
                {
                    c.Selected = false;
                    
                    if (c == cell || cell == null) 
                        SelectedTowerData = null;
                }
                else if (c == cell)
                {
                    c.Selected = true;
                    SelectedTowerData = c.Data;
                }
            }
            
            UIEventBus.Instance.Trigger_EventSelectShopItem(SelectedTowerData);
        }

        public void Handle_ShowHide()
        {
            StartCoroutine(ShowHide());
            
            Opened = !Opened;

            if (!Opened) Select(null);
        }

        private IEnumerator ShowHide()
        {
            const float t = .33f;
            var elapsed = 0f;

            var startPos = Opened ? 0f : _rect.rect.width;
            var endPos = Opened ? _rect.rect.width : 0f;
            
            while (elapsed < t)
            {
                elapsed += Time.deltaTime;
                var x = Mathf.Lerp(startPos, endPos, elapsed / t);
                _rect.anchoredPosition =  new Vector2(x, _rect.anchoredPosition.y);
                yield return null;
            }
            _rect.anchoredPosition =  new Vector2(endPos, _rect.anchoredPosition.y);
        }
    }
}