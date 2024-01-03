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

        private readonly AppSettingsDefinition _settings = DevSettings.Instance.appSettings;
        // Start is called before the first frame update
        void Awake() {
            _cellRenderer = cell.GetComponent<SpriteRenderer>();
        }

        public void OnClicked() {
            CDebug.LogWarning($"Painting is not yet implemented (it may never be).");
            CGrid.Instance.SetCell(Cords, CellType.Fungus, Time.time / 10f);
        }

        public void SetCell(CellType type, float value) {
            CDebug.Log($"Setting to {type}");
            switch (type) {
                case CellType.Empty:
                    _cellRenderer.color = _settings.groundColor;
                    break;
                case CellType.Obstacle:
                    _cellRenderer.color = _settings.obstacleColor;
                    break;
                case CellType.Food:
                    _cellRenderer.color = new Color(_settings.foodColor.r, _settings.foodColor.g, _settings.foodColor.b, value);
                    break;
                case CellType.Fungus:
                    _cellRenderer.color = new Color(Math.Clamp(value, 0f, 1f), 0f, Math.Clamp(1f - value, 0f, 1f));
                    break;
                default:
                    CDebug.LogWarning($"Could not set cell, type {type % Colorize.Red} not found.");
                    break;
            }
        }
    } 
}
