using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GenericFunctions;

public class Pigeon_Wave : Wave {

	protected override IEnumerator GenerateBirds(){

        SpawnBirds(BirdType.Pigeon, SpawnPoint(right, medHeight));
        yield return StartCoroutine(WaitFor(allDead));
        
        SpawnBirds(BirdType.Pigeon, SpawnPoint(left, medHeight));
        yield return StartCoroutine(WaitFor(allDead));

        StartCoroutine(PigeonMeatball(right,highHeight));
        BirdWaiter oneDeadPigeon = new BirdWaiter(CounterType.Killed, false, 1, BirdType.Pigeon);
        yield return StartCoroutine(WaitFor(oneDeadPigeon));
        StartCoroutine(PigeonMeatball(left,lowHeight));
        oneDeadPigeon = new BirdWaiter(CounterType.Killed, false, 1, BirdType.Pigeon);
        yield return StartCoroutine(WaitFor(oneDeadPigeon));
        StartCoroutine(PigeonMeatball(right,medHeight));
        yield return StartCoroutine(WaitFor(allDead));

        bool wallOnRight = Bool.TossCoin();
        yield return StartCoroutine(PigeonWall(wallOnRight,new Range(-.25f, 0.25f)));
        yield return StartCoroutine(WaitFor(allDead));

        yield return StartCoroutine(PigeonWall(wallOnRight,new Range(-.75f, .75f)));
        yield return StartCoroutine(WaitFor(allDead));
        
        yield return StartCoroutine(PigeonSine(!wallOnRight, 0.25f, medHeight));
        yield return StartCoroutine(WaitFor(allDead));

        yield return StartCoroutine(PigeonSlantWall(!wallOnRight, true, new Range(-.75f, .75f), .1f));
        yield return StartCoroutine(WaitFor(allDead));

        yield return StartCoroutine(PigeonBite(medHeight));
        yield return StartCoroutine(WaitFor(allDead));

        yield return StartCoroutine(AlternatingPigeons(10));
        yield return StartCoroutine(WaitFor(allDead));

        //////////////////////////////////////////////////////////////

        StartCoroutine(PigeonMeatball(left,medHeight));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(PigeonMeatball(right,lowHeight));
        StartCoroutine(PigeonMeatball(right,highHeight));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(PigeonMeatball(left,medHeight));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(PigeonMeatball(right,lowHeight));
        StartCoroutine(PigeonMeatball(right,highHeight));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(PigeonMeatball(left,medHeight));
        yield return StartCoroutine(WaitFor(allDead));

        for (int i=0; i<3; i++){
            yield return StartCoroutine(PigeonWall(left,new Range(.1f, 0.6f)));
            yield return new WaitForSeconds(.8f);
            yield return StartCoroutine(PigeonWall(right,new Range(-.6f, -0.1f)));
            yield return new WaitForSeconds(.8f);
        }
        yield return StartCoroutine(WaitFor(allDead));

        StartCoroutine(PigeonSine(right, 0.25f, medHeight));
        StartCoroutine(PigeonLine(right, highHeight, 15, 0.25f));
        yield return StartCoroutine(PigeonLine(left, lowHeight, 15, 0.25f));
        yield return StartCoroutine(WaitFor(allDead));

        StartCoroutine(PigeonSlantWall(wallOnRight, true, new Range(-.75f, .75f), .1f));
        yield return StartCoroutine(PigeonSlantWall(!wallOnRight, true, new Range(-.75f, .75f), .1f));
        yield return StartCoroutine(WaitFor(allDead));

        StartCoroutine(PigeonSlantWall(wallOnRight, wallOnRight, new Range(-.75f, .75f), .1f));
        yield return StartCoroutine(PigeonSlantWall(!wallOnRight, !wallOnRight, new Range(-.75f, .75f), .1f));
        yield return StartCoroutine(WaitFor(allDead));

        StartCoroutine(PigeonSine(right, 0.4f, 0.4f));
        StartCoroutine(PigeonSine(right, 0.4f, -0.4f));
        yield return new WaitForSeconds(pigSinPeriod/4);
        StartCoroutine(PigeonWall(right, new Range(.1f, 0.7f)));
        yield return new WaitForSeconds(pigSinPeriod/4);
        StartCoroutine(PigeonWall(right, new Range(-.3f, 0.3f)));
        yield return new WaitForSeconds(pigSinPeriod/4);
        StartCoroutine(PigeonWall(right, new Range(-.7f, -0.1f)));
        yield return new WaitForSeconds(pigSinPeriod/4);
        StartCoroutine(PigeonWall(right, new Range(-.3f, 0.3f)));
        yield return StartCoroutine(WaitFor(allDead));

        yield return StartCoroutine(AlternatingPigeons(20));
        yield return StartCoroutine(WaitFor(allDead));
    }

