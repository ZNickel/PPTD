using System.Collections.Generic;
using System.Linq;
using Source.Game.Entity;
using UnityEngine;

namespace Source.Game.Effects
{
    public class EffectSplash : Effect
    {
        public override void Launch(List<Enemy> enemies)
        {
            var damage = Tep.GetDamage(Level);
            var minFalloff = Tep.GetMinFall(Level);
            var maxFalloff = Tep.GetMaxFall(Level);

            var median = FindMedian(enemies);
            var inArea = AssignWeights(enemies, median);
            
            foreach (var (e, w) in inArea) 
                e.Hit(Mathf.Lerp(minFalloff, maxFalloff, w) * damage);

            IsOver = true;
        }

        public override void Tick(float t)
        {
            
        }

        private Vector3 FindMedian(List<Enemy> enemies)
        {
            var mid = enemies
                .Aggregate(Vector3.zero, (current, e) => current + e.transform.position);
            return mid / enemies.Count;
        }

        private List<(Enemy, float)> AssignWeights(List<Enemy> enemies, Vector3 median)
        {
            var weights = enemies
                .Select(e => (e, Vector3.Distance(e.transform.position, median)))
                .ToList();

            var maxD = float.MinValue;
            foreach (var (e, w) in weights)
                maxD = Mathf.Max(maxD, w);

            for (var i = 0; i < weights.Count; i++) 
                weights[i] = (weights[i].e, Mathf.Clamp(1f - weights[i].Item2 / maxD, 0f, 1f));

            return weights;
        }
    }
}