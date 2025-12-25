using System.Collections.Generic;
using System.Text;
using Source.Data;
using Source.Data.Way;
using UnityEngine;

namespace Source
{
    public class LevelGrid : MonoBehaviour
    {
        [Header("Struct")] 
        [SerializeField] private GameObject cellContainer;
        [SerializeField] private GameObject wayContainer;

        [Header("Setup")] 
        [SerializeField] private LevelData levelData;

        [SerializeField] private LevelSelectionContainer selectedLevel;

        [Header("!Initial only")] [SerializeField]
        private int cellCountWidth;

        [SerializeField] private int cellCountHeight;

        public LevelData CurrentLevelData => levelData;

        private void Awake()
        {
            if (selectedLevel != null && selectedLevel.lvlData != null) 
                levelData = selectedLevel.lvlData;
        }

        private void Start()
        {
            GenerateInGameTiles();
        }

        private void GenerateInGameTiles()
        {
            const float shift = .5f;

            for (var i = cellContainer.transform.childCount - 1; i >= 0; i--)
                DestroyImmediate(cellContainer.transform.GetChild(i).gameObject);

            for (var y = 0; y < cellCountHeight; y++)
            for (var x = 0; x < cellCountWidth; x++)
            {
                var nX = x + 1 >= cellCountWidth ? x : x + 1;
                var nY = y + 1 >= cellCountHeight ? y : y + 1;
                
                var ld = levelData.GetCellType(x, y);
                var rd = levelData.GetCellType(nX, y);
                var lu = levelData.GetCellType(x, nY);
                var ru = levelData.GetCellType(nX, nY);
                
                var m = levelData.Mapping.GetSprite(BlendSpriteType.M, ld);
                if (m != null) CreateSprite(x, y, m);
                var h = levelData.Mapping.GetSprite(BlendSpriteType.H, ld, rd);
                if (h != null) CreateSprite(x + shift, y, h);
                var v = levelData.Mapping.GetSprite(BlendSpriteType.V, lu, ld);
                if (v != null) CreateSprite(x, y + shift, v);
                var c = levelData.Mapping.GetSprite(BlendSpriteType.C, lu, ru, ld, rd);
                if (c != null) CreateSprite(x + shift, y + shift, c);

                if (ld == CellType.TowerPlace) 
                    CreateSprite(x, y, levelData.Mapping.TowerMarker, -.025f);
            }
        }

        private void CreateSprite(float x, float y, Sprite sprite, float dz = 0f)
        {
            var o = new GameObject($"SubCell [{x}:{y}]");
            o.transform.SetParent(cellContainer.transform);
            o.transform.localScale = new Vector3(1, 1, 1);
            o.transform.position = new Vector3(x, y, dz);
            var sr = o.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
        }

        private void BuildCellGrid()
        {
            for (var i = cellContainer.transform.childCount - 1; i >= 0; i--)
                DestroyImmediate(cellContainer.transform.GetChild(i).gameObject);
            for (var i = wayContainer.transform.childCount - 1; i >= 0; i--)
                DestroyImmediate(wayContainer.transform.GetChild(i).gameObject);

            for (var y = 0; y < cellCountHeight; y++)
            for (var x = 0; x < cellCountWidth; x++)
            {
                var o = new GameObject($"Cell [{x}:{y}]");
                o.transform.SetParent(cellContainer.transform);
                o.transform.localScale = new Vector3(1, 1, 1);
                o.transform.position = new Vector3(x, y, 0);

                var cell = o.AddComponent<Cell>();
                cell.ForceInit(levelData.GetCellType(x, y), levelData.EditorMapping);

                var sr = o.AddComponent<SpriteRenderer>();
                sr.sprite = levelData.GetEditorSprite(x, y);
            }

            if (levelData.Ways == null) return;
            var wayIndex = 0;
            foreach (var wd in levelData.Ways.ways)
            {
                if (wd == null) return;
                var o = new GameObject($"Way [{wayIndex}]");
                o.transform.SetParent(wayContainer.transform);
                var w = o.AddComponent<Way>();
                w.AddAll(wd.points);
                wayIndex++;
            }
        }

        public void CreateGrid()
        {
            var errors = new List<string>();
            if (!levelData) errors.Add("LevelGrid > Create | level data: None");
            if (cellCountWidth <= 0) errors.Add("LevelGrid > Create | width <= 0");
            if (cellCountHeight <= 0) errors.Add("LevelGrid > Create | height <= 0");
            if (errors.Count > 0)
            {
                Debug.LogError($"ERROR IN <{name}>:\n" + new StringBuilder().AppendJoin("\n", errors));
                return;
            }

            levelData.Init(cellCountWidth, cellCountHeight);
            BuildCellGrid();
        }

        public void LoadGrid()
        {
            if (!levelData || levelData.IsEmpty)
            {
                Debug.LogError($"ERROR IN <{name}>:\nLevelGrid > Load | level data: None or empty");
                return;
            }

            BuildCellGrid();
        }

        public void SaveGrid()
        {
            levelData.Init(cellCountWidth, cellCountHeight);

            foreach (var cell in transform.GetComponentsInChildren<Cell>())
            {
                levelData.SetCellType(
                    Mathf.RoundToInt(cell.transform.localPosition.x),
                    Mathf.RoundToInt(cell.transform.localPosition.y),
                    cell.Type
                );
            }

            var wpd = new WayPackData();
            foreach (var way in wayContainer.GetComponentsInChildren<Way>())
            {
                var w = new WayData
                {
                    points = way.Points
                };
                wpd.ways.Add(w);
            }

            levelData.SetWays(wpd);
        }

        public (float minX, float maxX, float minY, float maxY) GetBounds()
        {
            var maxX = float.MinValue;
            var maxY = float.MinValue;

            foreach (var t in GetComponentsInChildren<Transform>())
            {
                var pos = t.localPosition;
                if (pos.x > maxX) maxX = pos.x;
                if (pos.y > maxY) maxY = pos.y;
            }

            return (0, maxX, 0, maxY);
        }
    }
}