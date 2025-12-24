using System.Collections.Generic;
using Source.Game.Entity;
using UnityEngine;

namespace Source.Game.Effects
{
    public class EffectChain : Effect
    {
        public override void Launch(List<Enemy> enemies)
        {
            var n = Mathf.RoundToInt(Tep.GetCount(Level));
            var damage = Tep.GetDamage(Level);
            var minFalloff = Tep.GetMinFall(Level);
            var maxFalloff = Tep.GetMaxFall(Level);

            for (var i = 0; i < enemies.Count; i++)
            {
                var dmg = Mathf.Lerp(minFalloff, maxFalloff, 1f - i / (n - 1f)) * damage;
                enemies[i].Hit(dmg);
            }
            
            IsOver = true;
        }

        public override void Tick(float t)
        {
            
        }
    }
}