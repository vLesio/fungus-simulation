using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using CoinPackage.Debugging;
using GridSystem;
using Settings;
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

        private KnowledgeKeeper _knowledgeKeeper;

        private void Start()
        {
            _knowledgeKeeper = KnowledgeKeeper.Instance;
        }

        public void GrowHyphas()
        {
            List<Vector2Int> hyphaPositions = GetAllCellTypePositions(CellType.Hypha);
            foreach (var hyphaPosition in hyphaPositions)
            {
                if(_knowledgeKeeper.TryToGetResourceAmount(hyphaPosition) < DevSettings.Instance.appSettings.hyphaFoodLimitToGrow)
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

                if (resourceSum == 0)
                {
                    var hardRandom = Random.Range(0, possiblePositions.Count);
                    if (!IsCellFungus(possiblePositions[hardRandom]))
                    {
                        TryToAddHyphaAtPosition(possiblePositions[hardRandom]);
                        TryToAddResourceTransport(hyphaPosition, possiblePositions[hardRandom]);
                        continue;
                    }

                    if (Random.value > 0.5)
                    {
                        AddFungusFlow(hyphaPosition, possiblePositions[hardRandom]);
                        continue;
                    }
                    AddFungusFlow(possiblePositions[hardRandom], hyphaPosition);
                    continue;
                }
                
                var randomValue = Random.Range(0, resourceSum);

                foreach (var positions in chances)
                {
                    if (randomValue <= positions.Value)
                    {
                        if (!IsCellFungus(positions.Key))
                        {
                            TryToAddHyphaAtPosition(positions.Key);
                            TryToAddResourceTransport(hyphaPosition, positions.Key);
                            break;
                        }
                        if (Random.value > 0.5)
                        {
                            AddFungusFlow(hyphaPosition, positions.Key);
                            break;
                        }
                        AddFungusFlow(positions.Key, hyphaPosition);
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
                if(newPosition.x >= DevSettings.Instance.appSettings.gridSize.x || newPosition.x < 0)
                {
                    continue;
                }
                if(newPosition.y >= DevSettings.Instance.appSettings.gridSize.y || newPosition.y < 0)
                {
                    continue;
                }
                if (AreFungusCellsConnected(position, newPosition))
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
            CGrid.Instance.SetCell(position, CellType.Hypha);
            CGrid.Instance.SetFood(position, _knowledgeKeeper.TryToGetResourceAmount(position));
            CDebug.LogWarning("Should spawn hypha at position: " + position );
        }

        public bool AreFungusCellsConnected(Vector2Int left, Vector2Int right)
        {
            if(FungusResourceTransportMap.TryGetValue(left, value: out var value))
            {
                if(value.Contains(right))
                {
                    return true;
                }
            }
            
            if(FungusResourceTransportMap.TryGetValue(right, out var value1))
            {
                if(value1.Contains(left))
                {
                    return true;
                }
            }

            return false;
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

        private bool IsCellFungus(Vector2Int cell)
        {
            return FungusMap.ContainsKey(cell);
        }
    }
}
