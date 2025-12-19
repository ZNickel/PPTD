using System.Collections.Generic;
using UnityEngine;

namespace Source
{
    public class Way : MonoBehaviour
    {
        [SerializeField] private List<Vector3> points = new();
        
        public List<Vector3> Points => points; 
        
        public void Add(Vector3 p) => points.Add(p);
        public void AddAll(List<Vector3> ps) => points.AddRange(ps);
        public void Reverse() => points.Reverse();
        public void Clear() => points.Clear();

        #region Gizmos
        
        private void OnDrawGizmos() => Draw(Color.red);
        private void OnDrawGizmosSelected() => Draw(Color.darkOrange);

        private void Draw(Color color)
        {
            Gizmos.color = color;

            var size = .5f / points.Count;
            var n = 0;
            foreach (var point in points)
            {
                Gizmos.DrawWireSphere(point, .5f - n * size);
                n++;
            }
            
            if (points.Count < 2) return;
            for (var i = 0; i < points.Count - 1; i++) 
                Gizmos.DrawLine(points[i], points[i + 1]);
        }
        
        #endregion
    }
}