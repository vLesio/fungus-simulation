using System;
using System.Collections;
using System.Collections.Generic;
using CoinPackage.Debugging;
using Fungus;
using Settings;
using Utils.Singleton;
using UnityEngine;
using Utils;

[RequireComponent(typeof(DoNotDestroy))]
public class GameManager : Singleton<GameManager>
{
    private KnowledgeKeeper _knowledgeKeeper;
    private FungusManager _fungusManager;

    private bool _isStarted = false;
    public int step;
    float elapsed = 0f;

    private void Start()
    {
        _knowledgeKeeper = KnowledgeKeeper.Instance;
        _fungusManager = FungusManager.Instance;
    }

    public void ClearSimulation()
    {
        _knowledgeKeeper.ClearSimulation();
        _fungusManager.ClearSimulation();
        GridSystem.CGrid.Instance.ClearGrid();
        _isStarted = false;
    }

    public void StartStopSimulation()
    {
        _isStarted = !_isStarted;
    }

    public void Init()
    {
        SimulationInitializer.GenerateRandomMoisture(_knowledgeKeeper.MoistureMap);
        SimulationInitializer.SpawnFungalInCenter(_fungusManager);
        SimulationInitializer.SpawnResourceIncomeInCenter(_knowledgeKeeper);
        SimulationInitializer.GenerateRandomResourceIncome(_knowledgeKeeper.ResourceIncomeMap);
        _isStarted = true;
        step = 0;
    }

    private void Update()
    {
        if (!_isStarted) {
            return;
        }
        
        elapsed += Time.deltaTime;
        if (elapsed >= DevSettings.Instance.appSettings.timeBetweenUpdates) {
            elapsed = 0;
            Step();
        }
    }

    public void Step()
    {
        step += 1;
        FungusGrowth();
        ResourceTransport();
        ResourceGrowth();
        KillDeadCells();
        CDebug.Log(step);
    }

    private void FungusGrowth()
    {
        _fungusManager.GrowHyphas();
    }

    private void ResourceTransport()
    {
        _fungusManager.TransportResources();
    }

    private void KillDeadCells()
    {
        _fungusManager.RemoveDeadCells();
    }
    
    private void ResourceGrowth()
    {
        _knowledgeKeeper.PropagateResourceIncome();
    }
}
