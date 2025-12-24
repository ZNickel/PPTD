using Source.Data.Towers;
using Source.Game.Controllers;
using UnityEngine;

namespace Source.Game.Entity
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer srHead;
        [SerializeField] private GameObject bullet;

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

            var o = Instantiate(bullet);
            var b = o.GetComponent<Bullet>();
            var dir = _target.transform.position - transform.position;
            b.Setup(Data.BulletSprite, _stats.Damage, _stats.ProjectileSpeed, transform.position, dir);
            
            Debug.DrawRay(transform.position, dir, Color.orange, 0.025f);
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