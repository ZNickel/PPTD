using System.Collections.Generic;
using System.Linq;
using Source.Data.Towers;
using Source.Game.Entity;
using UnityEngine;

namespace Source.Game.Skills
{
    public class MeteorRainSkill : Skill
    {
        public override void Cast(TowerSkillParams tsp, float perf, List<Enemy> es)
        {
            var power = tsp.Power(perf);
            var time = tsp.Time(perf);
            
            var (min, max) = tsp.Count();
            var selected = GetRandomEnemyCount(min, max, perf, es).Select(e => e.transform.position).ToList();

            var touched = new List<(Enemy, float)>();

            var absoluteMax = float.MaxValue;
            foreach (var e in es)
            {
                var minD = float.MaxValue;
                foreach (var pos in selected)
                {
                    var d = Vector3.Distance(pos, e.transform.position);
                    if (d > time) continue;
                    
                    touched.Add((e, d));
                    absoluteMax = Mathf.Max(absoluteMax, d);
                }
            }

            foreach (var (e, d) in touched) 
                e.Hit(power * (1f - d / absoluteMax) * .9f + .1f);
        }
    }
}