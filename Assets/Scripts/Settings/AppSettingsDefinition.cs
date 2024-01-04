using UnityEngine;

namespace Settings {
    [CreateAssetMenu(menuName = "Settings/AppSettings", fileName = "AppSettings")]
    public class AppSettingsDefinition : ScriptableObject {
        [Header("Grid")]
        public Vector2 gridSize;
        
        [Header("Movement")]
        [Range(0f, 2f)] public float cameraMovementSpeed = 5f;
        public Vector2 cameraZoomRange = new Vector2(0.1f, 30f);
        public bool slowWhenZoomed = true;

        [Header("Presentation")]
        public Color groundColor;
        public Color foodColor;
        public Color obstacleColor;
        public Vector2 FoodColorClamp;
        [Range(0f, 3f)] public float timeBetweenUpdates;

        [Header("Resources")] 
        public float incomeCellsPercentage;
        public Vector2 resourceIncomeRange;
        
        [Header("Moisture")] 
        public Vector2 moistureRange;
        
        [Header("Fungus")] 
        public float centerFoodIncomeAmount;
        public float hyphaFoodLimitToGrow;
    }   
}
