using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GenericFunctions;

public class DuckWave : Wave
{
    private float _duckSinPeriod => ScreenSpace.ScreenSizeWorldUnits.x;

    public override IEnumerator GenerateBirds()
    {
        yield return StartCoroutine(DuckSine(true, .5f, 0f, DuckDirection.UpRight));
        yield return StartCoroutine(WaitFor(AllDead));

        SpawnBirds(BirdType.DuckLeader, SpawnPoint(Right, 0f));
        var oneDeadLeader = new BirdWaiter(BirdCounterType.BirdsKilled, false, 1, BirdType.DuckLeader);
        yield return StartCoroutine(WaitFor(oneDeadLeader));
        SpawnBirds(BirdType.DuckLeader, SpawnPoint(Left, 0f));
        oneDeadLeader = new BirdWaiter(BirdCounterType.BirdsKilled, false, 1, BirdType.DuckLeader);
        yield return StartCoroutine(WaitFor(oneDeadLeader));
        var topIsRight = Bool.TossCoin();
        SpawnBirds(BirdType.DuckLeader, SpawnPoint(topIsRight, HighHeight));
        SpawnBirds(BirdType.DuckLeader, SpawnPoint(!topIsRight, LowHeight));
        yield return StartCoroutine(WaitFor(AllDead));

        var wallOnRight = Bool.TossCoin();
        var firstDuckDir = wallOnRight ? DuckDirection.UpLeft : DuckDirection.UpRight;
        var secondDuckDir = !wallOnRight ? DuckDirection.DownLeft : DuckDirection.DownRight;

        StartCoroutine(DuckWall(wallOnRight, new Range(-.75f, -0.25f), firstDuckDir));
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(DuckWall(wallOnRight, new Range(-.75f, 0.75f), firstDuckDir));
        yield return StartCoroutine(WaitFor(AllDead));

        StartCoroutine(DuckWall(wallOnRight, new Range(-.25f, 0.25f), firstDuckDir));
        StartCoroutine(DuckWall(!wallOnRight, new Range(-.25f, 0.25f), secondDuckDir));
        yield return new WaitForSeconds(2f);
        StartCoroutine(DuckWall(wallOnRight, new Range(-.75f, 0.75f), firstDuckDir));
        yield return StartCoroutine(DuckWall(!wallOnRight, new Range(-.75f, 0.75f), secondDuckDir));
        yield return StartCoroutine(WaitFor(AllDead));


        yield return StartCoroutine(DuckSine(wallOnRight, 0.3f, 0f, firstDuckDir));
        yield return StartCoroutine(WaitFor(AllDead));

        yield return StartCoroutine(DuckSlantWall(!wallOnRight, true, new Range(-.75f, .75f), 0.1f, secondDuckDir));
        yield return StartCoroutine(WaitFor(AllDead));
    }

    #region Alternating Ducks

    /*
     * ->
     *         <-
     * ->
     *         <-
     */
    private IEnumerator AlternatingDucks(int numDucks)
    {
        float[] spawnOpts =
        {
            .9f, 0.85f, .8f, 0.75f, .7f, 0.65f, .6f, 0.55f, .5f, 0.45f, .4f, 0.35f, .3f, 0.25f, 0.2f, 0.15f, .1f, 0.05f, 0f,
            -.9f, -0.85f, -.8f, -0.75f, -.7f, -0.65f, -.6f, -0.55f, -.5f, -0.45f, -.4f, -0.35f, -.3f, -0.25f, -0.2f, -0.15f, -.1f, -0.05f
        };
        var spawnOptions = new List<float>(spawnOpts);
        var side = Bool.TossCoin();
        for (var i = 0; i < numDucks; i++)
        {
            var spawnNum = Random.Range(0, spawnOptions.Count);
            var spawnSpot = SpawnPoint(side, spawnOptions[spawnNum]);
            SpawnBirds(BirdType.Duck, spawnSpot, DuckDirectionGenerator(spawnSpot));
            spawnOptions.Remove(spawnOptions[spawnNum]);
            side = !side;
            if (spawnOptions.Count == 0)
            {
                spawnOptions = new List<float>(spawnOpts);
            }

            yield return new WaitForSeconds(0.25f);
        }
    }

    #endregion

    #region Duck Meatball

    /*
       0 0
      0 0 0
       0 0
    */
    private IEnumerator DuckMeatball(bool startOnRight, float midPoint, DuckDirection duckDir)
    {
        var waitTime = 0.285f;
        var hexHeight = 0.17f;
        var waitedTime = 0f;
        var waitHeight = (duckDir == DuckDirection.UpRight || duckDir == DuckDirection.UpLeft ? 1 : -1) * .6f;
        var i = 0;
        var startTime = Time.time;
        SpawnBirds(BirdType.Duck, SpawnPoint(startOnRight, midPoint + hexHeight / 2), duckDir);
        SpawnBirds(BirdType.Duck, SpawnPoint(startOnRight, midPoint - hexHeight / 2), duckDir);
        yield return new WaitForSeconds(waitTime);
        i++;
        waitedTime = Time.time - startTime;
        startTime = Time.time;
        SpawnBirds(BirdType.Duck, SpawnPoint(startOnRight, midPoint + waitedTime * waitHeight * i), duckDir);
        SpawnBirds(BirdType.Duck, SpawnPoint(startOnRight, midPoint + hexHeight + waitedTime * waitHeight * i), duckDir);
        SpawnBirds(BirdType.Duck, SpawnPoint(startOnRight, midPoint - hexHeight + waitedTime * waitHeight * i), duckDir);
        yield return new WaitForSeconds(waitTime);
        i++;
        waitedTime = Time.time - startTime;
        SpawnBirds(BirdType.Duck, SpawnPoint(startOnRight, midPoint + hexHeight / 2 + waitedTime * waitHeight * i), duckDir);
        SpawnBirds(BirdType.Duck, SpawnPoint(startOnRight, midPoint - hexHeight / 2 + waitedTime * waitHeight * i), duckDir);
    }

