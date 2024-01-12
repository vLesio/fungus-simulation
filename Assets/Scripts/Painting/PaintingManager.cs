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
            cellToPaint = cords;
            Type thisType = this.GetType();
            MethodInfo methodInfo = thisType.GetMethod($"{paintingOperationLeft.ToString()}{paintingMode.ToString()}");
            methodInfo?.Invoke(this, null);
        }

        public void ModifyCellRight(Vector2Int cords) {
            cellToPaint = cords;
            Type thisType = this.GetType();
            MethodInfo methodInfo = thisType.GetMethod($"{paintingOperationRight.ToString()}{paintingMode.ToString()}");
            methodInfo?.Invoke(this, null);
        }

        public void SetFood() {
            KnowledgeKeeper.Instance.SetResourceToCell(cellToPaint, value);
            CDebug.LogWarning("SetFood");
        }
        
        public void EraseFood()
        {
            KnowledgeKeeper.Instance.TryToClearResourceInCell(cellToPaint);
        }

        public void AddFood() {
            KnowledgeKeeper.Instance.AddResourceToCell(cellToPaint, value);
        }

        public void RemoveFood() {
            KnowledgeKeeper.Instance.TryToRemoveResourceFromCell(cellToPaint, value);
        }

        public void SetRock() {
            KnowledgeKeeper.Instance.AddRockInCell(cellToPaint);
        }
        
        public void EraseRock() {
            KnowledgeKeeper.Instance.RemoveRockFromCell(cellToPaint);
        }

        public void AddRock() {
            KnowledgeKeeper.Instance.AddRockInCell(cellToPaint);
        }

        public void RemoveRock() {
            KnowledgeKeeper.Instance.RemoveRockFromCell(cellToPaint);
        }
        
        public void SetFoodSource() {
            KnowledgeKeeper.Instance.SetResourceIncomeInCell(cellToPaint, value);
        }
        
        public void EraseFoodSource() {
            KnowledgeKeeper.Instance.ClearResourceIncomeInCell(cellToPaint);
        }

        public void AddFoodSource() {
            KnowledgeKeeper.Instance.AddResourceIncomeToCell(cellToPaint, value);
        }

        public void RemoveFoodSource() {
            KnowledgeKeeper.Instance.RemoveResourceIncomeFromCell(cellToPaint, value);
        }
        
        public void SetFungus() {
            FungusManager.Instance.TryToAddHyphaAtPosition(cellToPaint);
        }
        
        public void EraseFungus() {
            FungusManager.Instance.EraseHyphaAtPosition(cellToPaint);
        }

        public void AddFungus() {
            FungusManager.Instance.TryToAddHyphaAtPosition(cellToPaint);
        }

        public void RemoveFungus() {
            FungusManager.Instance.EraseHyphaAtPosition(cellToPaint);
        }
        
        
    }
}