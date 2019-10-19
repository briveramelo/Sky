using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GenericFunctions;

public class PigeonWave : Wave
{
    protected override IEnumerator GenerateBirds()
    {
        SpawnBirds(BirdType.Pigeon, SpawnPoint(Right, MedHeight));
        yield return StartCoroutine(WaitFor(AllDead));

        SpawnBirds(BirdType.Pigeon, SpawnPoint(Left, MedHeight));
        yield return StartCoroutine(WaitFor(AllDead));

        StartCoroutine(PigeonMeatball(Right, HighHeight));
        var oneDeadPigeon = new BirdWaiter(CounterType.Killed, false, 1, BirdType.Pigeon);
        yield return StartCoroutine(WaitFor(oneDeadPigeon));
        StartCoroutine(PigeonMeatball(Left, LowHeight));
        oneDeadPigeon = new BirdWaiter(CounterType.Killed, false, 1, BirdType.Pigeon);
        yield return StartCoroutine(WaitFor(oneDeadPigeon));
        StartCoroutine(PigeonMeatball(Right, MedHeight));
        yield return StartCoroutine(WaitFor(AllDead));

        var wallOnRight = Bool.TossCoin();
        yield return StartCoroutine(PigeonWall(wallOnRight, new Range(-.25f, 0.25f)));
        yield return StartCoroutine(WaitFor(AllDead));

        yield return StartCoroutine(PigeonWall(wallOnRight, new Range(-.75f, .75f)));
        yield return StartCoroutine(WaitFor(AllDead));

        yield return StartCoroutine(PigeonSine(!wallOnRight, 0.25f, MedHeight));
        yield return StartCoroutine(WaitFor(AllDead));

        yield return StartCoroutine(PigeonSlantWall(!wallOnRight, true, new Range(-.75f, .75f), .1f));
        yield return StartCoroutine(WaitFor(AllDead));

        yield return StartCoroutine(PigeonBite(MedHeight));
        yield return StartCoroutine(WaitFor(AllDead));

        yield return StartCoroutine(AlternatingPigeons(10));
        yield return StartCoroutine(WaitFor(AllDead));

        //////////////////////////////////////////////////////////////

        StartCoroutine(PigeonMeatball(Left, MedHeight));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(PigeonMeatball(Right, LowHeight));
        StartCoroutine(PigeonMeatball(Right, HighHeight));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(PigeonMeatball(Left, MedHeight));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(PigeonMeatball(Right, LowHeight));
        StartCoroutine(PigeonMeatball(Right, HighHeight));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(PigeonMeatball(Left, MedHeight));
        yield return StartCoroutine(WaitFor(AllDead));

        for (var i = 0; i < 3; i++)
        {
            yield return StartCoroutine(PigeonWall(Left, new Range(.1f, 0.6f)));
            yield return new WaitForSeconds(.8f);
            yield return StartCoroutine(PigeonWall(Right, new Range(-.6f, -0.1f)));
            yield return new WaitForSeconds(.8f);
        }

        yield return StartCoroutine(WaitFor(AllDead));

        StartCoroutine(PigeonSine(Right, 0.25f, MedHeight));
        StartCoroutine(PigeonLine(Right, HighHeight, 15, 0.25f));
        yield return StartCoroutine(PigeonLine(Left, LowHeight, 15, 0.25f));
        yield return StartCoroutine(WaitFor(AllDead));

        StartCoroutine(PigeonSlantWall(wallOnRight, true, new Range(-.75f, .75f), .1f));
        yield return StartCoroutine(PigeonSlantWall(!wallOnRight, true, new Range(-.75f, .75f), .1f));
        yield return StartCoroutine(WaitFor(AllDead));

        StartCoroutine(PigeonSlantWall(wallOnRight, wallOnRight, new Range(-.75f, .75f), .1f));
        yield return StartCoroutine(PigeonSlantWall(!wallOnRight, !wallOnRight, new Range(-.75f, .75f), .1f));
        yield return StartCoroutine(WaitFor(AllDead));

        StartCoroutine(PigeonSine(Right, 0.4f, 0.4f));
        StartCoroutine(PigeonSine(Right, 0.4f, -0.4f));
        yield return new WaitForSeconds(_pigSinPeriod / 4);
        StartCoroutine(PigeonWall(Right, new Range(.1f, 0.7f)));
        yield return new WaitForSeconds(_pigSinPeriod / 4);
        StartCoroutine(PigeonWall(Right, new Range(-.3f, 0.3f)));
        yield return new WaitForSeconds(_pigSinPeriod / 4);
        StartCoroutine(PigeonWall(Right, new Range(-.7f, -0.1f)));
        yield return new WaitForSeconds(_pigSinPeriod / 4);
        StartCoroutine(PigeonWall(Right, new Range(-.3f, 0.3f)));
        yield return StartCoroutine(WaitFor(AllDead));

        yield return StartCoroutine(AlternatingPigeons(20));
        yield return StartCoroutine(WaitFor(AllDead));
    }

