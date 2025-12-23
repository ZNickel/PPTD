using System.Collections.Generic;
using NUnit.Framework;
using Source.Game.Entity;
using UnityEngine;

namespace Source.Game.Skills
{
    public class SkillChain : Skill
    {
        public override void Launch(float perf, Enemy mainTarget, params Enemy[] enemies)
        {
            var n = Mathf.RoundToInt(Tsp.GetCount(Level, perf));
            var damage = Tsp.GetDamage(Level, perf);
            var minFalloff = Tsp.GetMinFall(Level, perf);
            var maxFalloff = Tsp.GetMaxFall(Level, perf);

            var chain = FindChain(n, mainTarget, enemies);

            for (var i = 0; i < chain.Count; i++)
            {
                var dmg = Mathf.Lerp(minFalloff, maxFalloff, 1f - i / (n - 1f)) * damage;
                chain[i].Hit(dmg);
            }
        }

        private static List<Enemy> FindChain(int n, Enemy mainTarget, Enemy[] enemies)
        {
            var chain = new List<Enemy> { mainTarget };

            var lastTarget = mainTarget;
            for (var i = 1; i < n; i++)
            {
                var minD = float.MaxValue;
                Enemy target = null;
                foreach (var enemy in enemies)
                {
                    if (chain.Contains(enemy)) continue;
                    var d = (enemy.transform.position - lastTarget.transform.position).magnitude;
                    if (!(d < minD)) continue;
                    minD = d;
                    target = enemy;
                }

                if (Mathf.Approximately(minD, float.MaxValue)) break;

                chain.Add(target);
                lastTarget = target;
            }

            return chain;
        }
    }
}