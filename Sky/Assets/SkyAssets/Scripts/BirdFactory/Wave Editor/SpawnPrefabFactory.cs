using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class SpawnPrefabFactory : Singleton<SpawnPrefabFactory>
    {
        private class SpawnPrefabData
        {
            public SpawnPrefab SpawnPrefabType;
            public GameObject Prefab;
            public Transform Parent;
            public string FilePath;
            public string PrefabName => SpawnPrefabType.ToString();

            public Sprite IconSprite
            {
                get
                {
                    if (_iconSprite == null)
                    {
                        //var iconTexture = AssetDatabase.GetCachedIcon(FilePath);
                        //var iconTexture = AssetPreview.GetMiniThumbnail(Prefab);
                        if (Prefab == null)
                        {
                            Debug.LogError($"prefab not yet set for type {SpawnPrefabType}");
                            return null;
                        }

                        var prefabSpriteRenderer = Prefab.GetComponent<SpriteRenderer>();
                        if (prefabSpriteRenderer == null)
                        {
                            prefabSpriteRenderer = Prefab.GetComponentInChildren<SpriteRenderer>();
                        }
                        if (prefabSpriteRenderer == null)
                        {
                            for (int i = 0; i < Prefab.transform.childCount; i++)
                            {
                                var rend = Prefab.transform.GetChild(i).GetComponent<SpriteRenderer>();
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

        private static Dictionary<SpawnPrefab, SpawnPrefabData> _spawnPrefabData;
        private string _prefabRootFolder => Path.Combine(Application.dataPath, "SkyAssets/Prefabs/Birds");

        protected override void Awake()
        {
            base.Awake();
            InitializeSpawnPrefabData();
            InitializeSpawnPrefabHierarchy();
        }

        private void InitializeSpawnPrefabData()
        {
            _spawnPrefabData = new Dictionary<SpawnPrefab, SpawnPrefabData>();
            var spawnPrefabs = Enum.GetValues(typeof(SpawnPrefab)).Cast<SpawnPrefab>().ToList();
            var prefabFolders = Directory.GetDirectories(_prefabRootFolder).ToList();
            prefabFolders.Insert(0, _prefabRootFolder);
            for (int i = 0; i < prefabFolders.Count; i++)
            {
                var startIndex = Application.dataPath.Length - "Assets/".Length + 1;
                var length = prefabFolders[i].Length - startIndex;
                prefabFolders[i] = prefabFolders[i].Substring(startIndex, length);
            }

            foreach (var spawnPrefab in spawnPrefabs)
            {
                bool fileFound = false;
                foreach (var prefabFolder in prefabFolders)
                {
                    var filePath = Path.Combine(prefabFolder, $"{spawnPrefab}.prefab");
                    if (!File.Exists(filePath))
                    {
                        continue;
                    }

                    _spawnPrefabData.Add(spawnPrefab, new SpawnPrefabData
                    {
                        SpawnPrefabType = spawnPrefab,
                        FilePath = filePath,
                        Prefab = AssetDatabase.LoadAssetAtPath<GameObject>(filePath)
                    });
                    fileFound = true;
                    break;
                }

                if (!fileFound)
                {
                    Debug.LogError($"No prefab found for type {spawnPrefab}");
                }
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

        public GameObject GetPrefab(SpawnPrefab prefabType)
        {
            if (_spawnPrefabData.TryGetValue(prefabType, out var data))
            {
                return data.Prefab;
            }

            Debug.LogError($"No Prefab found for spawnPrefabType:{prefabType}");
            return null;
        }

        public GameObject CreateInstance(SpawnPrefab prefabType)
        {
            if (_spawnPrefabData.TryGetValue(prefabType, out var data))
            {
                var instance = Instantiate(data.Prefab, data.Parent);
                return instance;
            }

            Debug.LogError($"No Prefab found for spawnPrefabType:{prefabType}");
            return null;
        }

        public Sprite GetSprite(SpawnPrefab prefabType)
        {
            if (_spawnPrefabData.TryGetValue(prefabType, out var data))
            {
                return data.IconSprite;
            }

            Debug.LogError($"No Sprite found for spawnPrefabType:{prefabType}");
            return null;
        }
    }
}