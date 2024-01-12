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
                if(DoesCellHasEnoughResources(hyphaPosition))
                {
                    MakeRandomGrow(hyphaPosition);
                }

                MakeConnection(hyphaPosition);
                
                if (DevSettings.Instance.appSettings.hyphaReRollFlowDirectionIfNotComingOut)
                {
                    if (DoesCellGetsFoodFromAllDirections(hyphaPosition))
                    {
                        // CDebug.LogWarning("Detected cell that gets food from all directions");
                        InvertRandomConnection(hyphaPosition);
                    }
                }
            }
        }

        private void MakeRandomGrow(Vector2Int cell)
        {
            List<Vector2Int> possiblePositions = GetPossibleGrowPositions(cell);
            if (possiblePositions.Count == 0)
            {
                return;
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
                    TryToAddResourceTransport(cell, possiblePositions[hardRandom]);
                    return;
                }
            }
                
            var randomValue = Random.Range(0, resourceSum);

            foreach (var positions in chances)
            {
                if (randomValue <= positions.Value)
                {
                    TryToAddHyphaAtPosition(positions.Key);
                    TryToAddResourceTransport(cell, positions.Key);
                    return;
                }
            }
        }

        private void MakeConnection(Vector2Int cell)
        {
            List<Vector2Int> possiblePositions = GetPossibleConnectionPositions(cell);
            if (possiblePositions.Count == 0)
            {
                return;
            }

            foreach (var position in possiblePositions)
            {
                if (Random.value > 0.5)
                {
                    AddFungusFlow(cell, position);
                    break;
                }
                AddFungusFlow(position, cell);
                break;
            }
        }

        private bool DoesCellGetsFoodFromAllDirections(Vector2Int cell)
        {
            //TODO: Handle case when cell is at the edge
            return GetAllPositionsFeedingMe(cell).Count == 4;
        }

        private void InvertRandomConnection(Vector2Int cell)
        {
            var feeders = GetAllPositionsFeedingMe(cell);
            var randomFeeder = feeders[Random.Range(0, feeders.Count)];
            
            InvertConnection(randomFeeder, cell);
        }

        private void InvertConnection(Vector2Int actualFeeder, Vector2Int actualReceiver)
        {
            RemoveFungusFlow(actualFeeder, actualReceiver);
            AddFungusFlow(actualReceiver, actualFeeder);
            // CDebug.LogError("DIRECTION INVERTED");
        }

        private bool DoesCellHasEnoughResources(Vector2Int cell)
        {
            return _knowledgeKeeper.TryToGetResourceAmount(cell) >=
                   DevSettings.Instance.appSettings.hyphaFoodLimitToGrow;
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
        
        private void RemoveFungusFlow(Vector2Int from, Vector2Int to)
        {
            if (FungusResourceTransportMap.ContainsKey(from))
            {
                FungusResourceTransportMap[from].Remove(to);
                return;
            }

            return;
        }

        private List<Vector2Int> GetPossibleGrowPositions(Vector2Int position)
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
                if (_knowledgeKeeper.RockList.Contains(newPosition))
                {
                    continue;
                }
                if (IsCellFungus(newPosition)) {
                    continue;
                }
                result.Add(newPosition);
            }
            return result;
        }

        private List<Vector2Int> GetPossibleConnectionPositions(Vector2Int position)
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
                if(newPosition.x >= DevSettings.Instance.appSettings.gridSize.x || newPosition.x < 0) {
                    continue;
                }
                if(newPosition.y >= DevSettings.Instance.appSettings.gridSize.y || newPosition.y < 0) {
                    continue;
                }
                if (!IsCellFungus(newPosition)) {
                    continue;
                }
                if (AreFungusCellsConnected(position, newPosition)) {
                    continue;
                }
                if (_knowledgeKeeper.RockList.Contains(newPosition)) {
                    continue;
                }
                // UNCOMENT IF YOU REALY KNOW WHAT YOU DOING :0
                // if (!DoesCellHasEnoughResources(newPosition)) {
                //     continue;
                // }
                
                result.Add(newPosition);
            }
            return result;
        }

        private List<Vector2Int> GetAllPositionsFeedingMe(Vector2Int position)
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
                if(newPosition.x >= DevSettings.Instance.appSettings.gridSize.x || newPosition.x < 0) {
                    continue;
                }
                if(newPosition.y >= DevSettings.Instance.appSettings.gridSize.y || newPosition.y < 0) {
                    continue;
                }
                if (!IsCellFungus(newPosition)) {
                    continue;
                }
                if (!DoesCellFeedMe(position, newPosition)) {
                    continue;
                }
                
                result.Add(newPosition);
            }
            return result;
        }

        private bool DoesCellFeedMe(Vector2Int me, Vector2Int potentialFeeder)
        {
            return FungusResourceTransportMap.TryGetValue(potentialFeeder, out var value) && value.Contains(me);
        }

        public void TransportResources()
        {
            _knowledgeKeeper.PropagateResourceFlow(FungusResourceTransportMap);
        }
        
        public void TryToAddHyphaAtPosition(Vector2Int position)
        {
            TryToAddCellTypeAtPosition(position, CellType.Hypha);
        }

        private void TryToAddCellTypeAtPosition(Vector2Int position, CellType cellType)
        {
            if (FungusMap.ContainsKey(position))
            {
                Debug.LogError("Cell already occupied");
                return;
            }
            FungusMap.Add(position, cellType);
            CGrid.Instance.SetCell(position, CellType.Hypha);
            CGrid.Instance.SetFood(position, _knowledgeKeeper.TryToGetResourceAmount(position));
            // CDebug.LogWarning("Should spawn hypha at position: " + position );
        }
        
        public void EraseHyphaAtPosition(Vector2Int position)
        {
            if(FungusMap.ContainsKey(position))
            {
                FungusMap.Remove(position);
                CGrid.Instance.SetCell(position, CellType.Dirt);
                CGrid.Instance.SetFood(position, _knowledgeKeeper.TryToGetResourceAmount(position));
                if (FungusResourceTransportMap.ContainsKey(position))
                {
                    FungusResourceTransportMap.Remove(position);
                }
                
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
                    if (FungusResourceTransportMap.ContainsKey(newPosition) &&
                        FungusResourceTransportMap[newPosition].Contains(position))
                    {
                        FungusResourceTransportMap[newPosition].Remove(position);
                    }
                }
            }
        }

        private bool AreFungusCellsConnected(Vector2Int left, Vector2Int right)
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

        public void RemoveDeadCells()
        {
            Vector2Int[] directions = new Vector2Int[]
            {
                Vector2Int.up,
                Vector2Int.down,
                Vector2Int.left,
                Vector2Int.right
            };

            var toRemove = new List<Vector2Int>();
            
            foreach (var cell in FungusMap)
            {
                if (!_knowledgeKeeper.ResourceMap.ContainsKey(cell.Key))
                {
                    toRemove.Add(cell.Key);
                } 
            }

            foreach (var cell in toRemove) {
                FungusMap.Remove(cell);
                
                if (FungusResourceTransportMap.ContainsKey(cell))
                    FungusResourceTransportMap.Remove(cell);
                    
                _knowledgeKeeper.TryToClearResourceInCell(cell);
                CGrid.Instance.SetFood(cell, 0);
                CGrid.Instance.SetCell(cell, CellType.Dirt);
                    
                foreach (var direction in directions)
                {
                    if (DoesCellFeedMe(cell, cell + direction))
                    {
                        RemoveFungusFlow(cell + direction, cell);
                    }
                }
            }
        }
    }
}
