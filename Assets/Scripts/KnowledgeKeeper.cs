using System.Collections;
using System.Collections.Generic;
using Fungus;
using Unity.VisualScripting;
using UnityEngine;
using Utils;

[RequireComponent(typeof(DoNotDestroy))]
public class KnowledgeKeeper : Utils.Singleton.Singleton<KnowledgeKeeper>
{
    public Dictionary<Vector2Int, float> ResourceMap = new Dictionary<Vector2Int, float>();
    public Dictionary<Vector2Int, float> ResourceIncomeMap = new Dictionary<Vector2Int, float>();
    public Dictionary<Vector2Int, float> MoistureMap = new Dictionary<Vector2Int, float>();
    public List<Vector2Int> RockList = new List<Vector2Int>();
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
            }
        }
        ResourceMap = resourcesAfterFlow;
    }
    
    public void PropagateResourceIncome()
    {
        foreach (var cell in ResourceIncomeMap.Keys)
        {
            AddResourceToCell(cell, ResourceIncomeMap[cell]);
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

        return amount;
    }
    
    public void TryToRemoveResourceFromCell(Vector2Int cell, float amount)
    {
        if (ResourceMap.TryGetValue(cell, out var currentAmount))
        {
            if (currentAmount <= 0)
            {
                Debug.LogError("Trying to remove resource from cell with 0 amount");
            }
            if (currentAmount <= amount)
            {
                Debug.LogError("Trying to remove more resource than there is in cell");
            }
            
            ResourceMap[cell] = currentAmount - amount;
            if(ResourceMap[cell] <= 0)
            {
                ResourceMap.Remove(cell);
            }
        }
    }
    
}
