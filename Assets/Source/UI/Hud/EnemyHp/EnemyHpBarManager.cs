using System.Collections.Generic;
using Source.Event;
using Source.Game.Entity;
using UnityEngine;

namespace Source.UI.Hud.EnemyHp
{
    public class EnemyHpBarManager : MonoBehaviour
    {
        [SerializeField] private GameObject hpBarPrefab;
        [SerializeField] private Vector3 posOffset = new(0f, -55f, 0f);

        private readonly Stack<EnemyHpBar> _free = new();
        private readonly Dictionary<EnemyHpBar, Enemy> _bBars = new();
        private readonly Dictionary<Enemy, EnemyHpBar> _bEnemies = new();
        private Vector3 _offset;

        private int _idCounter;
        private Camera _cam;

        private void Awake()
        {
            UIEventBus.Instance.AttachHpBar.AddListener(Attach);
            UIEventBus.Instance.DetachHpBar.AddListener(Detach);
            UIEventBus.Instance.UpdateEnemyHp.AddListener(UpdateHp);
        }

        private void Start()
        {
            _cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            var rt = hpBarPrefab.GetComponent<RectTransform>();
            var centerOffset = new Vector3(rt.sizeDelta.x / -2f, rt.sizeDelta.y / -2f, 0f);
            _offset = centerOffset + posOffset;
        }

        private void LateUpdate()
        {
            if (_bBars.Count == 0) return;

            foreach (var bar in _bBars.Keys)
            {
                var enemy = _bBars[bar];
                if (!enemy) continue;
                bar.Rt.anchoredPosition = _cam.WorldToScreenPoint(enemy.transform.position) + _offset;
            }
        }

        private void Attach(Enemy enemy)
        {
            var contains = _free.TryPop(out var result);
            var freeBar = contains ? result : CreateNew();
            _bBars[freeBar] = enemy;
            _bEnemies[enemy] = freeBar;
            freeBar.Show();
        }

        private void Detach(Enemy enemy)
        {
            var bar = _bEnemies[enemy];
            bar.Hide(() =>
            {
                _bEnemies.Remove(enemy);
                _bBars.Remove(bar);
                _free.Push(bar);
            });
        }

        private void UpdateHp(Enemy e, float hpNormalized)
        {
            var bar = _bEnemies[e];
            bar.UpdateHp(hpNormalized);
        }

        private EnemyHpBar CreateNew()
        {
            var o = Instantiate(hpBarPrefab, transform);
            var hpBar = o.GetComponent<EnemyHpBar>();
            hpBar.Id = _idCounter++;
            return hpBar;
        }
    }
}