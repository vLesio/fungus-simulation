using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Fungus
{
    public class FungusManager : Utils.Singleton.Singleton<FungusManager>
    {
        private Dictionary<Vector2Int, CellType> FungusMap = new Dictionary<Vector2Int, CellType>();
        private Dictionary<Vector2Int, List<Vector2Int>> FungusResourceTransportMap = new Dictionary<Vector2Int, List<Vector2Int>>();
        
        private KnowledgeKeeper _knowledgeKeeper = KnowledgeKeeper.Instance;

        public void GrowHyphas()
        {
            List<Vector2Int> hyphaPositions = GetAllCellTypePositions(CellType.Hypha);
            foreach (var hyphaPosition in hyphaPositions)
            {
                if(_knowledgeKeeper.TryToGetResourceAmount(hyphaPosition) < 1f)
                {
                    continue;
                }
                
                List<Vector2Int> possiblePositions = GetPossiblePositions(hyphaPosition);
                if (possiblePositions.Count == 0)
                {
                    continue;
                }
                
                var resourceSum = 0f;
                var chances = new Dictionary<Vector2Int, float>();
                foreach (var possiblePosition in possiblePositions)
                {
                    resourceSum += _knowledgeKeeper.TryToGetResourceAmount(possiblePosition);
                    resourceSum += _knowledgeKeeper.TryToGetMoistureAmount(possiblePosition);
                    chances.Add(possiblePosition, resourceSum);
                }
                var randomValue = Random.Range(0, resourceSum);

                foreach (var positions in chances)
                {
                    if (randomValue <= positions.Value)
                    {
                        TryToAddHyphaAtPosition(positions.Key);
                        TryToAddResourceTransport(hyphaPosition, positions.Key);
                        break;
                    }
                }
            }
        }

        public void TryToAddResourceTransport(Vector2Int parent, Vector2Int child)
        {
            AddFungusFlow(parent, child);
        }
        
        private void AddFungusFlow(Vector2Int from, Vector2Int to)
        {
            if (FungusResourceTransportMap.ContainsKey(from))
            {
                FungusResourceTransportMap[from].Add(to);
                return;
            }
            FungusResourceTransportMap.Add(from, new List<Vector2Int>{ to });
        }
        
        public List<Vector2Int> GetPossiblePositions(Vector2Int position)
        {
            List<Vector2Int> result = new List<Vector2Int>();
            Vector2Int[] directions = new Vector2Int[]
            {
                Vector2Int.up,
                Vector2Int.down,
                Vector2Int.left,
                Vector2Int.right
            };
            foreach (var direction in directions)
            {
                Vector2Int newPosition = position + direction;
                if (FungusMap.ContainsKey(newPosition))
                {
                    continue;
                }
                if (_knowledgeKeeper.RockList.Contains(newPosition))
                {
                    continue;
                }
                result.Add(newPosition);
            }
            return result;
        }

        public void TransportResources()
        {
            _knowledgeKeeper.PropagateResourceFlow(FungusResourceTransportMap);
        }
        
        public void TryToAddHyphaAtPosition(Vector2Int position)
        {
            TryToAddCellTypeAtPosition(position, CellType.Hypha);
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
