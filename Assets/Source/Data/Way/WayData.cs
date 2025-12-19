using System.Collections.Generic;
using UnityEngine;

namespace Source.Data.Way
{
    [System.Serializable]
    public class WayData
    {
        public List<Vector3> points = new();
        private float _totalDistance = -1f;

        public float TotalDistance => _totalDistance > 0
            ? _totalDistance
            : _totalDistance = CalcTotalDistance();
        
        public int PointCount => points.Count;

        public Vector3 this[int index] => points[index];

        private float CalcTotalDistance()
        {
            var d = 0f;
            for (var i = 0; i < points.Count - 1; i++)
                d += Vector3.Distance(points[i], points[i + 1]);
            return d;
        }
    }
}