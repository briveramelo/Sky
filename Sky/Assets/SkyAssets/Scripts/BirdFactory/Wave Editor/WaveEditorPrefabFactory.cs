using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BRM.EventBrokers;
using BRM.EventBrokers.Interfaces;
using BRM.Sky.CustomWaveData;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class WaveEditorPrefabFactory : Singleton<WaveEditorPrefabFactory>
    {
        private class SpawnPrefabData
        {
            public SpawnPrefab SpawnPrefabType;
            public GameObject EditorPrefab;
            public Transform Parent;
            public string PrefabName => SpawnPrefabType.ToString();
        }

        [Serializable]
        private class SpawnPrefabSprite
        {
            public SpawnPrefab PrefabType;
            public Sprite Sprite;
        }

        [SerializeField] private GameObject _editorBirdPrefab;
        [SerializeField] private List<SpawnPrefabSprite> _birdSprites;

        private Dictionary<SpawnPrefab, SpawnPrefabData> _spawnPrefabData;
        private IBrokerEvents _eventBroker = new StaticEventBroker();

        protected override bool _destroyOnLoad => true;

        protected override void Awake()
        {
            base.Awake();
            InitializeSpawnPrefabData();
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

        private void InitializeSpawnPrefabData()
        {
            _spawnPrefabData = new Dictionary<SpawnPrefab, SpawnPrefabData>();
            var spawnPrefabTypes = EnumHelpers.GetAll<SpawnPrefab>().ToList();

            foreach (var spawnPrefabType in spawnPrefabTypes)
            {
                var spawnData = new SpawnPrefabData
                {
                    SpawnPrefabType = spawnPrefabType,
                    EditorPrefab = _editorBirdPrefab
                };
                _spawnPrefabData.Add(spawnPrefabType, spawnData);
            }
        }

        private void InitializeSpawnPrefabHierarchy()
        {
            foreach (var kvp in _spawnPrefabData)
            {
                var prefabName = kvp.Value.PrefabName;
                var hierarchyParent = new GameObject(prefabName);
                hierarchyParent.transform.SetParent(transform);
                kvp.Value.Parent = hierarchyParent.transform;
            }
        }

        public GameObject CreateInstance(SpawnPrefab prefabType)
        {
            if (_spawnPrefabData.TryGetValue(prefabType, out var data))
            {
                var instance = Instantiate(data.EditorPrefab, data.Parent);
                instance.GetComponent<SpriteRenderer>().sprite = GetSprite(prefabType);
                return instance;
            }

            Debug.LogError($"No EditorPrefab found for spawnPrefabType:{prefabType.ToString()}");
            return null;
        }

        public Sprite GetSprite(SpawnPrefab prefabType)
        {
            var birdSprite = _birdSprites.Find(bird => bird.PrefabType == prefabType);
            if (birdSprite != null)
            {
                return birdSprite.Sprite;
            }

            Debug.LogError($"No Sprite found for spawnPrefabType:{prefabType.ToString()}");
            return null;
        }
    }
}