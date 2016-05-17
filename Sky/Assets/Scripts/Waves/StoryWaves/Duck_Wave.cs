using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GenericFunctions;

public class Duck_Wave : Wave {

	protected override IEnumerator GenerateBirds(){
        yield return StartCoroutine(DuckSine(true,.5f, 0f, DuckDirection.UpRight));
        yield return StartCoroutine(WaitFor(allDead));
        
        SpawnBirds(BirdType.DuckLeader, SpawnPoint(right, 0f));
        BirdWaiter oneDeadLeader = new BirdWaiter(CounterType.Killed, false, 1, BirdType.DuckLeader);
        yield return StartCoroutine(WaitFor(oneDeadLeader));
        SpawnBirds(BirdType.DuckLeader, SpawnPoint(left, 0f));
        oneDeadLeader = new BirdWaiter(CounterType.Killed, false, 1, BirdType.DuckLeader);
        yield return StartCoroutine(WaitFor(oneDeadLeader));
        bool topIsRight = Bool.TossCoin();
        SpawnBirds(BirdType.DuckLeader, SpawnPoint(topIsRight, highHeight));
        SpawnBirds(BirdType.DuckLeader, SpawnPoint(!topIsRight, lowHeight));
        yield return StartCoroutine(WaitFor(allDead));

        bool wallOnRight = Bool.TossCoin();
        DuckDirection firstDuckDir = wallOnRight ? DuckDirection.UpLeft : DuckDirection.UpRight;
        DuckDirection secondDuckDir = !wallOnRight ? DuckDirection.DownLeft : DuckDirection.DownRight;

        StartCoroutine(DuckWall(wallOnRight,new Range(-.75f, -0.25f), firstDuckDir));
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(DuckWall(wallOnRight,new Range(-.75f, 0.75f), firstDuckDir));
        yield return StartCoroutine(WaitFor(allDead));

        StartCoroutine(DuckWall(wallOnRight,new Range(-.25f, 0.25f), firstDuckDir));
        StartCoroutine(DuckWall(!wallOnRight,new Range(-.25f, 0.25f), secondDuckDir));
        yield return new WaitForSeconds(2f);
        StartCoroutine(DuckWall(wallOnRight,new Range(-.75f, 0.75f), firstDuckDir));
        yield return StartCoroutine(DuckWall(!wallOnRight,new Range(-.75f, 0.75f), secondDuckDir));
        yield return StartCoroutine(WaitFor(allDead));

        
        yield return StartCoroutine(DuckSine(wallOnRight,0.3f, 0f, firstDuckDir));
        yield return StartCoroutine(WaitFor(allDead));

        yield return StartCoroutine(DuckSlantWall(!wallOnRight, true, new Range(-.75f, .75f), 0.1f, secondDuckDir));
        yield return StartCoroutine(WaitFor(allDead));

	}
    
