using System.Collections.Generic;
using Source.Data.Towers;
using Source.Game.Entity;
using UnityEngine;

namespace Source.Game.Skills
{
    public abstract class Skill
    {
        public abstract void Cast(TowerSkillParams tsp, float perf, List<Enemy> enemies);
        
        protected List<Enemy> GetRandomEnemyCount(int min, int max, float perf, List<Enemy> es)
        {
            min = Mathf.Clamp(min, 0, es.Count);
            max = Mathf.Clamp(max, 0, es.Count);
            var lerp = Mathf.Lerp(min, max, perf);
            var roundLerp = Mathf.RoundToInt(lerp);
            var count = Mathf.Clamp(roundLerp, 0, es.Count);

            var all = new List<Enemy>(es);
            var selected = new List<Enemy>();

            for (var i = 0; i < count; i++)
            {
                var index = Random.Range(0, all.Count);
                selected.Add(all[index]);
                all.RemoveAt(index);
            }

            return selected;
        }
    }
}