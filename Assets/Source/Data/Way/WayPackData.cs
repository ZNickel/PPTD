using System.Collections.Generic;

namespace Source.Data.Way
{
    [System.Serializable]
    public class WayPackData
    {
        public List<WayData> ways = new ();

        public WayData this[int index] => ways[index];
        
        public int Count => ways.Count;
    }
}