using UnityEngine;
using System.Collections;
using GenericFunctions;

public interface IWaveRunnable{
	IEnumerator RunWave();
}

[RequireComponent (typeof (ScoreSheet))]
public abstract class Wave : MonoBehaviour, IWaveRunnable{

	protected static int waveNumber;
	protected const float wavePauseTime = 10f;
	protected const float lowHeight = -0.6f;
	protected const float medHeight = 0f;
	protected const float highHeight = 0.6f;

	protected const bool right = true;
	protected const bool left = false;

	IEnumerator IWaveRunnable.RunWave(){
		yield return StartCoroutine (RunWave());
	}

	protected virtual IEnumerator RunWave(){
		//override with specific wave content here
		yield return StartCoroutine (FinishTheWave());
	} 

	/// <summary> Spawn Birds
	/// </summary>
	protected void SpawnBirds(BirdType birdType, Vector2 spawnPoint, DuckDirection duckDir = DuckDirection.UpRight){ 
		int direction = spawnPoint.x<0 ? 1 : -1;
		Bird bird = (Instantiate (Incubator.Instance.Birds[(int)birdType], spawnPoint, Quaternion.identity) as GameObject).GetComponent<Bird>();

		if (birdType == BirdType.Pigeon || birdType == BirdType.BirdOfParadise){
			LinearBird linearBirdScript = (LinearBird)bird;
			linearBirdScript.SetVelocity(Vector2.right * direction);
		}
		else if (birdType ==  BirdType.Duck){
			IDirectable duckScript = (IDirectable)bird;
			duckScript.SetDuckDirection(duckDir);
		} 
	}

	/// <summary> side is +/- 1 and y position is between -1 <-> +1
	/// Multiplies these input numbers by worldDimensions
	/// </summary>
	protected static Vector2 SpawnPoint(bool startOnRight, float y1, float y2=-1337f){
		y2 = y2==-1337f ? y1 : y2; 
		return new Vector2 (((startOnRight ? 1:-1) * Constants.WorldDimensions.x), Random.Range (y1, y2) * Constants.WorldDimensions.y);
	}
		
	/// <summary> Wait until at most "birdsRemaining" "birdTypes" remain alive on screen
	/// </summary>
	protected IEnumerator WaitUntilRemaining(int birdsRemaining, bool waitExtraWhenDone=false, BirdType birdType=BirdType.All){
		while (ScoreSheet.Reporter.GetCount(CounterType.Alive,birdType) > birdsRemaining){
			yield return null;
		}
		if (waitExtraWhenDone) yield return StartCoroutine (WaitUntilTimeRange());
	}
		
	/// <summary> Wait between minTime and maxTime seconds
	/// </summary>
	protected IEnumerator WaitUntilTimeRange(float minTime=1f, float maxTime=2f){
		yield return new WaitForSeconds(Random.Range(minTime,maxTime));
	}

	protected IEnumerator MassProduce(BirdType birdType, int birdCount, bool startOnRight=true, float yHeight=lowHeight, DuckDirection duckDir=DuckDirection.UpRight){
		for (int i=0; i<birdCount; i++){
			SpawnBirds (birdType, SpawnPoint(startOnRight,yHeight),duckDir);
			yield return new WaitForSeconds(1f);
		}
	}

	protected IEnumerator FinishTheWave(){
		yield return StartCoroutine (WaitUntilRemaining (0));
		yield return new WaitForSeconds(2f);
		SpawnBirds (BirdType.BirdOfParadise, SpawnPoint(right,lowHeight));
		yield return StartCoroutine (WaitUntilRemaining (0));
		waveNumber++;
		ScoreSheet.Resetter.ResetWaveCounters();
	}
}