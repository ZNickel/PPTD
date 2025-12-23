using System.Collections.Generic;
using System.Linq;
using Source.Game.Entity;
using UnityEngine;

namespace Source.Game.Skills
{
    public class SkillSplash : Skill
    {
        public override void Launch(float perf, Enemy mainTarget, params Enemy[] enemies)
        {
            var damage = Tsp.GetDamage(Level, perf);
            var minFalloff = Tsp.GetMinFall(Level, perf);
            var maxFalloff = Tsp.GetMaxFall(Level, perf);
            var radius = Tsp.GetRadius(Level, perf);

            var inArea = FindAllInArea(radius, mainTarget, enemies);

            foreach (var (e, r) in inArea)
            {
                var dmg = Mathf.Lerp(minFalloff, maxFalloff, 1f - r) * damage;
                e.Hit(dmg);
            }
        }

        private List<(Enemy, float)> FindAllInArea(float radius, Enemy main, Enemy[] enemies)
        {
            var inArea = new List<(Enemy, float)> { (main, 1f) };

            var mPos = main.transform.position;

            foreach (var e in enemies)
            {
                var r = (e.transform.position - mPos).sqrMagnitude;
                if (r <= radius) inArea.Add((e, r / radius));
            }

            return inArea;
        }
    }
}