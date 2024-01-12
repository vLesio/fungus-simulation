using System;
using System.Collections;
using System.Collections.Generic;
using CoinPackage.Debugging;
using Fungus;
using Settings;
using UnityEngine;
using Utils.Singleton;

namespace GridSystem {
    public class CGrid : Singleton<CGrid> {
        [SerializeField] private GameObject cellPrefab;
        
        private SpriteRenderer _renderer;
        private Dictionary<Vector2Int, Cell> _cells = new();
        private AppSettingsDefinition _settings = DevSettings.Instance.appSettings;
        // Start is called before the first frame update
        protected override void Awake() {
            base.Awake();
            _renderer = GetComponent<SpriteRenderer>();
            InitializeCells();
        }

        private void InitializeCells() {
            var cellWidth = 1f / _settings.gridSize.x;
            var cellHeight = 1f / _settings.gridSize.y;
            var cellShift = new Vector3((float)-Math.Floor(_settings.gridSize.x/2) * cellWidth, 
                                        (float)-Math.Floor(_settings.gridSize.y/2) * cellHeight);

            if (_settings.gridSize.x % 2 == 0) {
                cellShift += new Vector3(cellWidth/2f, 0f);
            }
            if (_settings.gridSize.y % 2 == 0) {
                cellShift += new Vector3(0f, cellHeight/2f);
            }

            for (var i = 0; i < _settings.gridSize.x; i++) {
                for (var j = 0; j < _settings.gridSize.y; j++) {
                    var cellPosition = new Vector3(cellWidth * i, cellHeight * j) + cellShift;
                    var obj = Instantiate(cellPrefab, transform);
                    obj.transform.localScale = new Vector3(cellWidth, cellHeight, 1f);
                    obj.transform.localPosition = cellPosition;
                    var cell = obj.GetComponent<Cell>();
                    cell.Cords = new Vector2Int(i, j);
                    cell.SetCell(CellType.Dirt);
                    _cells.Add(new Vector2Int(i, j), cell);
                }
            }
        }

        public void ClearGrid() {
            foreach (var cell in _cells.Values) {
                cell.SetFood(0f);
                cell.SetCell(CellType.Dirt);
            }
        }

        public void SetFood(Vector2Int cellPos, float food) {
            if (_cells.TryGetValue(cellPos, out var cell)) {
                cell.SetFood(food);
            }
        }
        
        public void AddFood(Vector2Int cellPos, float food) {
            if (_cells.TryGetValue(cellPos, out var cell)) {
                cell.AddFood(food);
            }
        }
        
        public void SetCell(Vector2Int cellPos, CellType type) {
            if (_cells.TryGetValue(cellPos, out var cell)) {
                cell.SetCell(type);
            }
        }
    }
}
