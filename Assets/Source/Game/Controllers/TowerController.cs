using System;
using System.Collections.Generic;
using System.Text;
using Source.Data;
using Source.Event;
using Source.Game.Entity;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Game.Controllers
{
    public class TowerController : MonoBehaviour
    {
        private enum Mode
        {
            Select,
            Place,
        }

        private enum CellStatus
        {
            Na,
            Empty,
            Tower,
        }
        
        [SerializeField] private Transform towerContainer;
        [SerializeField] private GameObject towerPrefab;
        
        private Mode TcMode { get; set; }

        private Camera _camera;
        private GameController _gc;
        
        private TowerData _placeTowerData;

        private Vector2 _qaShift; 
        private (CellStatus status, Tower tower)[,] _grid;
        private Dictionary<int, Dictionary<int, (CellStatus status, Tower tower)>> _quickAccessTable;
        
        private GameObject _ghost;
        private SpriteRenderer _ghostBase, _ghostHead;

        private void Awake() => _gc = GetComponent<GameController>();

        public void Setup(CellType[] fullGrid, int w, int h)
        {
            ExtractTowerRect(fullGrid, w, h);
        }

        private void Start()
        {
            _camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            UIEventBus.Instance.EventSelectShopItem.AddListener(ShopItemSelected);
            CreateGhost();
            ValidateGhost();
        }

        public bool ShowenMenu;
        
        private void ShopItemSelected(TowerData data)
        {
            _placeTowerData = data;
            TcMode = data == null ? Mode.Select : Mode.Place;
            ValidateGhost();
        }

        private void Update()
        {
            if (ShowenMenu) return;
            var mouseScreen = Mouse.current.position.ReadValue();
            var mwp = new Vector3(mouseScreen.x, mouseScreen.y, _camera.transform.position.z);
            var worldPos = _camera.ScreenToWorldPoint(mwp);

            var x = Mathf.RoundToInt(worldPos.x);
            var y = Mathf.RoundToInt(worldPos.y);

            var inBounds = _quickAccessTable.ContainsKey(x) && _quickAccessTable[x].ContainsKey(y);
            
            if (TcMode == Mode.Select)
            {
                var t = !inBounds || _quickAccessTable[x][y].status != CellStatus.Tower ? null : _quickAccessTable[x][y].tower;
                var screenPos = _camera.WorldToScreenPoint(new Vector3(x, y, 0));

                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    UIEventBus.Instance.Trigger_EventShowTowerPopup(this, screenPos.x, screenPos.y, t);
                }
                
                return;
            }
            
            if (!inBounds) return;
            
            _ghost.transform.position = _quickAccessTable[x][y].status == CellStatus.Empty 
                ? new Vector3(x, y, -1f) 
                : new Vector3(-1000, -1000, -1f);
            if (Mouse.current.leftButton.wasPressedThisFrame) CreateTower(_placeTowerData, x, y);
        }

        public void LevelUp(Tower t)
        {
            var cost = t.Data.LvlUpPrice(t.Level + 1);
            if (!_gc.CResource.ChangeCoin(-cost)) return;
            t.LevelUp();
        }

        public void Sell(Tower t)
        {
            
        }

        public void Skill(Tower t)
        {
            
        }

        private void CreateGhost()
        {
            _ghost = new GameObject("_tower_placer_ghost_");
            _ghostBase = CreateGhostSprite(_ghost, "_base_", 0f);
            _ghostHead = CreateGhostSprite(_ghost, "_head_", -.5f);
        }

        private static SpriteRenderer CreateGhostSprite(GameObject parent, string n, float z)
        {
            var o = new GameObject(n);
            o.transform.SetParent(parent.transform);
            o.transform.localPosition = new Vector3(0, 0, z);
            var sr = o.AddComponent<SpriteRenderer>();
            sr.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            return sr;
        }

        private void ValidateGhost()
        {
            if (_placeTowerData == null)
            {
                _ghost.SetActive(false);
            }
            else
            {
                _ghost.SetActive(true);
                _ghostHead.sprite = _placeTowerData.HeadSprites[0];
            }
        }
        
        private void ExtractTowerRect(CellType[] fullGrid, int w, int h)
        {
            var rectGrid = new CellType[w, h];
            for (var n = 0; n < fullGrid.Length; n++)
                rectGrid[n % w, n / w] = fullGrid[n];

            int minX, maxX, minY, maxY;
            minX = minY = int.MaxValue;
            maxX = maxY = int.MinValue;

            for (var x = 0; x < w; x++)
            for (var y = 0; y < h; y++)
            {
                if (rectGrid[x, y] != CellType.TowerPlace) continue;
                
                minX = Mathf.Min(minX, x);
                minY = Mathf.Min(minY, y);
                maxX = Mathf.Max(maxX, x);
                maxY = Mathf.Max(maxY, y);
            }

            var rW = (maxX - minX) + 1;
            var rH = (maxY - minY) + 1;
            
            _grid = new (CellStatus status, Tower tower)[rW, rH];
            _quickAccessTable = new Dictionary<int, Dictionary<int, (CellStatus status, Tower tower)>>();

            _qaShift = new Vector2(minX, minY);
            
            for (var x = 0; x < rW; x++)
            for (var y = 0; y < rH; y++)
            {
                var xQ = x + minX;
                var yQ = y + minY;
                
                var status = rectGrid[xQ, yQ] == CellType.TowerPlace ? CellStatus.Empty : CellStatus.Na;
                
                _grid[x, y] = (status, null);
                
                if (!_quickAccessTable.ContainsKey(xQ))
                    _quickAccessTable[xQ] = new Dictionary<int, (CellStatus status, Tower tower)>();

                if (!_quickAccessTable[xQ].ContainsKey(yQ))
                    _quickAccessTable[xQ][yQ] = _grid[x, y];
            }

        }

        private void CreateTower(TowerData data, int x, int y)
        {
            if (_quickAccessTable[x][y].status != CellStatus.Empty) return;

            if (!_gc.CResource.ChangeCoin(-data.Price)) return;
            
            var tower = Instantiate(towerPrefab, towerContainer).GetComponent<Tower>();
            tower.Setup(_gc.CWave, this, data);
            tower.transform.position = new Vector3(x, y, -.5f);

            _quickAccessTable[x][y] = (CellStatus.Tower, tower);
            _grid[(int)(x - _qaShift.x), (int)(y - _qaShift.y)] = (CellStatus.Tower, tower);

        }
    }
}