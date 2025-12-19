using System;
using Source.Data;
using Source.Game.Controllers;
using UnityEngine;

namespace Source.Game.Entity
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer srHead;
        [SerializeField] private GameObject bullet;

        public TowerData Data => _data;
        public int Level => _level;
        
        private GameObject _head;
        private TowerData _data;
        private int _level;

        private WaveController _waveController;
        private TowerController _towerController;

        private TowerStats _stats;

        private Enemy _target;
        private float _elapsed;
        
        public void Setup(WaveController wc, TowerController tc, TowerData d)
        {
            _waveController = wc;
            _towerController = tc;
            _data = d;
            srHead.sprite = d.HeadSprites[0];
            _stats = _data.GetStats(0);
            _head = srHead.gameObject;
        }

        private void Update()
        {
            if (!_target) return;
        }

        private void FixedUpdate()
        {
            if (_target == null) _target = _waveController.GetNearestInRange(transform.position, _stats.Range);
            
            if (!_target) return;
            
            _elapsed += Time.fixedDeltaTime;
            if (_elapsed < 1f / _stats.AtkSpeed) return;
            _elapsed = 0;

            var o = Instantiate(bullet);
            var b = o.GetComponent<Bullet>();
            var dir = _target.transform.position - transform.position;
            b.Setup(_data.BulletSprite, _stats.Damage, _stats.ProjectileSpeed, transform.position, dir);
            
            Debug.DrawRay(transform.position, dir, Color.orange, 0.025f);
        }
        
        public void LevelUp()
        {
            if (_level >= _data.LevelCount) return;
            _level += 1;
            _stats = _data.GetStats(_level);
            srHead.sprite = _data.HeadSprites[_level];
        }
    }
}