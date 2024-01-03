using System;
using System.Collections;
using System.Collections.Generic;
using CoinPackage.Debugging;
using UnityEngine;

namespace GridSystem {
    public class Cell : MonoBehaviour {
        [NonSerialized] public Vector2Int Cords;
        [SerializeField] private GameObject cell;
        private SpriteRenderer _cellRenderer;
        // Start is called before the first frame update
        void Awake() {
            _cellRenderer = cell.GetComponent<SpriteRenderer>();
        }

        public void OnClicked() {
            CDebug.LogWarning($"Painting is not yet implemented (it may never be).");
            CGrid.Instance.SetCell(Cords, CellType.Food, 0.5f);
        }

        public void SetCell(CellType type, float value) {
            CDebug.Log($"Setting to {type}");
            switch (type) {
                case CellType.Empty:
                    _cellRenderer.color = new Color(0.3f, 0.3f, 0.3f, 1f);
                    break;
                case CellType.Obstacle:
                    _cellRenderer.color = new Color(1f, 0f, 0f, 1f);
                    break;
                case CellType.Food:
                    _cellRenderer.color = new Color(0f, 1f, 0f, value);
                    break;
                default:
                    CDebug.LogWarning($"Could not set cell, type {type % Colorize.Red} not found.");
                    break;
            }
        }
    } 
}
