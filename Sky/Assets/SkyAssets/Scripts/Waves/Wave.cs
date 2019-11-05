using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GenericFunctions;
using System;
using UnityEngine.SceneManagement;

public interface IWaveRunnable
{
    IEnumerator RunWave();
    WaveName MyWave { get; }
}

public delegate void SpawnDelegate();

public abstract class Wave : MonoBehaviour, IWaveRunnable
{
    [SerializeField] private WaveName _myWaveName;
    WaveName IWaveRunnable.MyWave => _myWaveName;

    private IWaveUi waveUi;
    private IWaveUi _waveUi => waveUi ?? FindObjectOfType<WaveUi>().GetComponent<IWaveUi>();

    protected BirdWaiter AllDead = new BirdWaiter(CounterType.Alive, false, 0, BirdType.All);
    protected BirdWaiter AllDeadExceptTentacles = new BirdWaiter(CounterType.Alive, true, 0, BirdType.Tentacles);

    protected static int WaveNumber;
    protected const float LowHeight = -0.6f;
    protected const float MedHeight = 0f;
    protected const float HighHeight = 0.6f;
    protected static float[] Heights;
    protected Vector2[] DuckSpawnPoints;
    protected Dictionary<BirdType, SpawnDelegate> BirdSpawnDelegates = new Dictionary<BirdType, SpawnDelegate>();

    protected const bool Right = true;
    protected const bool Left = false;

    #region SpawnDelegates

    protected static SpawnDelegate SpawnAtRandom(BirdType birdType)
    {
        return () =>
        {
            Vector2 spawnPoint;
            if (birdType == BirdType.DuckLeader)
            {
                spawnPoint = SpawnPoint(Bool.TossCoin(), .5f * LowHeight, .5f * HighHeight);
            }
            else if (birdType == BirdType.Tentacles || birdType == BirdType.Crow)
            {
                spawnPoint = Vector2.zero;
            }
            else
            {
                spawnPoint = SpawnPoint(Bool.TossCoin(), LowHeight, HighHeight);
            }

            SpawnBirds(birdType, spawnPoint, (DuckDirection) UnityEngine.Random.Range(0, Enum.GetNames(typeof(DuckDirection)).Length));
        };
    }


    #endregion

    private void Start()
    {
        Heights = new[] {LowHeight, MedHeight, HighHeight};
        DuckSpawnPoints = new Vector2[6];
        for (var i = 0; i < 6; i++)
        {
            DuckSpawnPoints[i] = SpawnPoint(i % 2 == 0, Heights[Mathf.FloorToInt(i / 2)]);
        }

        for (var i = 0; i < Enum.GetNames(typeof(BirdType)).Length - 1; i++)
        {
            BirdSpawnDelegates.Add((BirdType) i, SpawnAtRandom((BirdType) i));
        }
    }

    IEnumerator IWaveRunnable.RunWave()
    {
        Debug.Log("running wave");
        yield return StartCoroutine(StartWave());
        Debug.Log("generating birds");
        yield return StartCoroutine(GenerateBirds());
        Debug.Log("finishing wave");
        yield return StartCoroutine(FinishWave());
    }

    private IEnumerator StartWave()
    {
        yield return StartCoroutine(_waveUi.AnimateWaveStart(_myWaveName));
    }

    protected virtual IEnumerator GenerateBirds()
    {
        yield return null;
    }

    private IEnumerator FinishWave()
    {
        yield return new WaitForSeconds(2f);
        SpawnBirds(BirdType.BirdOfParadise, SpawnPoint(Right, LowHeight));
        yield return StartCoroutine(WaitFor(AllDeadExceptTentacles, true));
        yield return StartCoroutine(_waveUi.AnimateWaveEnd(_myWaveName));
        WaveNumber++;
        ScoreSheet.Resetter.ResetWaveCounters();
    }

    /// <summary> Spawn Birds
    /// </summary>
    protected static void SpawnBirds(BirdType birdType, Vector2 spawnPoint, DuckDirection duckDir = DuckDirection.UpRight)
    {
        var direction = spawnPoint.x < 0 ? 1 : -1;

        if (birdType == BirdType.Eagle)
        {
            spawnPoint = new Vector2(-ScreenSpace.WorldEdge.x * 5f, 0f);
        }

        var bird = BirdFactory.Instance.CreateBird(birdType, spawnPoint);

        if (birdType == BirdType.Pigeon || birdType == BirdType.BirdOfParadise)
        {
            var linearBirdScript = (LinearBird) bird;
            linearBirdScript.SetVelocity(Vector2.right * direction);
        }
        else if (birdType == BirdType.Duck)
        {
            var duckScript = (IDirectable) bird;
            duckScript.SetDuckDirection(duckDir);
        }
    }

