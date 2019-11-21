using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BRM.EventBrokers;
using BRM.EventBrokers.Interfaces;
using BRM.Sky.CustomWaveData;
using BRM.UnityAssets;
using BRM.UnityAssets.Editor;
using BRM.UnityAssets.Interfaces;
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

            public Sprite IconSprite
            {
                get
                {
                    if (_iconSprite == null)
                    {
                        if (EditorPrefab == null)
                        {
                            Debug.LogError($"prefab not yet set for type {SpawnPrefabType}");
                            return null;
                        }

                        var prefabSpriteRenderer = EditorPrefab.GetComponent<SpriteRenderer>();
                        if (prefabSpriteRenderer == null)
                        {
                            prefabSpriteRenderer = EditorPrefab.GetComponentInChildren<SpriteRenderer>();
                        }

                        if (prefabSpriteRenderer == null)
                        {
                            for (int i = 0; i < EditorPrefab.transform.childCount; i++)
                            {
                                var rend = EditorPrefab.transform.GetChild(i).GetComponent<SpriteRenderer>();
                                if (rend == null)
                                {
                                    continue;
                                }

                                prefabSpriteRenderer = rend;
                                break;
                            }
                        }

                        if (prefabSpriteRenderer == null)
                        {
                            Debug.LogError($"No sprite renderer found on prefabType:{SpawnPrefabType}");
                            return null;
                        }

                        var prefabTex = prefabSpriteRenderer.sprite.texture;
                        var iconTexture = prefabTex;

                        var iconSize = new Vector2(iconTexture.width, iconTexture.height);
                        var sprite = Sprite.Create(new Texture2D((int) iconSize.x, (int) iconSize.y), new Rect(Vector2.zero, iconSize), Vector2.one * 0.5f);
                        _iconSprite = sprite;
                    }

                    return _iconSprite;
                }
            }

            private Sprite _iconSprite;
        }

        [Serializable]
        private class SpawnPrefabSprite
        {
            public SpawnPrefab PrefabType;
            public Sprite Sprite;
        }

        [SerializeField] private List<SpawnPrefabSprite> _birdSprites;

        private SimpleAssetLoader<GameObject> _prefabLoader;
        
        private Dictionary<SpawnPrefab, SpawnPrefabData> _spawnPrefabData;
        private IBrokerEvents _eventBroker = new StaticEventBroker();

        protected override bool _destroyOnLoad => true;

        protected override void Awake()
        {
            base.Awake();
        #if UNITY_EDITOR
            _prefabLoader = new AssetDatabaseLoader<GameObject>();
        #else
            _prefabLoader = null;
        #endif
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
            var spawnPrefabs = EnumHelpers.GetAll<SpawnPrefab>().ToList();

            foreach (var spawnPrefab in spawnPrefabs)
            {
                var spawnData = new SpawnPrefabData
                {
                    SpawnPrefabType = spawnPrefab,
                };
                var prefabLoadingData = new AssetData<GameObject>
                {
                    OnAssetLoaded = asset => spawnData.EditorPrefab = asset,
                    Name = "EditorBird",
                    ContainerName = "SkyAssets/Prefabs/Birds/Editor/",
                    FileExtension = "prefab"
                };
                if (!File.Exists($"{Application.dataPath}/{prefabLoadingData.CombinedPath}"))
                {
                    Debug.LogError($"No prefab found for type {spawnPrefab}");
                    continue;
                }
                
                _prefabLoader.Load(prefabLoadingData);
                _spawnPrefabData.Add(spawnPrefab, spawnData);
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

            Debug.LogError($"No EditorPrefab found for spawnPrefabType:{prefabType}");
            return null;
        }

        public Sprite GetSprite(SpawnPrefab prefabType)
        {
            var birdSprite = _birdSprites.Find(bird => bird.PrefabType == prefabType);
            if (birdSprite != null)
            {
                return birdSprite.Sprite;
            }

            Debug.LogError($"No Sprite found for spawnPrefabType:{prefabType}");
            return null;
        }
    }
}