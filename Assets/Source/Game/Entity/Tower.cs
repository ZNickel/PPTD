using Source.Data.Towers;
using Source.Game.Controllers;
using Source.Game.Entity.Bullets;
using UnityEngine;

namespace Source.Game.Entity
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer srHead;

        public TowerData Data { get; private set; }

        public int Level { get; private set; }

        private WaveController _waveController;
        private TowerStats _stats;

        private Enemy _target;
        private float _elapsed;
        
        public void Setup(WaveController wc, TowerData d)
        {
            _waveController = wc;
            Data = d;
            srHead.sprite = d.HeadSprites[0];
            _stats = Data.GetStats(0);
        }

        private void FixedUpdate()
        {
            if (_target == null) _target = _waveController.GetNearestInRange(transform.position, _stats.Range);
            
            if (!_target) return;
            
            _elapsed += Time.fixedDeltaTime;
            if (_elapsed < 1f / _stats.AtkSpeed) return;
            _elapsed = 0;
            
            CreateBullet();
        }

        private void CreateBullet()
        {
            Instantiate(Data.BulletPrefab)
                .GetComponent<UniBullet>()
                .Setup(_target, transform.position, _target.transform.position, _stats.ProjectileSpeed, _stats.Damage);
        }
        
        public void LevelUp()
        {
            if (Level >= Data.LevelCount) return;
            Level += 1;
            _stats = Data.GetStats(Level);
            srHead.sprite = Data.HeadSprites[Level];
        }
    }
}