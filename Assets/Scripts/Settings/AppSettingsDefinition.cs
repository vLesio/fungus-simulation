using UnityEngine;

namespace Settings {
    [CreateAssetMenu(menuName = "Settings/AppSettings", fileName = "AppSettings")]
    public class AppSettingsDefinition : ScriptableObject {
        [Header("Grid")]
        public Vector2 gridSize;

        [Header("Resources")] 
        public float incomeCellsPercentage;
        public Vector2 resourceIncomeRange;
        
        [Header("Moisture")] 
        public Vector2 moistureRange;
    }   
}
