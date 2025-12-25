using System.Collections.Generic;
using Source.Game.Entity;
using UnityEngine;

namespace Source.Game.Effects
{
    public class EffectDebuff : Effect
    {
        private float _timeLimit;
        private Enemy _target;
        private float _totalTime;
        
        public override void Launch(List<Enemy> enemies)
        {
            if (enemies.Count == 0 || enemies[0] == null) return;
            _timeLimit = Tep.GetTime(Level);
            _target = enemies[0];
            _target.AddDebuff(Tep.GetDamage(Level));
            IsOver = false;
        }
        
        public override void Tick(float t)
        {
            _totalTime += t;

            if (!_target)
            {
                IsOver = true;
                return;
            }

            if (_totalTime > _timeLimit)
            {
                _target?.CancelDebuff();
                IsOver = true;
            }
        }
    }
}