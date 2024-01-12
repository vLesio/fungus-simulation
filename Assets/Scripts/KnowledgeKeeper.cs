using System.Collections;
using System.Collections.Generic;
using Fungus;
using GridSystem;
using Settings;
using UnityEngine;
using Utils;
using Utils.Singleton;

[RequireComponent(typeof(DoNotDestroy))]
public class KnowledgeKeeper : Singleton<KnowledgeKeeper>
{
    public Dictionary<Vector2Int, float> ResourceMap = new Dictionary<Vector2Int, float>();
    public Dictionary<Vector2Int, float> ResourceIncomeMap = new Dictionary<Vector2Int, float>();
    public Dictionary<Vector2Int, float> MoistureMap = new Dictionary<Vector2Int, float>();
    public List<Vector2Int> RockList = new List<Vector2Int>();

    public void ClearSimulation()
    {
        RockList.Clear();
        ResourceMap.Clear();
        ResourceIncomeMap.Clear();
        MoistureMap.Clear();
    }
    public void PropagateResourceFlow(Dictionary<Vector2Int, List<Vector2Int>> resourceFlowMap)
    {
        Dictionary<Vector2Int, float> resourcesAfterFlow = new Dictionary<Vector2Int, float>();
        foreach (var cell in resourceFlowMap.Keys)
        {
            float amountToGive = TryToGetResourceAmount(cell);
            if (amountToGive > 0)
            {
                List<Vector2Int> neighbours = resourceFlowMap[cell];
                float amountToGivePerNeighbour = amountToGive / neighbours.Count;
                foreach (var neighbour in neighbours)
                {
                    if (resourcesAfterFlow.TryGetValue(neighbour, out var currentAmount))
                    {
                        resourcesAfterFlow[neighbour] = currentAmount + amountToGivePerNeighbour;
                    }
                    else
                    {
                        resourcesAfterFlow.Add(neighbour, amountToGivePerNeighbour);
                    }
                }
                TryToClearResourceInCell(cell);
            }
        }
        
        foreach(var cell in resourcesAfterFlow)
        {
            AddResourceToCell(cell.Key, resourcesAfterFlow[cell.Key]);
            TryToRemoveResourceFromCell(cell.Key, DevSettings.Instance.appSettings.hyphaConsumptionAmount);
        }
    }
    
    public void PropagateResourceIncome()
    {
        foreach (var cell in ResourceIncomeMap.Keys)
        {
            AddResourceToCell(cell, ResourceIncomeMap[cell]);
            CGrid.Instance.SetFood(cell, TryToGetResourceAmount(cell));
        }
    }

    public float TryToGetResourceAmount(Vector2Int cell)
    {
        if (ResourceMap.TryGetValue(cell, out var amount))
        {
            return amount;
        }

        return 0;
    }
    
    public float TryToGetMoistureAmount(Vector2Int cell)
    {
        if (MoistureMap.TryGetValue(cell, out var amount))
        {
            return amount;
        }

        return 0;
    }
    
    public float AddResourceToCell(Vector2Int cell, float amount)
    {
        if (ResourceMap.TryGetValue(cell, out var currentAmount))
        {
            ResourceMap[cell] = currentAmount + amount;
        }
        else
        {
            ResourceMap.Add(cell, amount);
        }
        
        CGrid.Instance.SetFood(cell, TryToGetResourceAmount(cell));
        return amount;
    }
    
    public void AddRockInCell(Vector2Int cell)
    {
        if (!RockList.Contains(cell))
        {
            RockList.Add(cell);
            CGrid.Instance.SetCell(cell, CellType.Obstacle);
        }
    }
    
    public void RemoveRockFromCell(Vector2Int cell)
    {
        if (RockList.Contains(cell))
        {
            RockList.Remove(cell);
            CGrid.Instance.SetCell(cell, CellType.Dirt);
        }
    }
    
    public float SetResourceToCell(Vector2Int cell, float amount)
    {
        if (ResourceMap.TryGetValue(cell, out var currentAmount))
        {
            ResourceMap[cell] = amount;
        }
        else
        {
            ResourceMap.Add(cell, amount);
        }
        CGrid.Instance.SetFood(cell, TryToGetResourceAmount(cell));
        return amount;
    }
    
    public void TryToRemoveResourceFromCell(Vector2Int cell, float amount)
    {
        if (ResourceMap.TryGetValue(cell, out var currentAmount))
        {
            ResourceMap[cell] = currentAmount - amount;
            CGrid.Instance.SetFood(cell, TryToGetResourceAmount(cell));
            if(ResourceMap[cell] <= 0)
            {
                ResourceMap.Remove(cell);
            }
        }
    }
    
    public void TryToClearResourceInCell(Vector2Int cell)
    {
        if (ResourceMap.TryGetValue(cell, out var currentAmount))
        {
            ResourceMap.Remove(cell);
        }
    }
    
    public void AddResourceIncomeToCell(Vector2Int cell, float amount)
    {
        if (ResourceIncomeMap.TryGetValue(cell, out var currentAmount))
        {
            ResourceIncomeMap[cell] = currentAmount + amount;
        }
        else
        {
            ResourceIncomeMap.Add(cell, amount);
        }
    }
    
    public void RemoveResourceIncomeFromCell(Vector2Int cell, float amount)
    {
        if (ResourceIncomeMap.TryGetValue(cell, out var currentAmount))
        {
            ResourceIncomeMap[cell] = currentAmount - amount;
            if(ResourceIncomeMap[cell] <= 0)
            {
                ResourceIncomeMap.Remove(cell);
            }
        }
    }

    public void SetResourceIncomeInCell(Vector2Int cell, float amount)
    {
        if (ResourceIncomeMap.TryGetValue(cell, out var currentAmount))
        {
            ResourceIncomeMap[cell] = amount;
        }
        else
        {
            ResourceIncomeMap.Add(cell, amount);
        }
    }
    
    public void ClearResourceIncomeInCell(Vector2Int cell)
    {
        if (ResourceIncomeMap.TryGetValue(cell, out var currentAmount))
        {
            ResourceIncomeMap.Remove(cell);
        }
    }
}
