using System;
using Source.Data;
using UnityEngine;

namespace Source
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private CellType type;
        [SerializeField] private TileMap mapping;

        public CellType Type => type;

        public void ForceInit(CellType t, TileMap map)
        {
            mapping = map;
            type = t;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            var sr = GetComponent<SpriteRenderer>();
            if (!sr) return;
            if (!mapping) return;
            sr.sprite = mapping[type];
            sr.color = new Color(1, 1, 1, 1);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 0, 0, .25f);
            Gizmos.DrawLine(
                transform.position + new Vector3(-0.33f, -0.33f, 0),
                transform.position + new Vector3(+0.33f, +0.33f, 0)
            );
            Gizmos.DrawLine(
                transform.position + new Vector3(-0.33f, +0.33f, 0),
                transform.position + new Vector3(+0.33f, -0.33f, 0)
            );
        }
#endif
    }
}