using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

namespace Fungus
{
    public class FungusManager : Utils.Singleton.Singleton<FungusManager>
    {
        private Dictionary<Vector2Int, CellType> FungusMap = new Dictionary<Vector2Int, CellType>();
        private Dictionary<Vector2Int, List<Vector2Int>> FungusResourceTransportMap = new Dictionary<Vector2Int, List<Vector2Int>>();
        
        private KnowledgeKeeper _knowledgeKeeper = KnowledgeKeeper.Instance;

        public void GrowHyphas()
        {
            //TODO: implement
        }

        public void GrowStrobenkopers()
        {
            //TODO: implement
        }

        public void TransportResources()
        {
            _knowledgeKeeper.PropagateResourceFlow(FungusResourceTransportMap);
        }
        
        public void TryToAddHyphaAtPosition(Vector2Int position)
        {
            TryToAddCellTypeAtPosition(position, CellType.Hypha);
        }
        
        public void TryToAddStrobenkoperAtPosition(Vector2Int position)
        {
            TryToAddCellTypeAtPosition(position, CellType.Strobenkoper);
        }
        
        public void TryToAddCellTypeAtPosition(Vector2Int position, CellType cellType)
        {
            if (FungusMap.ContainsKey(position))
            {
                Debug.LogError("Cell already occupied");
                return;
            }
            FungusMap.Add(position, cellType);
        }

        private List<Vector2Int> GetAllCellTypePositions(CellType cellType)
        {
            List<Vector2Int> result = new List<Vector2Int>();
            foreach (var item in FungusMap)
            {
                if (item.Value == cellType)
                {
                    result.Add(item.Key);
                }
            }
            return result;
        }
    }
}
