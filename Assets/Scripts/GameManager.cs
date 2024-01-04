using System;
using System.Collections;
using System.Collections.Generic;
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
    float elapsed = 0f;

    private void Start()
    {
        _knowledgeKeeper = KnowledgeKeeper.Instance;
        _fungusManager = FungusManager.Instance;
    }

    public void Init()
    {
        SimulationInitializer.GenerateRandomMoisture(_knowledgeKeeper.MoistureMap);
        SimulationInitializer.SpawnFungalInCenter(_fungusManager);
        SimulationInitializer.SpawnResourceIncomeInCenter(_knowledgeKeeper);
        SimulationInitializer.GenerateRandomResourceIncome(_knowledgeKeeper.ResourceIncomeMap);
        _isStarted = true;
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
        FungusGrowth();
        ResourceTransport();
        ResourceGrowth();
    }

    private void FungusGrowth()
    {
        _fungusManager.GrowHyphas();
    }

    private void ResourceTransport()
    {
        _fungusManager.TransportResources();
    }
    
    private void ResourceGrowth()
    {
        _knowledgeKeeper.PropagateResourceIncome();
    }
}
