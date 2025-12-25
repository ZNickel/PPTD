using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.UI.Map
{
    public class MapController : MonoBehaviour
    {
        [SerializeField] private MapMarker[] markers;
        
        public List<Vector2> GetPositions()
        {
            var cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            var pos = markers
                .Select(m => cam.WorldToScreenPoint(m.transform.position)).Select(dummy => (Vector2)dummy)
                .ToList();

            foreach (var m in markers) Destroy(m);

            return pos;
        }
    }
}