    /// <summary> side is +/- 1 and y position is between -1 <-> +1
    /// Multiplies these input numbers by worldDimensions
    /// </summary>
    protected static Vector2 SpawnPoint(bool startOnRight, float y1, float y2 = -1337f)
    {
        y2 = y2 == -1337f ? y1 : y2;
        y1 = Mathf.Clamp(y1, -1f, 1f);
        y2 = Mathf.Clamp(y2, -1f, 1f);
        return new Vector2((startOnRight ? 1 : -1) * ScreenSpace.WorldEdge.x, UnityEngine.Random.Range(y1, y2) * ScreenSpace.WorldEdge.y);
    }

    protected IEnumerator MassProduce(SpawnDelegate spawn, int birdCount)
    {
        for (var i = 0; i < birdCount; i++)
        {
            spawn();
            yield return new WaitForSeconds(1f);
        }
    }

    protected IEnumerator ProduceDucks(int numDucks)
    {
        var duckSpawnList = new List<Vector2>(DuckSpawnPoints);
        for (var i = 0; i < numDucks; i++)
        {
            yield return new WaitForSeconds(1f);
            var chosenPoint = UnityEngine.Random.Range(0, duckSpawnList.Count);
            SpawnBirds(BirdType.Duck, duckSpawnList[chosenPoint], DuckDirectionGenerator(duckSpawnList[chosenPoint]));
            duckSpawnList.RemoveAt(chosenPoint);
            if (duckSpawnList.Count == 0)
            {
                duckSpawnList = new List<Vector2>(DuckSpawnPoints);
            }
        }
    }

    /// <summary> Will output DuckDirection based on duck's spawning position
    /// </summary>
    protected DuckDirection DuckDirectionGenerator(Vector2 spawnPoint)
    {
        if (spawnPoint.y == 0)
        {
            var goUp = Bool.TossCoin();
            return spawnPoint.x > 0 ? goUp ? DuckDirection.UpLeft : DuckDirection.DownLeft : goUp ? DuckDirection.UpRight : DuckDirection.DownRight;
        }

        if (spawnPoint.y > 0)
        {
            return spawnPoint.x > 0 ? DuckDirection.DownLeft : DuckDirection.DownRight;
        }

        return spawnPoint.x > 0 ? DuckDirection.UpLeft : DuckDirection.UpRight;
    }

    protected IEnumerator FlyPigeonsAsDuckLeader()
    {
        SpawnBirds(BirdType.Pigeon, SpawnPoint(Right, 0));
        for (var i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(.5f);
            SpawnBirds(BirdType.Pigeon, SpawnPoint(Right, .1f * (i + 1)));
            SpawnBirds(BirdType.Pigeon, SpawnPoint(Right, -.1f * (i + 1)));
        }

        SpawnBirds(BirdType.DuckLeader, SpawnPoint(Right, 0));
    }

    protected IEnumerator Produce1Wait3(SpawnDelegate spawn)
    {
        spawn();
        yield return StartCoroutine(WaitFor(AllDeadExceptTentacles, true));
        yield return StartCoroutine(MassProduce(spawn, 3));
        yield return StartCoroutine(WaitFor(AllDeadExceptTentacles, true));
    }

    /// <summary> Wait until at most "birdsRemaining" "birdTypes" remain alive on screen
    /// </summary>
    protected IEnumerator WaitFor(BirdWaiter birdWaiter, bool waitExtra = false)
    {
        while (birdWaiter.Wait)
        {
            yield return null;
        }

        FinishWaiting(birdWaiter);
        if (waitExtra)
        {
            yield return StartCoroutine(WaitUntilTimeRange());
        }
    }

    /// <summary> Wait between minTime and maxTime seconds
    /// </summary>
    protected IEnumerator WaitUntilTimeRange(float minTime = 1f, float maxTime = 2f)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(minTime, maxTime));
    }

    protected IEnumerator WaitInParallel(params BirdWaiter[] birdWaiters)
    {
        var birdWaitList = new List<BirdWaiter>(birdWaiters);
        while (birdWaitList.Count > 0)
        {
            for (var i = 0; i < birdWaitList.Count; i++)
            {
                if (!birdWaitList[i].Wait)
                {
                    FinishWaiting(birdWaitList[i]);
                    birdWaitList.Remove(birdWaitList[i]);
                }
            }

            yield return null;
        }
    }

    private void FinishWaiting(BirdWaiter birdWaiter)
    {
        if (birdWaiter.Perform != null)
        {
            StartCoroutine(birdWaiter.Perform);
        }
        else
        {
            if (birdWaiter.Spawn != null)
            {
                birdWaiter.Spawn();
            }
        }
    }
}