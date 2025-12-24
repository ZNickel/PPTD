using System.Collections.Generic;
using Source.Game.Entity;
using UnityEngine;

namespace Source.Game.Effects
{
    public class EffectDot : Effect
    {
        private float _interval;
        private float _timeLimit;
        private float _dmg;
        private Enemy _target;
        
        private float _intervalElapsed;
        private float _totalTime;
        
        public override void Launch(List<Enemy> enemies)
        {
            if (enemies.Count == 0) return;
            if (enemies[0] == null) return;
            _interval = Tep.GetInterval(Level);
            _timeLimit = Tep.GetTime(Level);
            _dmg = Tep.GetDamage(Level);
            _target = enemies[0];
            IsOver = false;
        }
        
        public override void Tick(float t)
        {
            _totalTime += t;
            _intervalElapsed += t;

            if (_intervalElapsed >= _interval)
            {
                _intervalElapsed = 0f;
                _target?.Hit(_dmg);
            }
            
            IsOver = !_target || _totalTime > _timeLimit;
        }
    }
}