    #endregion

    #region Duck Wall

    /*
    -
    -
    -
    */
    private IEnumerator DuckWall(bool startOnRight, Range heightRange, DuckDirection duckDir)
    {
        heightRange.Max = Mathf.Clamp(heightRange.Max, -1f, 1f);
        heightRange.Min = Mathf.Clamp(heightRange.Min, -1f, 1f);
        var separationHeight = 0.075f;
        for (var spawnHeight = heightRange.Min; spawnHeight < heightRange.Max; spawnHeight += separationHeight)
        {
            SpawnBirds(BirdType.Duck, SpawnPoint(startOnRight, spawnHeight), duckDir);
        }

        yield return null;
    }

    #endregion

    #region Duck Slant Wall

    /*
     *    -
     *   -
     *  -
     * -
     */
    private IEnumerator DuckSlantWall(bool startOnRight, bool leadOnBottom, Range heightRange, float waitTime, DuckDirection duckDir)
    {
        heightRange.Min = Mathf.Clamp(heightRange.Min, -1f, 1f);
        heightRange.Max = Mathf.Clamp(heightRange.Max, -1f, 1f);
        var separationHeight = 0.075f;
        if (leadOnBottom)
        {
            for (var spawnHeight = heightRange.Min; spawnHeight < heightRange.Max; spawnHeight += separationHeight)
            {
                SpawnBirds(BirdType.Duck, SpawnPoint(startOnRight, spawnHeight), duckDir);
                yield return new WaitForSeconds(waitTime);
            }
        }
        else
        {
            for (var spawnHeight = heightRange.Max; spawnHeight > heightRange.Min; spawnHeight -= separationHeight)
            {
                SpawnBirds(BirdType.Duck, SpawnPoint(startOnRight, spawnHeight), duckDir);
                yield return new WaitForSeconds(waitTime);
            }
        }
    }

    #endregion

    #region Duck Sine

    /*
     *     -      
     *   -   -   -
     *         -  
     */
    private IEnumerator DuckSine(bool onRight, float sineAmp, float midPoint, DuckDirection duckDir)
    {
        var numDucks = 20;
        var waitTime = _duckSinPeriod / numDucks;
        for (var i = 0; i < numDucks; i++)
        {
            var ySpawnHeight = sineAmp * Mathf.Sin(2 * Mathf.PI / _duckSinPeriod * (waitTime * i));
            SpawnBirds(BirdType.Duck, SpawnPoint(onRight, midPoint + ySpawnHeight), duckDir);
            yield return new WaitForSeconds(waitTime);
        }
    }

    #endregion

    #region Duck Line

    /*
     * ----------------------
     */
    private IEnumerator DuckLine(bool onRight, float height, int numDucks, float waitTime, DuckDirection duckDir)
    {
        for (var i = 0; i < numDucks; i++)
        {
            SpawnBirds(BirdType.Duck, SpawnPoint(onRight, height), duckDir);
            yield return new WaitForSeconds(waitTime);
        }
    }

    #endregion

    #region Duck V^

    /*
     * o         o
     *  \       /
     * 
     *  /       \
     * o         o
     */

    private IEnumerator DuckVv(int numDucks)
    {
        var waitTime = 0.2f;
        StartCoroutine(DuckLine(Right, 1, numDucks, waitTime, DuckDirection.DownLeft));
        StartCoroutine(DuckLine(Right, -1, numDucks, waitTime, DuckDirection.UpLeft));
        StartCoroutine(DuckLine(Left, 1, numDucks, waitTime, DuckDirection.DownRight));
        yield return StartCoroutine(DuckLine(Left, -1, numDucks, waitTime, DuckDirection.UpRight));
    }

    #endregion

    #region Graveyard

    private IEnumerator OldWave()
    {
        float[] heights = {-1, 1};
        DuckDirection[] directions = {DuckDirection.UpLeft, DuckDirection.DownLeft};

        var spawnDucks = AtHeight(heights, directions);

        for (var i = 0; i < heights.Length; i++)
        {
            yield return StartCoroutine(Produce1Wait3(() => spawnDucks(i)));
        }

        yield return MirrorDucks(heights, directions);
        yield return MirrorDucks(new float[] {0, 0}, directions);
    }

    private IEnumerator MirrorDucks(float[] heights, DuckDirection[] directions)
    {
        var spawnDucks = AtHeights(heights, directions);
        yield return StartCoroutine(Produce1Wait3(spawnDucks));
    }

    public delegate void DuckDelegate(int i);

    private DuckDelegate AtHeight(float[] myHeights, DuckDirection[] myDirections)
    {
        return i => { SpawnBirds(BirdType.Duck, SpawnPoint(Right, myHeights[i]), myDirections[i]); };
    }

    private SpawnDelegate AtHeights(float[] myHeights, DuckDirection[] myDirections)
    {
        return () =>
        {
            for (var i = 0; i < myHeights.Length; i++)
            {
                SpawnBirds(BirdType.Duck, SpawnPoint(Right, myHeights[i]), myDirections[i]);
            }
        };
    }

    #endregion
}