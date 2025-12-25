using System.Collections.Generic;
using Source.Data.Towers;
using Source.Game.Entity;
using UnityEngine;

namespace Source.Game.Skills
{
    public class FreezSkill : Skill
    {
        public override void Cast(TowerSkillParams tsp, float perf, List<Enemy> es)
        {
            var power = tsp.Power(perf);
            var time = tsp.Time(perf);
            
            var (min, max) = tsp.Count();
            var selected = GetRandomEnemyCount(min, max, perf, es);
            foreach (var e in selected) e.AddDebuff(power, true, time);
        }
    }
}