using System.Collections.Generic;
using Source.Data;
using Source.Data.Way;
using UnityEngine;

namespace Source
{
    
    [CreateAssetMenu(fileName = "LevelData", menuName = "Level/Level Data")]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private TileMap editorTileMap;
        [SerializeField] private TileMap tileMap;
        [SerializeField] private int width = 5;
        [SerializeField] private int height = 5;
        [SerializeField] private int startCoins;
        [SerializeField] private int startPower = 0;
        [SerializeField] private int startHealth = 100;
        [SerializeField] private CellType[] types;
        [SerializeField] private WayPackData ways;
        [SerializeField] private WaveData[] waveData;
        
        public int StartCoins => startCoins;
        public int StartPower => startPower;
        public int StartHealth => startHealth;
        public CellType[] Types => types;
        public int Width => width;
        public int Height => height;
        
        public TileMap Mapping => tileMap;
        public TileMap EditorMapping => tileMap;
        public bool IsEmpty => types == null || types.Length == 0;
        
        public void Init(int w, int h)
        {
            width = w;
            height = h;
            types = new CellType[width * height];
            ways = new WayPackData();
        }

        public Sprite GetSprite(int x, int y) => tileMap[types[y * width + x]];
        public Sprite GetEditorSprite(int x, int y) => editorTileMap[types[y * width + x]];

        public CellType GetCellType(int x, int y)
        {
            var ind = y * width + x;
            return ind < types.Length ? types[ind] : CellType.None;
        }

        public WayPackData Ways => ways;
            
        public void SetCellType(int x, int y, CellType type) => types[y * width + x] = type;

        public void SetWays(WayPackData d) => ways = d;

        public WaveData[] Waves => waveData;
    }
}