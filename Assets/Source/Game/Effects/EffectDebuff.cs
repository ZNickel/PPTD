using System.Collections.Generic;
using Source.Game.Entity;
using UnityEngine;

namespace Source.Game.Effects
{
    public class EffectDebuff : Effect
    {
        public override void Launch(List<Enemy> enemies)
        {
            IsOver = true;
        }

        public override void Tick(float t)
        {
            
        }
    }
}