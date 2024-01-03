using UnityEngine;

namespace Fungus
{
    public enum CellType
    {
        Dirt,
        Obstacle,
        Hypha,
    }
    
    public class BaseCell
    {
        public CellType CellType;
        public Vector2Int Position;
        public float CurrentFood;
        
    }
}
