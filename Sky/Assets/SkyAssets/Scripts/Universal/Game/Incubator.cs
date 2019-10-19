using UnityEngine;
using GenericFunctions;

public class Incubator : Singleton<Incubator>
{
    [SerializeField] private GameObject[] _birds;
    public GameObject[] Birds => _birds;

    private void Pigeon()
    {
        SpawnNextBird(BirdType.Pigeon);
    }

    private void Duck()
    {
        SpawnNextBird(BirdType.Duck);
    }

    private void DuckLeader()
    {
        SpawnNextBird(BirdType.DuckLeader);
    }

    private void Albatross()
    {
        SpawnNextBird(BirdType.Albatross);
    }

    private void BabyCrow()
    {
        SpawnNextBird(BirdType.BabyCrow);
    }

    private void Crow()
    {
        SpawnNextBird(BirdType.Crow);
    }

    private void Tentacles()
    {
        SpawnNextBird(BirdType.Tentacles);
    }

    private void Seagull()
    {
        SpawnNextBird(BirdType.Seagull);
    }

    private void Pelican()
    {
        SpawnNextBird(BirdType.Pelican);
    }

    private void Shoebill()
    {
        SpawnNextBird(BirdType.Shoebill);
    }

    private void Bat()
    {
        SpawnNextBird(BirdType.Bat);
    }

    private void Eagle()
    {
        SpawnNextBird(BirdType.Eagle);
    }

    private void BirdOfParadise()
    {
        SpawnNextBird(BirdType.BirdOfParadise);
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

        Instantiate(_birds[(int) birdType], new Vector3(xSpot, ySpot, 0f), Quaternion.identity);
    }
}