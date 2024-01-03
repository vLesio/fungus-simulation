using System;
using System.Collections;
using System.Collections.Generic;
using CoinPackage.Debugging;
using Settings;
using UnityEngine;

namespace GridSystem {
    public class Cell : MonoBehaviour {
        [NonSerialized] public Vector2Int Cords;
        [SerializeField] private GameObject cell;
        private SpriteRenderer _cellRenderer;
        private float _food;
        private readonly AppSettingsDefinition _settings = DevSettings.Instance.appSettings;
        // Start is called before the first frame update
        void Awake() {
            _cellRenderer = cell.GetComponent<SpriteRenderer>();
        }

        public void OnClicked() {
            CDebug.LogWarning($"Painting is not yet implemented (it may never be).");
            CGrid.Instance.SetCell(Cords, CellType.Fungus, Time.time / 10f);
        }

        public void SetFood(float food) {
            _food = food;
        }
        
        public void AddFood(float food) {
            _food += food;
        }

        public void SetCell(CellType type) {
            CDebug.Log($"Setting to {type}");
            switch (type) {
                case CellType.Empty:
                    _cellRenderer.color = _settings.groundColor;
                    break;
                case CellType.Obstacle:
                    _cellRenderer.color = _settings.obstacleColor;
                    break;
                case CellType.Food:
                    _cellRenderer.color = new Color(_settings.foodColor.r, _settings.foodColor.g, _settings.foodColor.b, _food);
                    break;
                case CellType.Fungus:
                    _cellRenderer.color = new Color(Math.Clamp(_food, 0f, 1f), 0f, Math.Clamp(1f - _food, 0f, 1f));
                    break;
                default:
                    CDebug.LogWarning($"Could not set cell, type {type % Colorize.Red} not found.");
                    break;
            }
        }
    } 
}
