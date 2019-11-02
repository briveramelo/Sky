using UnityEngine;
using GenericFunctions;

public class BirdOfParadise : LinearBird
{
    [SerializeField] private GameObject _balloon;
    
    protected override BirdType MyBirdType => BirdType.BirdOfParadise;
    
    protected override void Awake()
    {
        MoveSpeed = 3f;
        base.Awake();
        Destroy(gameObject, 10f);
    }

    protected override void OnDeath()
    {
        SpawnBalloon();
        base.OnDeath();
    }

    private void SpawnBalloon()
    {
        var xSpot = Random.Range(-Constants.ScreenSizeWorldUnits.x, Constants.ScreenSizeWorldUnits.x) * 0.67f;
        var spawnSpot = new Vector3(xSpot, -Constants.ScreenSizeWorldUnits.y * 1.6f, 0f);
        Instantiate(_balloon, spawnSpot, Quaternion.identity);
    }
}