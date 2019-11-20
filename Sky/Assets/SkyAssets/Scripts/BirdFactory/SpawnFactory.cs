using System;
using System.Collections.Generic;
using System.Linq;
using BRM.Sky.CustomWaveData;
using UnityEngine;

public class SpawnFactory : Singleton<SpawnFactory>
{
    [Serializable]
    private class SpawnPrefabData
    {
        public SpawnPrefab PrefabType;
        public GameObject Prefab;
        [HideInInspector] public Transform Parent;
    }

    [SerializeField] private List<SpawnPrefabData> _spawnPrefabs;
    
    private Dictionary<SpawnPrefab, SpawnPrefabData> _spawnPrefabData;
    
    protected override bool _destroyOnLoad => true;

    protected override void Awake()
    {
        base.Awake();
        InitializeSpawnPrefabData();
        InitializeSpawnPrefabHierarchy();
    }

    private void InitializeSpawnPrefabData()
    {
        _spawnPrefabData = _spawnPrefabs.ToDictionary(spawn => spawn.PrefabType, spawn => spawn);
    }
    
    private void InitializeSpawnPrefabHierarchy()
    {
        foreach (var kvp in _spawnPrefabData)
        {
            var prefabName = kvp.Value.Prefab.name;
            var hierarchyParent = new GameObject(prefabName);
            hierarchyParent.transform.SetParent(transform);
            kvp.Value.Parent = hierarchyParent.transform;
        }
    }

    public GameObject CreateInstance(SpawnPrefab prefabType)
    {
        if (_spawnPrefabData.TryGetValue(prefabType, out var data))
        {
            var instance = Instantiate(data.Prefab, data.Parent);
            return instance;
        }

        Debug.LogError($"No EditorPrefab found for spawnPrefabType:{prefabType}");
        return null;
    }
}