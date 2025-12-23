using Source.Data.Towers;
using Source.Game.Entity;
using UnityEngine;

namespace Source.Game.Skills
{
    public abstract class Skill : MonoBehaviour
    {
        protected TowerSkillParams Tsp;
        protected int Level;
        
        public abstract void Launch(float perf, Enemy mainTarget, params Enemy[] enemies);
        
        public void SetStats(int level, TowerSkillParams tsp)
        {
            Level = level;
            Tsp = tsp;
        }
    }
}