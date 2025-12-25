using System.Collections.Generic;
using Source.Data.Towers;
using Source.Game.Entity;

namespace Source.Game.Skills
{
    public class EmptySkill : Skill
    {
        public override void Cast(TowerSkillParams tsp, float perf, List<Enemy> es)
        {
            
        }
    }
}