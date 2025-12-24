using System.Collections.Generic;
using Source.Data.Towers;
using UnityEngine;

namespace Source.Game.Skills
{
    public class SkillLauncher : MonoBehaviour
    {
        private readonly Dictionary<TowerSkillType, Skill> _skillsOfType = new()
        {
            [TowerSkillType.Chain] = new SkillChain(),
            [TowerSkillType.Debuff] = new SkillDebuff(),
            [TowerSkillType.Dot] = new SkillDot(),
            [TowerSkillType.Splash] = new SkillSplash(),
            [TowerSkillType.Hit] = new SkillHit(),
        };
        
        
    }
}