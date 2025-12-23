using System.Collections.Generic;
using Source.Data.Towers;
using Source.Game.Entity;
using UnityEngine;

namespace Source.Game.Skills
{
    public class SkillDot : Skill
    {
        private class EnemyNTime
        {
            private readonly float _time, _damage;
            private readonly Enemy _enemy;
            private float _elapsed;

            public bool IsOver => _elapsed >= _time;
            
            public EnemyNTime(float time, float damage, Enemy enemy)
            {
                _elapsed = 0f;
                _damage = damage;
                _enemy = enemy;
                _time = time;
            }
            
            public void Hit(float t)
            {
                if (IsOver) return;
                _enemy.Hit(_damage);
                _elapsed += t;
            }
        }
        
        private readonly List<EnemyNTime> _enemies = new();

        public override void Launch(float perf, Enemy mainTarget, params Enemy[] enemies)
        {
            var t = Tsp.GetTime(Level, perf);
            var d = Tsp.GetDamage(Level, perf);
            
            if (mainTarget) 
                _enemies.Add(new EnemyNTime(t, d, mainTarget));
            foreach (var enemy in enemies)
                _enemies.Add(new EnemyNTime(t, d, enemy));
        }
        
        private void FixedUpdate()
        {
            var dTime = Time.fixedDeltaTime;
            foreach (var ent in _enemies) ent.Hit(dTime);
            _enemies.RemoveAll(ent => ent.IsOver);
        }
    }
}