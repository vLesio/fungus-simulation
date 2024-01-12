using System;
using System.Reflection;
using CoinPackage.Debugging;
using UnityEngine;
using Utils.Singleton;

namespace Painting {
    public class PaintingManager : Singleton<PaintingManager> {

        [SerializeField] private PaintingMode paintingMode;
        [SerializeField] private PaintingOperation paintingOperationLeft;
        [SerializeField] private PaintingOperation paintingOperationRight;
        [SerializeField] private float value;
        [SerializeField] private Vector2Int cellToPaint;

        protected override void Awake() {
            base.Awake();
        }

        public void OnTypeChange() {
            
        }

        public void OnOperationChange() {
            
        }

        public void OnValueChange() {
            
        }
        
        public void ModifyCellLeft(Vector2Int cords) {
            CDebug.LogWarning($"{paintingOperationLeft.ToString()}{paintingMode.ToString()}");
            cellToPaint = cords;
            Type thisType = this.GetType();
            MethodInfo methodInfo = thisType.GetMethod($"{paintingOperationLeft.ToString()}{paintingMode.ToString()}");
            methodInfo?.Invoke(this, null);
        }

        public void ModifyCellRight(Vector2Int cords) {
            CDebug.LogWarning($"{paintingOperationRight.ToString()}{paintingMode.ToString()}");
            cellToPaint = cords;
            Type thisType = this.GetType();
            MethodInfo methodInfo = thisType.GetMethod($"{paintingOperationRight.ToString()}{paintingMode.ToString()}");
            methodInfo?.Invoke(this, null);
        }

        private void SetFood(Vector2Int cords) {
            CDebug.Log("SetFood");
        }
        
        private void EraseFood(Vector2Int cords) {
            CDebug.Log("EraseFood");
        }

        private void AddFood(Vector2Int cords) {
            CDebug.Log("AddFood");
        }

        private void RemoveFood(Vector2Int cords) {
            CDebug.Log("RemoveFood");
        }

        private void SetRock(Vector2Int cords) {
            CDebug.Log("SetRock");
        }
        
        private void EraseRock(Vector2Int cords) {
            CDebug.Log("EraseRock");
        }

        private void AddRock(Vector2Int cords) {
            CDebug.Log("AddRock");
        }

        private void RemoveRock(Vector2Int cords) {
            CDebug.Log("RemoveRock");
        }
        
        private void SetFoodSource(Vector2Int cords) {
            CDebug.Log("SetFoodSource");
        }
        
        private void EraseFoodSource(Vector2Int cords) {
            CDebug.Log("EraseFoodSource");
        }

        private void AddFoodSource(Vector2Int cords) {
            CDebug.Log("AddFoodSource");
        }

        private void RemoveFoodSource(Vector2Int cords) {
            CDebug.Log("RemoveFoodSource");
        }
        
        private void SetFungus(Vector2Int cords) {
            CDebug.Log("SetFungus");
        }
        
        private void EraseFungus(Vector2Int cords) {
            CDebug.Log("EraseFungus");
        }

        private void AddFungus(Vector2Int cords) {
            CDebug.Log("AddFungus");
        }

        private void RemoveFungus(Vector2Int cords) {
            CDebug.Log("RemoveFungus");
        }
        
        
    }
}