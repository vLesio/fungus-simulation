using UnityEngine;

namespace Settings {
    [CreateAssetMenu(menuName = "Settings/AppSettings", fileName = "AppSettings")]
    public class AppSettingsDefinition : ScriptableObject {
        [Header("Grid")]
        public Vector2 gridSize;
    }   
}
