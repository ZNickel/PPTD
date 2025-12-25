using System.Collections.Generic;
using Source.Game.Entity;
using UnityEngine;

namespace Source.Game.Effects
{
    public class EffectChain : Effect
    {
        public override void Launch(List<Enemy> enemies)
        {
            var damage = Tep.GetDamage(Level);
            
            for (var i = 0; i < enemies.Count; i++)
            {
                enemies[i].Hit(damage);
            }
            
            IsOver = true;
        }

        public override void Tick(float t)
        {
            
        }
    }
}