    #region Alternating Pigeons
    /*
     * ->
     *         <-
     * ->
     *         <-
     */
    private IEnumerator AlternatingPigeons(int numPigeons) {
        float[] spawnOpts = new float[] {
            .9f, 0.85f, .8f, 0.75f, .7f, 0.65f, .6f, 0.55f, .5f, 0.45f, .4f, 0.35f, .3f, 0.25f, 0.2f, 0.15f, .1f, 0.05f, 0f,
            -.9f, -0.85f, -.8f, -0.75f, -.7f, -0.65f, -.6f, -0.55f, -.5f, -0.45f, -.4f, -0.35f, -.3f, -0.25f, -0.2f, -0.15f, -.1f, -0.05f
        };
        List<float> spawnOptions = new List<float>(spawnOpts);
        bool side = Bool.TossCoin();
        for (int i=0; i<numPigeons; i++) {
            int spawnNum = Random.Range(0, spawnOptions.Count);
            SpawnBirds(BirdType.Pigeon, SpawnPoint(side, spawnOptions[spawnNum]));
            spawnOptions.Remove(spawnOptions[spawnNum]);
            side = !side;
            if (spawnOptions.Count==0) {
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
    private IEnumerator PigeonMeatball(bool startOnRight, float midPoint) {
        float waitTime = 0.205f;
        float hexHeight = 0.075f;
        SpawnBirds(BirdType.Pigeon, SpawnPoint(startOnRight, midPoint+hexHeight));
        SpawnBirds(BirdType.Pigeon, SpawnPoint(startOnRight, midPoint-hexHeight));
        yield return new WaitForSeconds(waitTime);
        SpawnBirds(BirdType.Pigeon, SpawnPoint(startOnRight, midPoint));
        SpawnBirds(BirdType.Pigeon, SpawnPoint(startOnRight, midPoint+2*hexHeight));
        SpawnBirds(BirdType.Pigeon, SpawnPoint(startOnRight, midPoint-2*hexHeight));
        yield return new WaitForSeconds(waitTime);
        SpawnBirds(BirdType.Pigeon, SpawnPoint(startOnRight, midPoint+hexHeight));
        SpawnBirds(BirdType.Pigeon, SpawnPoint(startOnRight, midPoint-hexHeight));
    }
    #endregion

    #region Pigeon Wall
    /*
    -
    -
    -
    */
    private IEnumerator PigeonWall(bool startOnRight, Range heightRange) {
        heightRange.max = Mathf.Clamp(heightRange.max, -1f, 1f);
        heightRange.min = Mathf.Clamp(heightRange.min, -1f, 1f);
        float separationHeight = 0.075f;
        for (float spawnHeight=heightRange.min; spawnHeight<heightRange.max; spawnHeight+=separationHeight) {
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
    private IEnumerator PigeonSlantWall(bool startOnRight, bool leadOnBottom, Range heightRange, float waitTime) {
        heightRange.min = Mathf.Clamp(heightRange.min, -1f, 1f);
        heightRange.max = Mathf.Clamp(heightRange.max, -1f, 1f);
        float separationHeight = 0.075f;
        if (leadOnBottom) {
            for (float spawnHeight=heightRange.min; spawnHeight<heightRange.max; spawnHeight+=separationHeight) {
                SpawnBirds(BirdType.Pigeon, SpawnPoint(startOnRight, spawnHeight));
                yield return new WaitForSeconds(waitTime);
            }
        }
        else {
            for (float spawnHeight=heightRange.max; spawnHeight>heightRange.min; spawnHeight-=separationHeight) {
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


    private static class PigeonTeethStats {
        public const float separationHeight = 0.075f;
        public const float teethSeparationHeight = separationHeight * 6f;
    }

    private IEnumerator PigeonBite(float midSpawnHeight) {
        bool onRight = Bool.TossCoin();
        StartCoroutine (PigeonTeeth(onRight, midSpawnHeight+PigeonTeethStats.teethSeparationHeight));
        yield return StartCoroutine (PigeonTeeth(!onRight, midSpawnHeight-PigeonTeethStats.teethSeparationHeight));
    }

    private IEnumerator PigeonTeeth(bool onRight, float spawnHeight) {
        float waitTime = 0.175f;
        float topStart = spawnHeight + PigeonTeethStats.teethSeparationHeight / 2;
        float botStart = spawnHeight - PigeonTeethStats.teethSeparationHeight / 2;
        SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, topStart));
        SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, botStart));
        yield return new WaitForSeconds(waitTime);

        SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, topStart + PigeonTeethStats.separationHeight));
        SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, topStart - PigeonTeethStats.separationHeight));
        SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, botStart + PigeonTeethStats.separationHeight));
        SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, botStart - PigeonTeethStats.separationHeight));
        yield return new WaitForSeconds(waitTime);

        SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, topStart + 2*PigeonTeethStats.separationHeight));
        SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, topStart - 2*PigeonTeethStats.separationHeight));
        SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, botStart + 2*PigeonTeethStats.separationHeight));
        SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, botStart - 2*PigeonTeethStats.separationHeight));
        yield return new WaitForSeconds(waitTime);

        SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, topStart + 3*PigeonTeethStats.separationHeight));
        SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, topStart - 3*PigeonTeethStats.separationHeight));
        SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, botStart - 3*PigeonTeethStats.separationHeight));
    }
    #endregion

    #region Pigeon Sine
    /*
     *     -      
     *   -   -   -
     *         -  
     */
    private float pigSinPeriod = 6.13f;

    private IEnumerator PigeonSine(bool onRight, float sineAmp, float midPoint) {
        int numPigeons = 20;
        float waitTime = pigSinPeriod / numPigeons;
        float ySpawnHeight=0f;
        for (int i=0; i<numPigeons-2; i++) {
            ySpawnHeight = sineAmp * Mathf.Sin((2 * Mathf.PI / pigSinPeriod) * (waitTime * i));
            SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, midPoint + ySpawnHeight));
            yield return new WaitForSeconds(waitTime);
        }
        SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, midPoint + ySpawnHeight/2));
    }
    #endregion

    #region Pigeon Line
    /*
     * ----------------------
     */
    private IEnumerator PigeonLine(bool onRight, float height, int numPigeons, float waitTime) {
        for (int i=0; i<numPigeons; i++) {
            SpawnBirds(BirdType.Pigeon, SpawnPoint(onRight, height));
            yield return new WaitForSeconds(waitTime);
        }
    }
    #endregion

    #region Graveyard
        #region Pigeon 1 Wait 3 top mid bot

        private IEnumerator Pigeon3() {
        float[] bottomHeights = { lowHeight, lowHeight, medHeight };
        float[] topHeights = { medHeight, highHeight, highHeight };

        PigeonDelegate[] SpawnPigeons = new PigeonDelegate[]{
            AtHeight(heights),
            AtHeight(bottomHeights) + AtHeight(topHeights)
        };
        for (int j = 0; j < SpawnPigeons.Length; j++) {
            for (int i = 0; i < heights.Length; i++) {
                yield return StartCoroutine(Produce1Wait3(() => SpawnPigeons[j](i)));
            }
        }
    }

        private delegate void PigeonDelegate(int i);

        private PigeonDelegate AtHeight(float[] myHeights){
		return i =>{
			SpawnBirds(BirdType.Pigeon,SpawnPoint(right,myHeights[i]));
		};
	}
        #endregion
    #endregion
}