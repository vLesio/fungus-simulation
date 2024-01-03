using System.Collections.Generic;
using Fungus;
using Settings;
using UnityEngine;

public static class SimulationInitializer
{
    public static void GenerateRandomResourceIncome(Dictionary<Vector2Int, float> resourceIncomeMap)
    {
        var grid = DevSettings.Instance.appSettings.gridSize;
        var cellsWithIncome = grid.x * grid.y * DevSettings.Instance.appSettings.incomeCellsPercentage;
        for (int i = 0; i < cellsWithIncome; i++)
        {
            var cell = new Vector2Int(Random.Range(0, (int) grid.x), Random.Range(0, (int) grid.y));
            while (resourceIncomeMap.ContainsKey(cell))
            {
                cell = new Vector2Int(Random.Range(0, (int) grid.x), Random.Range(0, (int) grid.y));
            }
            
            var income = Random.Range(DevSettings.Instance.appSettings.resourceIncomeRange.x,
                DevSettings.Instance.appSettings.resourceIncomeRange.y);
            resourceIncomeMap.Add(cell, income);
        }
    }
    
    public static void GenerateRandomMoisture(Dictionary<Vector2Int, float> moistureMap)
    {
        for(int x = 0; x < DevSettings.Instance.appSettings.gridSize.x; x++)
        {
            for(int y = 0; y < DevSettings.Instance.appSettings.gridSize.y; y++)
            {
                var cell = new Vector2Int(x, y);
                var moisture = Random.Range(DevSettings.Instance.appSettings.moistureRange.x,
                    DevSettings.Instance.appSettings.moistureRange.y);
                moistureMap.Add(cell, moisture);
            }
        }
    }

    public static void SpawnFungalInCenter(FungusManager fungusManager)
    {
        var center = GetMiddleCell();
        
        fungusManager.TryToAddHyphaAtPosition(center);
        var crossPositions = GetCrossPositionsAroundCell(center);
        foreach (var crossPosition in crossPositions)
        {
            fungusManager.TryToAddStrobenkoperAtPosition(crossPosition);
        }
    }

    private static Vector2Int GetMiddleCell()
    {
        var grid = DevSettings.Instance.appSettings.gridSize;
        return new Vector2Int((int) grid.x / 2, (int) grid.y / 2);
    }

    private static List<Vector2Int> GetCrossPositionsAroundCell(Vector2Int cell)
    {
        List<Vector2Int> output = new List<Vector2Int>();
        output.Add(new Vector2Int(cell.x + 1, cell.y));
        output.Add(new Vector2Int(cell.x - 1, cell.y));
        output.Add(new Vector2Int(cell.x, cell.y + 1));
        output.Add(new Vector2Int(cell.x, cell.y - 1));
        return output;
    }

}