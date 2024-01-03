using System.Collections;
using System.Collections.Generic;
using Fungus;
using Utils.Singleton;
using UnityEngine;
using Utils;

[RequireComponent(typeof(DoNotDestroy))]
public class GameManager : Singleton<GameManager>
{
    private KnowledgeKeeper _knowledgeKeeper = KnowledgeKeeper.Instance;
    private FungusManager _fungusManager = FungusManager.Instance;

    public void Step()
    {
        FungusGrowth();
        ResourceTransport();
        ResourceGrowth();
    }

    private void FungusGrowth()
    {
        _fungusManager.GrowStrobenkopers();
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
