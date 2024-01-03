using System;
using System.Collections;
using System.Collections.Generic;
using CoinPackage.Debugging;
using Fungus;
using Settings;
using UnityEngine;

namespace GridSystem {
    public class Cell : MonoBehaviour {
        [NonSerialized] public Vector2Int Cords;
        [SerializeField] private GameObject cell;
        private SpriteRenderer _cellRenderer;
        private float _food;
        private CellType _currentType;
        private readonly AppSettingsDefinition _settings = DevSettings.Instance.appSettings;
        // Start is called before the first frame update
        void Awake() {
            _cellRenderer = cell.GetComponent<SpriteRenderer>();
            _food = 0f;
        }

        public void OnLeftClicked() {
            CDebug.LogWarning($"Painting is not yet implemented (it may never be).");
            CGrid.Instance.AddFood(Cords, 0.01f);
            CGrid.Instance.SetCell(Cords, CellType.Hypha);
        }
        
        public void OnRightClicked() {
            CDebug.LogWarning($"Painting is not yet implemented (it may never be).");
            CGrid.Instance.SetCell(Cords, CellType.Dirt);
        }

        public void SetFood(float food) {
            _food = food;
            SetCell(_currentType);
        }
        
        public void AddFood(float food) {
            _food += food;
            SetCell(_currentType);
        }

        public void SetCell(CellType type) {
            // CDebug.Log($"Setting to {type}");
            _currentType = type;
            switch (type) {
                case CellType.Dirt:
                    _cellRenderer.color = _food <= 0f
                        ? _settings.groundColor
                        : new Color(_settings.foodColor.r, _settings.foodColor.g, _settings.foodColor.b, 1 - _food);
                    break;
                case CellType.Obstacle:
                    _cellRenderer.color = _settings.obstacleColor;
                    break;
                case CellType.Hypha:
                    _cellRenderer.color = new Color(Math.Clamp(_food, 0f, 1f), 0f, Math.Clamp(1f - _food, 0f, 1f));
                    break;
                default:
                    CDebug.LogWarning($"Could not set cell, type {type % Colorize.Red} not found.");
                    break;
            }
        }
    } 
}
