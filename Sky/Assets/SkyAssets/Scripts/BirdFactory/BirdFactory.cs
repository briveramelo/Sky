using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GenericFunctions;
using Random = UnityEngine.Random;

public class BirdFactory : Singleton<BirdFactory>
{
    [System.Serializable]
    private struct BirdPrefab
    {
        public BirdType BirdType;
        public GameObject Prefab;
    }

    private class BirdData
    {
        public GameObject Prefab;
        public Transform Parent;
    }

    [SerializeField] private BirdPrefab[] _birdPrefabs;
    [SerializeField] private Transform _birdParentTransform;

    private List<IDeathDebug> _createdBirds = new List<IDeathDebug>();
    
    private Dictionary<BirdType, BirdData> _birdTypeData = new Dictionary<BirdType, BirdData>();
    protected override bool _destroyOnLoad => true;
    
    protected override void Awake()
    {
        base.Awake();
        InitializeBirdPrefabData();
    }

    private void InitializeBirdPrefabData()
    {
        foreach (var birdPrefab in _birdPrefabs)
        {
            var container = new GameObject(birdPrefab.BirdType.ToString());
            container.transform.SetParent(_birdParentTransform);
            
            _birdTypeData[birdPrefab.BirdType] = new BirdData
            {
                Prefab = birdPrefab.Prefab,
                Parent = container.transform
            };
        }
    }

    public void CreateNextBird(BirdType birdType)
    {
        var xSpot = -ScreenSpace.WorldEdge.x;
        var ySpot = Random.Range(-ScreenSpace.WorldEdge.y, ScreenSpace.WorldEdge.y) * 0.6f;
        if (birdType == BirdType.Tentacles || birdType == BirdType.Crow)
        {
            xSpot = 0f;
            ySpot = 0f;
        }
        else if (birdType == BirdType.Eagle)
        {
            xSpot = -ScreenSpace.WorldEdge.x * 5f;
        }
        else if (birdType == BirdType.Seagull)
        {
            xSpot = ScreenSpace.WorldEdge.x * (Bool.TossCoin() ? 1 : -1);
        }

        CreateBird(birdType, new Vector3(xSpot, ySpot));
    }

    public Bird CreateBird(BirdType birdType, Vector2 position)
    {
        var birdGameObject = Instantiate(_birdTypeData[birdType].Prefab, position, Quaternion.identity, _birdTypeData[birdType].Parent);
        var bird = birdGameObject.GetComponent<Bird>();
        _createdBirds.Add(bird);
        return bird;
    }

    public void KillAllLivingBirds()
    {
        _createdBirds.RemoveAll(bird => bird == null || bird as Bird == null);
        for (int i = 0; i < _createdBirds.Count; i++)
        {
            _createdBirds[i].KillDebug();
        }
    }
}