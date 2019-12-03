using System;
using System.Collections.Generic;
using System.Linq;
using BRM.EventBrokers;
using BRM.EventBrokers.Interfaces;
using BRM.Sky.CustomWaveData;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class WaveEditorPrefabFactory : Singleton<WaveEditorPrefabFactory>
    {
        [Serializable]
        private class SpawnPrefabData
        {
            public SpawnPrefab SpawnPrefabType;
            public Sprite Sprite;
            [HideInInspector] public Transform Parent;
            public string PrefabName => SpawnPrefabType.ToString();
        }

        [SerializeField] private GameObject _editorBirdPrefab;
        [SerializeField] private List<SpawnPrefabData> _spawnPrefabList;

        private Dictionary<SpawnPrefab, SpawnPrefabData> _spawnPrefabDict;
        private IBrokerEvents _eventBroker = new StaticEventBroker();

        protected override bool _destroyOnLoad => true;

        protected override void Awake()
        {
            base.Awake();
            _spawnPrefabDict = _spawnPrefabList.ToDictionary(item=>item.SpawnPrefabType);
            InitializeSpawnPrefabHierarchy();
            _eventBroker.Subscribe<WaveEditorTestData>(OnWaveEditorStateChange);
        }

        private void OnDestroy()
        {
            _eventBroker.Unsubscribe<WaveEditorTestData>(OnWaveEditorStateChange);
        }

        private void OnWaveEditorStateChange(WaveEditorTestData data)
        {
            gameObject.SetActive(data.State == WaveEditorState.Editing);
        }

        private void InitializeSpawnPrefabHierarchy()
        {
            foreach (var kvp in _spawnPrefabDict)
            {
                var prefabName = kvp.Value.PrefabName;
                var hierarchyParent = new GameObject(prefabName);
                hierarchyParent.transform.SetParent(transform);
                kvp.Value.Parent = hierarchyParent.transform;
            }
        }

        public GameObject CreateInstance(SpawnPrefab prefabType)
        {
            if (_spawnPrefabDict.TryGetValue(prefabType, out var data))
            {
                var instance = Instantiate(_editorBirdPrefab, data.Parent);
                instance.GetComponent<SpriteRenderer>().sprite = GetSprite(prefabType);
                return instance;
            }

            Debug.LogError($"No EditorPrefab found for spawnPrefabType:{prefabType.ToString()}");
            return null;
        }

        public Sprite GetSprite(SpawnPrefab prefabType)
        {
            if (_spawnPrefabDict.TryGetValue(prefabType, out var data))
            {
                return data.Sprite;
            }

            Debug.LogError($"No Sprite found for spawnPrefabType:{prefabType.ToString()}");
            return null;
        }
    }
}