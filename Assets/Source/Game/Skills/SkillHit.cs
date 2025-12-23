using System.Collections.Generic;
using NUnit.Framework;
using Source.Game.Entity;
using UnityEngine;

namespace Source.Game.Skills
{
    public class SkillHit : Skill
    {
        public override void Launch(float perf, Enemy mainTarget, params Enemy[] enemies)
        {
            var damage = Tsp.GetDamage(Level, perf);
            mainTarget.Hit(damage);
        }
    }
}