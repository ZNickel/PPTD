using System.Collections.Generic;
using Source.Data.Towers;
using Source.Event;
using Source.Game.Controllers;
using Source.Game.Effects;
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
        private TowerEffectBuilder _effectBuilder;
        private TowerStats _stats;

        private List<Enemy> _targets;
        private float _elapsed;

        private bool _everyShotUpdate;

        private float _boost;
        
        public void Setup(WaveController wc, TowerData d)
        {
            _effectBuilder = GetComponent<TowerEffectBuilder>();
            _waveController = wc;
            Data = d;
            srHead.sprite = d.HeadSprites[0];
            _stats = Data.GetStats(0);
            _effectBuilder.Setup(_waveController);
            _targets = new List<Enemy>();
            _everyShotUpdate = EveryShotUpdate();
            UIEventBus.Instance.EventClickPowerChanged.AddListener(boost => _boost = boost);
        }

        private void FixedUpdate()
        {
            _elapsed += Time.fixedDeltaTime;
            if (_elapsed < 1f / Data.GetBoostedAtkSpeed(_stats.AtkSpeed, _boost)) return;
            _elapsed = 0;
            
            TryUpdateTarget();
            if (_targets.Count == 0) return;
            CreateBullet();
        }

        private void CreateBullet()
        {
            var effect = Effect.CreateEffect(Data.Effects[0].EffectType, Data.Effects[0], Level);
            Instantiate(Data.BulletPrefab)
                .GetComponent<UniBullet>()
                .Setup(
                    _targets[0], 
                    transform.position, 
                    _targets[0].transform.position, 
                    _stats.ProjectileSpeed,
                    Data.GetBoostedDamage(_stats.Damage, _boost),
                    () => _effectBuilder.Launch(effect, _targets)
                );
        }
        
        public void LevelUp()
        {
            if (Level >= Data.LevelCount) return;
            Level += 1;
            _stats = Data.GetStats(Level);
            srHead.sprite = Data.HeadSprites[Level];
        }

        private void TryUpdateTarget()
        {
            if (!_everyShotUpdate)
            {
                _targets.RemoveAll(e => e == null || Vector3.Distance(e.transform.position, transform.position) > _stats.Range);
                if (_targets.Count > 0) return;
            }
            
            _targets = _waveController.Selector.Get(
                transform.position, 
                Data.GetBoostedRange(_stats.Range, _boost), 
                0, 
                0, 
                Data.TargetSelectBehaviour
            );
        }

        private bool EveryShotUpdate()
        {
            return Data.TargetSelectBehaviour switch
            {
                TargetSelectBehaviour.Nearest => true,
                TargetSelectBehaviour.RandomInRange => true,
                TargetSelectBehaviour.MostPowerFull => true,
                _ => false
            };
        }
    }
}