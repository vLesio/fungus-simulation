using UnityEngine;

namespace Fungus
{
    public enum CellType
    {
        Dirt,
        Hypha,
        Strobenkoper
    }
    
    public class BaseCell
    {
        public CellType CellType;
        public Vector2Int Position;
        public float CurrentFood;
        
    }
}
