using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridSystem {
    public class Cell : MonoBehaviour {
        [SerializeField] private GameObject cell;
        private SpriteRenderer _cellRenderer;
        // Start is called before the first frame update
        void Awake() {
            _cellRenderer = cell.GetComponent<SpriteRenderer>();
        }

        public void OnClicked() {
            _cellRenderer.color = Color.blue;
        }
    } 
}
