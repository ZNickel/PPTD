using System;
using System.Collections.Generic;
using System.Linq;
using Source.Data;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace Source
{
    [CreateAssetMenu(fileName = "SpriteMap", menuName = "Sprite map")]
    public class TileMap : ScriptableObject
    {
        [SerializeField] private Sprite towerMarker;
        [SerializeField] private Sprite[] rawBlends;

        [SerializeField] private Sprite none;
        [SerializeField] private Sprite road;
        [SerializeField] private Sprite grass;
        [SerializeField] private Sprite towerPlace;
        
        public Sprite GetSprite(BlendSpriteType type, CellType a, CellType b = CellType.None,
            CellType c = CellType.None, CellType d = CellType.None)
        {
            string sName;
            
            if (type is BlendSpriteType.H or BlendSpriteType.V && a == b)
            {
                sName = $"Tiles_M_{Ctl(a)}";
            }
            else if (type is BlendSpriteType.C && a == b && c == d)
            {
                sName = $"Tiles_V_{Ctl(a)}{Ctl(c)}";
            }
            else if (type is BlendSpriteType.C && a == c && b == d)
            {
                sName = $"Tiles_H_{Ctl(a)}{Ctl(b)}";
            }
            else if (type is BlendSpriteType.C && a == b && c == d && a == c)
            {
                sName = $"Tiles_M_{Ctl(a)}";
            }
            else
            {
                sName = $"Tiles_{Bst(type)}_{Ctl(a)}{Ctl(b)}{Ctl(c)}{Ctl(d)}";
            }

            var fod = rawBlends.FirstOrDefault(sprite => sprite != null && sprite.name == sName);
            if (fod == null) Debug.Log($"{sName} [null]");
            return fod;
        }

        private static string Ctl(CellType type)
        {
            return type switch
            {
                CellType.Grass => "G",
                CellType.Road => "R",
                CellType.TowerPlace => "T",
                _ => ""
            };
        }
        
        private static string Bst(BlendSpriteType type)
        {
            return type switch
            {
                BlendSpriteType.M => "M",
                BlendSpriteType.H => "H",
                BlendSpriteType.V => "V",
                BlendSpriteType.C => "C",
                _ => ""
            };
        }

        public Sprite TowerMarker => towerMarker;
        
        public Sprite this[CellType cellType]
        {
            get
            {
                return cellType switch
                {
                    CellType.Road => road,
                    CellType.Grass => grass,
                    CellType.TowerPlace => towerPlace,
                    _ => none
                };
            }
        }
    }
}