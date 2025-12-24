using System.Collections.Generic;
using Source.Data.Towers;
using Source.Game.Entity;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Game.Controllers
{
    public class EnemySelector
    {
        private readonly List<Enemy>[] _alive;
        public EnemySelector(List<Enemy>[] alive) => _alive = alive;

        public List<Enemy> Get(Vector3 pos, float range, float subRange, int n, TargetSelectBehaviour beh)
        {
            return beh switch
            {
                TargetSelectBehaviour.Nearest => NearestInRange(pos, range),
                TargetSelectBehaviour.NearestAndLock => NearestInRange(pos, range),
                TargetSelectBehaviour.RandomInRange => RandomInRange(pos, range),
                TargetSelectBehaviour.MostPowerFull => MostPowerFull(pos, range),
                TargetSelectBehaviour.AllAroundNearest => AllAroundNearest(pos, range, subRange, n),
                TargetSelectBehaviour.AllInRange => AllInRange(pos, range),
                TargetSelectBehaviour.ChainOfNearest => ChainOfNearest(pos, range, subRange, n),
                _ => new List<Enemy>()
            };
        }

        private List<Enemy> NearestInRange(Vector3 pos, float range)
        {
            Enemy res = null;
            var lastD = range;
            foreach (var l in _alive)
            foreach (var e in l)
            {
                if (e == null) continue;
                
                var d = Vector2.Distance(e.transform.position, pos);
                if (d >= lastD) continue;
                lastD = d;
                res = e;
            }
            return res == null ? new List<Enemy>() : new List<Enemy> {res};
        }

        private List<Enemy> RandomInRange(Vector3 pos, float range)
        {
            var list = AllInRange(pos, range);
            return list.Count == 0 ? new List<Enemy>() : new List<Enemy> {list[Random.Range(0, list.Count)]};
        }
        
        private List<Enemy> MostPowerFull(Vector3 pos, float range)
        {
            var hp = float.MinValue;
            Enemy res = null;
            
            foreach (var l in _alive)
            foreach (var e in l)
            {
                if (e == null) continue;
                if (Vector2.Distance(e.transform.position, pos) > range) continue;
                if (e.Data.Hp > hp) res = e;
            }

            return res == null ? new List<Enemy>() : new List<Enemy> {res};
        }

        private List<Enemy> AllAroundNearest(Vector3 pos, float range, float subRange, int n)
        {
            var nearest = NearestInRange(pos, range);
            if (nearest.Count == 0) return nearest;

            var ePos = nearest[0].transform.position;
            
            foreach (var l in _alive)
            foreach (var e in l)
            {
                if (e == null || e == nearest[0]) continue;
                var d = Vector2.Distance(e.transform.position, ePos);
                if (d <= subRange) nearest.Add(e);
            }
            
            return nearest;
        }
        
        private List<Enemy> ChainOfNearest(Vector3 pos, float range, float subRange, int n)
        {
            var result = new List<Enemy>();
            var used = new HashSet<Enemy>();

            var current = FindClosestEnemyExcluded(pos, used);
            if (current == null)
                return result;

            result.Add(current);
            used.Add(current);

            while (result.Count < n)
            {
                var next = FindClosestEnemyExcluded(current.transform.position, used);
                if (next == null)
                    break;

                result.Add(next);
                used.Add(next);
                current = next;
            }

            return result;
        }
        
        private Enemy FindClosestEnemyExcluded(Vector3 pos, HashSet<Enemy> excluded)
        {
            Enemy closest = null;
            var minSqrDist = float.MaxValue;

            foreach (var list in _alive)
            foreach (var enemy in list)
            {
                if (enemy == null || excluded.Contains(enemy)) continue;
                var sqrDist = (enemy.transform.position - pos).sqrMagnitude;
                if (!(sqrDist < minSqrDist)) continue;
                minSqrDist = sqrDist;
                closest = enemy;
            }

            return closest;
        }
        
        private List<Enemy> AllInRange(Vector3 pos, float range)
        {
            var list = new List<Enemy>();
            
            foreach (var l in _alive)
            foreach (var e in l)
            {
                if (e == null) continue;
                var d = Vector2.Distance(e.transform.position, pos);
                if (d <= range) list.Add(e);
            }

            return list;
        }
    }
}