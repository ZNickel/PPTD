using System.Collections.Generic;
using UnityEngine;

namespace Source.Data.Way
{
    [System.Serializable]
    public class WayPackData
    {
        public List<WayData> ways = new ();

        public WayData this[int index] => ways[index];
        
        public int Count => ways.Count;

        public Vector3 GetIndicatorPosition() => ways[Random.Range(0, Count)].GetWaveIndicatorPosition();
    }
}