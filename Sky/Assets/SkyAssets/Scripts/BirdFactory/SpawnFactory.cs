using System;
using System.Collections.Generic;
using System.Linq;
using BRM.EventBrokers;
using BRM.EventBrokers.Interfaces;
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

    protected override bool _destroyOnLoad => true;

    private Dictionary<SpawnPrefab, SpawnPrefabData> _spawnPrefabData;
    private List<GameObject> _spawnedInstances = new List<GameObject>();
    private IBrokerEvents _eventBroker = new StaticEventBroker();


    protected override void Awake()
    {
        base.Awake();
        InitializeSpawnPrefabData();
        InitializeSpawnPrefabHierarchy();

        _eventBroker.Subscribe<WaveEditorTestData>(OnWaveEditorStateChange);
        _eventBroker.Subscribe<WaveEditorSliderData>(OnWaveEditorSliderChange);
    }

    private void OnDestroy()
    {
        _eventBroker.Unsubscribe<WaveEditorTestData>(OnWaveEditorStateChange);
        _eventBroker.Unsubscribe<WaveEditorSliderData>(OnWaveEditorSliderChange);
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
            _spawnedInstances.Add(instance);
            return instance;
        }

        Debug.LogError($"No EditorPrefab found for spawnPrefabType:{prefabType}");
        return null;
    }

    private void OnWaveEditorStateChange(WaveEditorTestData data)
    {
        if (data.State == WaveEditorState.Editing)
        {
            DestroyAllBirds();
        }
    }

    private void OnWaveEditorSliderChange(WaveEditorSliderData data)
    {
        DestroyAllBirds();
    }

    private void DestroyAllBirds()
    {
        for (int i = 0; i < _spawnedInstances.Count; i++)
        {
            var instance = _spawnedInstances[i];
            if (instance != null)
            {
                var deathDebug = instance.GetComponentInChildren<IDeathDebug>(true);
                if (deathDebug != null && !ReferenceEquals(null, deathDebug))
                {
                    deathDebug.KillDebug();
                }
            }
        }

        _spawnedInstances.Clear();
    }
}