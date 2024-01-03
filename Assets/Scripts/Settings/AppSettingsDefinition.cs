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
    }   
}