    #region Alternating Pigeons

    /*
     * ->
     *         <-
     * ->
     *         <-
     */
    private IEnumerator AlternatingPigeons(int numPigeons)
    {
        var spawnOpts = new float[]
        {
            .9f, 0.85f, .8f, 0.75f, .7f, 0.65f, .6f, 0.55f, .5f, 0.45f, .4f, 0.35f, .3f, 0.25f, 0.2f, 0.15f, .1f, 0.05f, 0f,
            -.9f, -0.85f, -.8f, -0.75f, -.7f, -0.65f, -.6f, -0.55f, -.5f, -0.45f, -.4f, -0.35f, -.3f, -0.25f, -0.2f, -0.15f, -.1f, -0.05f
        };
        var spawnOptions = new List<float>(spawnOpts);
        var side = Bool.TossCoin();
        for (var i = 0; i < numPigeons; i++)
        {
            var spawnNum = Random.Range(0, spawnOptions.Count);
            SpawnBirds(BirdType.Pigeon, SpawnPoint(side, spawnOptions[spawnNum]));
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

    #region Pigeon Meatball

    /*
       0
      0 0
       0 
      0 0
       0
    */
    private IEnumerator PigeonMeatball(bool startOnRight, float midPoint)
    {
        var waitTime = 0.205f;
        var hexHeight = 0.075f;
        SpawnBirds(BirdType.Pigeon, SpawnPoint(startOnRight, midPoint + hexHeight));
        SpawnBirds(BirdType.Pigeon, SpawnPoint(startOnRight, midPoint - hexHeight));
        yield return new WaitForSeconds(waitTime);
        SpawnBirds(BirdType.Pigeon, SpawnPoint(startOnRight, midPoint));
        SpawnBirds(BirdType.Pigeon, SpawnPoint(startOnRight, midPoint + 2 * hexHeight));
        SpawnBirds(BirdType.Pigeon, SpawnPoint(startOnRight, midPoint - 2 * hexHeight));
        yield return new WaitForSeconds(waitTime);
        SpawnBirds(BirdType.Pigeon, SpawnPoint(startOnRight, midPoint + hexHeight));
        SpawnBirds(BirdType.Pigeon, SpawnPoint(startOnRight, midPoint - hexHeight));
    }

    #endregion

    #region Pigeon Wall

    /*
    -
    -
    -
    */
    private IEnumerator PigeonWall(bool startOnRight, Range heightRange)
    {
        heightRange.Max = Mathf.Clamp(heightRange.Max, -1f, 1f);
        heightRange.Min = Mathf.Clamp(heightRange.Min, -1f, 1f);
        var separationHeight = 0.075f;
        for (var spawnHeight = heightRange.Min; spawnHeight < heightRange.Max; spawnHeight += separationHeight)
        {
            SpawnBirds(BirdType.Pigeon, SpawnPoint(startOnRight, spawnHeight));
        }

        yield return null;
    }

    #endregion

    #region Pigeon Slant Wall

    /*
     *    -
     *   -
     *  -
     * -
     */
    private IEnumerator PigeonSlantWall(bool startOnRight, bool leadOnBottom, Range heightRange, float waitTime)
    {
        heightRange.Min = Mathf.Clamp(heightRange.Min, -1f, 1f);
        heightRange.Max = Mathf.Clamp(heightRange.Max, -1f, 1f);
        var separationHeight = 0.075f;
        if (leadOnBottom)
        {
            for (var spawnHeight = heightRange.Min; spawnHeight < heightRange.Max; spawnHeight += separationHeight)
            {
                SpawnBirds(BirdType.Pigeon, SpawnPoint(startOnRight, spawnHeight));
                yield return new WaitForSeconds(waitTime);
            }
        }
        else
        {
            for (var spawnHeight = heightRange.Max; spawnHeight > heightRange.Min; spawnHeight -= separationHeight)
            {
                SpawnBirds(BirdType.Pigeon, SpawnPoint(startOnRight, spawnHeight));
                yield return new WaitForSeconds(waitTime);
            }
        }
    }

    #endregion

    #region Pigeon Bite

    /*
     * >
     *          <
     * >
     *          <
     * >
     *          <
     */


    private static class PigeonTeethStats
    {
        public const float SeparationHeight = 0.075f;
        public const float TeethSeparationHeight = SeparationHeight * 6f;
    }

    private IEnumerator PigeonBite(float midSpawnHeight)
    {
        var onRight = Bool.TossCoin();
        StartCoroutine(PigeonTeeth(onRight, midSpawnHeight + PigeonTeethStats.TeethSeparationHeight));
        yield return StartCoroutine(PigeonTeeth(!onRight, midSpawnHeight - PigeonTeethStats.TeethSeparationHeight));
    }

    private IEnumerator PigeonTeeth(bool onRight, float spawnHeight)
    {
        var waitTime = 0.175f;
        var topStart = spawnHeight + PigeonTeethStats.TeethSeparationHeight / 2;
        var botStart = spawnHeight - PigeonTeethStats.TeethSeparationHeight / 2;
        SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, topStart));
        SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, botStart));
        yield return new WaitForSeconds(waitTime);

        SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, topStart + PigeonTeethStats.SeparationHeight));
        SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, topStart - PigeonTeethStats.SeparationHeight));
        SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, botStart + PigeonTeethStats.SeparationHeight));
        SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, botStart - PigeonTeethStats.SeparationHeight));
        yield return new WaitForSeconds(waitTime);

        SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, topStart + 2 * PigeonTeethStats.SeparationHeight));
        SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, topStart - 2 * PigeonTeethStats.SeparationHeight));
        SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, botStart + 2 * PigeonTeethStats.SeparationHeight));
        SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, botStart - 2 * PigeonTeethStats.SeparationHeight));
        yield return new WaitForSeconds(waitTime);

        SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, topStart + 3 * PigeonTeethStats.SeparationHeight));
        SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, topStart - 3 * PigeonTeethStats.SeparationHeight));
        SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, botStart - 3 * PigeonTeethStats.SeparationHeight));
    }

    #endregion

    #region Pigeon Sine

    /*
     *     -      
     *   -   -   -
     *         -  
     */
    private const float _pigSinPeriod = 6.13f;

    private IEnumerator PigeonSine(bool onRight, float sineAmp, float midPoint)
    {
        var numPigeons = 20;
        var waitTime = _pigSinPeriod / numPigeons;
        var ySpawnHeight = 0f;
        for (var i = 0; i < numPigeons - 2; i++)
        {
            ySpawnHeight = sineAmp * Mathf.Sin(2 * Mathf.PI / _pigSinPeriod * (waitTime * i));
            SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, midPoint + ySpawnHeight));
            yield return new WaitForSeconds(waitTime);
        }

        SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, midPoint + ySpawnHeight / 2));
    }

    #endregion

    #region Pigeon Line

    /*
     * ----------------------
     */
    private IEnumerator PigeonLine(bool onRight, float height, int numPigeons, float waitTime)
    {
        for (var i = 0; i < numPigeons; i++)
        {
            SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, height));
            yield return new WaitForSeconds(waitTime);
        }
    }

    #endregion

    #region Graveyard

    #region Pigeon 1 Wait 3 top mid bot

    private IEnumerator Pigeon3()
    {
        float[] bottomHeights = {LowHeight, LowHeight, MedHeight};
        float[] topHeights = {MedHeight, HighHeight, HighHeight};

        var spawnPigeons = new PigeonDelegate[]
        {
            AtHeight(Heights),
            AtHeight(bottomHeights) + AtHeight(topHeights)
        };
        for (var j = 0; j < spawnPigeons.Length; j++)
        {
            for (var i = 0; i < Heights.Length; i++)
            {
                yield return StartCoroutine(Produce1Wait3(() => spawnPigeons[j](i)));
            }
        }
    }

    private delegate void PigeonDelegate(int i);

    private PigeonDelegate AtHeight(float[] myHeights)
    {
        return i => { SpawnBirds(BirdType.Pigeon, SpawnPoint(Right, myHeights[i])); };
    }

    #endregion

    #endregion
}