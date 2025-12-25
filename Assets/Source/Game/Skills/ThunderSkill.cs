using System.Collections.Generic;
using Source.Data.Towers;
using Source.Game.Entity;

namespace Source.Game.Skills
{
    public class ThunderSkill : Skill
    {
        public override void Cast(TowerSkillParams tsp, float perf, List<Enemy> es)
        {
            var power = tsp.Power(perf);

            var (min, max) = tsp.Count();
            var selected = GetRandomEnemyCount(min, max, perf, es);

            var count = selected.Count;
            for (var i = 0; i < selected.Count; i++)
            {
                selected[i].Hit(power * (i / (float)count * .75f + .25f));
            }
        }
    }
}