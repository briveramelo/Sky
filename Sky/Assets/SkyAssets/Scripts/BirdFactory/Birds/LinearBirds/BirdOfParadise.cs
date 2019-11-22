using UnityEngine;
using GenericFunctions;

public class BirdOfParadise : LinearBird
{
    [SerializeField] private GameObject _balloon;
    
    protected override BirdType MyBirdType => BirdType.BirdOfParadise;
    protected override float MoveSpeed => 3f;
    
    protected override void Start()
    {
        base.Start();
        Destroy(gameObject, 10f);
    }

    protected override void OnDeath()
    {
        SpawnBalloon();
        base.OnDeath();
    }

    private void SpawnBalloon()
    {
        var xSpot = Random.Range(-ScreenSpace.WorldEdge.x, ScreenSpace.WorldEdge.x) * 0.67f;
        var spawnSpot = new Vector3(xSpot, -ScreenSpace.WorldEdge.y - 0.6f, 0f);
        Instantiate(_balloon, spawnSpot, Quaternion.identity);
    }
}