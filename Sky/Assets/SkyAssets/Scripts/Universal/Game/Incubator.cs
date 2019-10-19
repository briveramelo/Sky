using System.Collections.Generic;
using UnityEngine;
using GenericFunctions;

public class Incubator : Singleton<Incubator>
{
    [System.Serializable]
    private struct BirdPrefab
    {
        public BirdType BirdType;
        public GameObject Prefab;
    }

    [SerializeField] private BirdPrefab[] _birdPrefabs;
    public Dictionary<BirdType, GameObject> BirdPrefabs { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        BirdPrefabs = new Dictionary<BirdType, GameObject>();
        foreach (var birdPrefab in _birdPrefabs)
        {
            BirdPrefabs.Add(birdPrefab.BirdType, birdPrefab.Prefab);
        }
    }

    public void SpawnNextBird(BirdType birdType)
    {
        var xSpot = -Constants.WorldDimensions.x;
        var ySpot = Random.Range(-Constants.WorldDimensions.y, Constants.WorldDimensions.y) * 0.6f;
        if (birdType == BirdType.Tentacles || birdType == BirdType.Crow)
        {
            xSpot = 0f;
            ySpot = 0f;
        }
        else if (birdType == BirdType.Eagle)
        {
            xSpot = -Constants.WorldDimensions.x * 5f;
        }
        else if (birdType == BirdType.Seagull)
        {
            xSpot = Constants.WorldDimensions.x * (Bool.TossCoin() ? 1 : -1);
        }

        Instantiate(BirdPrefabs[birdType], new Vector3(xSpot, ySpot, 0f), Quaternion.identity);
    }
}