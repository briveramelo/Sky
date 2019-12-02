using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BRM.EventBrokers;
using BRM.EventBrokers.Interfaces;
using BRM.Sky.CustomWaveData;
using BRM.Sky.WaveEditor;
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
    private List<BatchData> _customBatchData = new List<BatchData>();
    private IBrokerEvents _eventBroker = new StaticEventBroker();

    protected override void Awake()
    {
        base.Awake();
        InitializeSpawnPrefabData();
        InitializeSpawnPrefabHierarchy();

        _eventBroker.Subscribe<WaveEditorTestData>(OnWaveEditorStateChange);
        _eventBroker.Subscribe<WaveEditorSliderData>(OnWaveEditorSliderChange);
        _eventBroker.Subscribe<BatchSavedData>(OnBatchSaved);
        OnBatchSaved(null);
    }

    private void OnBatchSaved(BatchSavedData data)
    {
        _customBatchData = CustomBatchDataLoader.GetCustomBatchData();
    }

    private void OnDestroy()
    {
        _eventBroker.Unsubscribe<WaveEditorTestData>(OnWaveEditorStateChange);
        _eventBroker.Unsubscribe<WaveEditorSliderData>(OnWaveEditorSliderChange);
        _eventBroker.Unsubscribe<BatchSavedData>(OnBatchSaved);
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

        Debug.LogError($"No EditorPrefab found for spawnPrefabType:{prefabType.ToString()}");
        return null;
    }

    public IEnumerator GenerateBatch(BatchData batchData)
    {
        var batchStartTime = Time.time;
        float GetTimeAfterBatchStart()
        {
            return Time.time - batchStartTime;
        }

        var spawnEventData = batchData.SpawnEventData;
        var spawnCount = spawnEventData.Count;
        spawnEventData = spawnEventData.OrderBy(spawnEvent => spawnEvent.TimeAfterBatchStartSec).ToList();
        for (int i = 0; i < spawnCount; i++)
        {
            var spawnEvent = spawnEventData[i];
            var waitTime = GetTimeAfterBatchStart() - spawnEvent.TimeAfterBatchStartSec;
            if (!Mathf.Approximately(0, waitTime))
            {
                yield return new WaitForSeconds(waitTime);
            }

            if (TryExtractCustomBatchData(spawnEvent, out var customBatch))
            {
                yield return StartCoroutine(GenerateBatch(customBatch));
            }
            else
            {
                var instance = CreateInstance(spawnEvent.SpawnPrefab);
                instance.transform.position = spawnEvent.NormalizedPosition.ViewportToWorldPosition();
            }
        }
    }

    private bool TryExtractCustomBatchData(SpawnEventData spawnEventData, out BatchData batchData)
    {
        batchData = null;
        if (spawnEventData.SpawnPrefab == SpawnPrefab.Batch)
        {
            batchData = _customBatchData.Find(batch => batch.Name == spawnEventData.BatchName);
            if (batchData == null)
            {
                Debug.LogErrorFormat("No custom batch found for batch name:{0}", spawnEventData.BatchName);
                return false;
            }

            return true;
        }

        return false;
    }

    #region Wave Editor Support
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
        var aliveBirds = FindObjectsOfType<Bird>();
        for (int i = 0; i < aliveBirds.Length; i++)
        {
            aliveBirds[i].KillDebug();
        }
    }
    #endregion
}