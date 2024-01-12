using System;
using System.Reflection;
using CoinPackage.Debugging;
using Fungus;
using TMPro;
using UnityEngine;
using Utils.Singleton;

namespace Painting {
    public class PaintingManager : Singleton<PaintingManager> {

        [SerializeField] private TMP_Dropdown typeDropdown;
        [SerializeField] private TMP_Dropdown operationDropdown;
        [SerializeField] private TMP_InputField valueField;

        [SerializeField] private PaintingMode paintingMode;
        [SerializeField] private PaintingOperation paintingOperationLeft;
        [SerializeField] private PaintingOperation paintingOperationRight;
        [SerializeField] private float value;
        [SerializeField] private Vector2Int cellToPaint;

        protected override void Awake() {
            base.Awake();
            typeDropdown.onValueChanged.AddListener(OnTypeChange);
            operationDropdown.onValueChanged.AddListener(OnOperationChange);
            valueField.onValueChanged.AddListener(OnValueChange);
        }

        public void OnTypeChange(int val) {
            switch (val) {
                case 0:
                    paintingMode = PaintingMode.Rock;
                    break;
                case 1:
                    paintingMode = PaintingMode.Food;
                    break;
                case 2:
                    paintingMode = PaintingMode.FoodSource;
                    break;
                case 3:
                    paintingMode = PaintingMode.Fungus;
                    break;
                default:
                    paintingMode = PaintingMode.Rock;
                    CDebug.LogWarning("Set incorrect painting mode.");
                    break;
            }
        }

        public void OnOperationChange(int val) {
            switch (val) {
                case 0:
                    paintingOperationLeft = PaintingOperation.Set;
                    paintingOperationRight = PaintingOperation.Erase;
                    break;
                case 1:
                    paintingOperationLeft = PaintingOperation.Add;
                    paintingOperationRight = PaintingOperation.Remove;
                    break;
                default:
                    CDebug.LogWarning("Selected wrong painting command value.");
                    break;
            }
        }

        public void OnValueChange(string val) {
            value = float.Parse(val);
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

        private void SetFood() {
            KnowledgeKeeper.Instance.SetResourceToCell(cellToPaint, value);
        }
        
        private void EraseFood()
        {
            KnowledgeKeeper.Instance.TryToClearResourceInCell(cellToPaint);
        }

        private void AddFood() {
            KnowledgeKeeper.Instance.AddResourceToCell(cellToPaint, value);
        }

        private void RemoveFood() {
            KnowledgeKeeper.Instance.TryToRemoveResourceFromCell(cellToPaint, value);
        }

        private void SetRock() {
            KnowledgeKeeper.Instance.AddRockInCell(cellToPaint);
        }
        
        private void EraseRock() {
            KnowledgeKeeper.Instance.RemoveRockFromCell(cellToPaint);
        }

        private void AddRock() {
            KnowledgeKeeper.Instance.AddRockInCell(cellToPaint);
        }

        private void RemoveRock() {
            KnowledgeKeeper.Instance.RemoveRockFromCell(cellToPaint);
        }
        
        private void SetFoodSource() {
            KnowledgeKeeper.Instance.SetResourceIncomeInCell(cellToPaint, value);
        }
        
        private void EraseFoodSource() {
            KnowledgeKeeper.Instance.ClearResourceIncomeInCell(cellToPaint);
        }

        private void AddFoodSource() {
            KnowledgeKeeper.Instance.AddResourceIncomeToCell(cellToPaint, value);
        }

        private void RemoveFoodSource() {
            KnowledgeKeeper.Instance.RemoveResourceIncomeFromCell(cellToPaint, value);
        }
        
        private void SetFungus() {
            FungusManager.Instance.TryToAddHyphaAtPosition(cellToPaint);
        }
        
        private void EraseFungus() {
            FungusManager.Instance.EraseHyphaAtPosition(cellToPaint);
        }

        private void AddFungus() {
            FungusManager.Instance.TryToAddHyphaAtPosition(cellToPaint);
        }

        private void RemoveFungus() {
            FungusManager.Instance.EraseHyphaAtPosition(cellToPaint);
        }
        
        
    }
}