    #region Alternating Ducks
    /*
     * ->
     *         <-
     * ->
     *         <-
     */
    IEnumerator AlternatingDucks(int numDucks) {
        float[] spawnOpts = new float[] {
            .9f, 0.85f, .8f, 0.75f, .7f, 0.65f, .6f, 0.55f, .5f, 0.45f, .4f, 0.35f, .3f, 0.25f, 0.2f, 0.15f, .1f, 0.05f, 0f,
            -.9f, -0.85f, -.8f, -0.75f, -.7f, -0.65f, -.6f, -0.55f, -.5f, -0.45f, -.4f, -0.35f, -.3f, -0.25f, -0.2f, -0.15f, -.1f, -0.05f
        };
        List<float> spawnOptions = new List<float>(spawnOpts);
        bool side = Bool.TossCoin();
        for (int i=0; i<numDucks; i++) {
            int spawnNum = Random.Range(0, spawnOptions.Count);
            Vector2 spawnSpot = SpawnPoint(side, spawnOptions[spawnNum]);
            SpawnBirds(BirdType.Duck, spawnSpot, DuckDirectionGenerator(spawnSpot));
            spawnOptions.Remove(spawnOptions[spawnNum]);
            side = !side;
            if (spawnOptions.Count==0) {
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
    IEnumerator DuckMeatball(bool startOnRight, float midPoint, DuckDirection duckDir) {
        float waitTime = 0.285f;
        float hexHeight = 0.17f;
        float waitedTime = 0f;
        float waitHeight = (duckDir == DuckDirection.UpRight || duckDir == DuckDirection.UpLeft ? 1 :- 1) * .6f;
        int i = 0;
        float startTime = Time.time;
        SpawnBirds(BirdType.Duck, SpawnPoint(startOnRight, midPoint + hexHeight/2), duckDir);
        SpawnBirds(BirdType.Duck, SpawnPoint(startOnRight, midPoint - hexHeight/2), duckDir);
        yield return new WaitForSeconds(waitTime); i++;
        waitedTime = Time.time - startTime;
        startTime = Time.time;
        SpawnBirds(BirdType.Duck, SpawnPoint(startOnRight, midPoint + waitedTime*waitHeight*i), duckDir);
        SpawnBirds(BirdType.Duck, SpawnPoint(startOnRight, midPoint + hexHeight + waitedTime*waitHeight*i), duckDir);
        SpawnBirds(BirdType.Duck, SpawnPoint(startOnRight, midPoint - hexHeight + waitedTime*waitHeight*i), duckDir);
        yield return new WaitForSeconds(waitTime); i++;
        waitedTime = Time.time - startTime;
        SpawnBirds(BirdType.Duck, SpawnPoint(startOnRight, midPoint + hexHeight/2 + waitedTime*waitHeight*i), duckDir);
        SpawnBirds(BirdType.Duck, SpawnPoint(startOnRight, midPoint - hexHeight/2 + waitedTime*waitHeight*i), duckDir);

    }
    #endregion

    #region Duck Wall
    /*
    -
    -
    -
    */
    IEnumerator DuckWall(bool startOnRight, Range heightRange, DuckDirection duckDir) {
        heightRange.max = Mathf.Clamp(heightRange.max, -1f, 1f);
        heightRange.min = Mathf.Clamp(heightRange.min, -1f, 1f);
        float separationHeight = 0.075f;
        for (float spawnHeight=heightRange.min; spawnHeight<heightRange.max; spawnHeight+=separationHeight) {
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
    IEnumerator DuckSlantWall(bool startOnRight, bool leadOnBottom, Range heightRange, float waitTime, DuckDirection duckDir) {
        heightRange.min = Mathf.Clamp(heightRange.min, -1f, 1f);
        heightRange.max = Mathf.Clamp(heightRange.max, -1f, 1f);
        float separationHeight = 0.075f;
        if (leadOnBottom) {
            for (float spawnHeight=heightRange.min; spawnHeight<heightRange.max; spawnHeight+=separationHeight) {
                SpawnBirds(BirdType.Duck, SpawnPoint(startOnRight, spawnHeight), duckDir);
                yield return new WaitForSeconds(waitTime);
            }
        }
        else {
            for (float spawnHeight=heightRange.max; spawnHeight>heightRange.min; spawnHeight-=separationHeight) {
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
    float duckSinPeriod = Constants.WorldDimensions.x*2f;
    IEnumerator DuckSine(bool onRight, float sineAmp, float midPoint, DuckDirection duckDir) {
        int numDucks = 20;
        float waitTime = duckSinPeriod / numDucks;
        for (int i=0; i<numDucks; i++) {
            float ySpawnHeight = sineAmp * Mathf.Sin((2 * Mathf.PI / duckSinPeriod) * (waitTime * i));
            SpawnBirds(BirdType.Duck, SpawnPoint(onRight, midPoint + ySpawnHeight), duckDir);
            yield return new WaitForSeconds(waitTime);
        }
    }
    #endregion

    #region Duck Line
    /*
     * ----------------------
     */
    IEnumerator DuckLine(bool onRight, float height, int numDucks, float waitTime, DuckDirection duckDir) {
        for (int i=0; i<numDucks; i++) {
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

     IEnumerator DuckVV(int numDucks) {
        float waitTime = 0.2f;
        StartCoroutine(DuckLine(right, 1, numDucks, waitTime, DuckDirection.DownLeft));
        StartCoroutine(DuckLine(right, -1, numDucks, waitTime, DuckDirection.UpLeft));
        StartCoroutine(DuckLine(left, 1, numDucks, waitTime, DuckDirection.DownRight));
        yield return StartCoroutine(DuckLine(left, -1, numDucks, waitTime, DuckDirection.UpRight));
     }

    #endregion

    #region Graveyard
    IEnumerator OldWave() {
        float[] heights = new float[]{-1,1};
		DuckDirection[] directions = new DuckDirection[]{DuckDirection.UpLeft, DuckDirection.DownLeft};

		DuckDelegate SpawnDucks = AtHeight(heights, directions);

		for (int i=0; i<heights.Length; i++){
			yield return StartCoroutine(Produce1Wait3(()=>SpawnDucks(i)));
		}
		yield return MirrorDucks(heights, directions);
		yield return MirrorDucks(new float[]{0,0}, directions);
    }

    IEnumerator MirrorDucks(float[] heights, DuckDirection[] directions){
		SpawnDelegate SpawnDucks = AtHeights(heights, directions);
		yield return StartCoroutine(Produce1Wait3(SpawnDucks));
	}

	public delegate void DuckDelegate(int i);
	DuckDelegate AtHeight(float[] myHeights, DuckDirection[] myDirections){
		return (int i)=>{
			SpawnBirds (BirdType.Duck, SpawnPoint(right,myHeights[i]),myDirections[i]);
		};
	}
	SpawnDelegate AtHeights(float[] myHeights, DuckDirection[] myDirections){
		return ()=>{
			for (int i=0; i<myHeights.Length; i++){
				SpawnBirds (BirdType.Duck, SpawnPoint(right,myHeights[i]),myDirections[i]);
			}
		};
	}
    #endregion
}