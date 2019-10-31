using System.Collections.Generic;
using UnityEngine;
using GenericFunctions;

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
    private Dictionary<BirdType, BirdData> _birdTypeData = new Dictionary<BirdType, BirdData>();
    protected override bool _destroyOnLoad => true;

    protected override void Awake()
    {
        base.Awake();
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
        var xSpot = -Constants.WorldSize.x;
        var ySpot = Random.Range(-Constants.WorldSize.y, Constants.WorldSize.y) * 0.6f;
        if (birdType == BirdType.Tentacles || birdType == BirdType.Crow)
        {
            xSpot = 0f;
            ySpot = 0f;
        }
        else if (birdType == BirdType.Eagle)
        {
            xSpot = -Constants.WorldSize.x * 5f;
        }
        else if (birdType == BirdType.Seagull)
        {
            xSpot = Constants.WorldSize.x * (Bool.TossCoin() ? 1 : -1);
        }

        CreateBird(birdType, new Vector3(xSpot, ySpot));
    }

    public Bird CreateBird(BirdType birdType, Vector2 position)
    {
        var birdGameObject = Instantiate(_birdTypeData[birdType].Prefab, position, Quaternion.identity, _birdTypeData[birdType].Parent);
        var bird = birdGameObject.GetComponent<Bird>();
        return bird;
    }
}