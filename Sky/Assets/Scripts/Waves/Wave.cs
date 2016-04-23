﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GenericFunctions;
using System;

public interface IWaveRunnable{
	IEnumerator RunWave();
}

public delegate void SpawnDelegate();

public abstract class Wave : MonoBehaviour, IWaveRunnable{

    [SerializeField] WaveName MyWaveName;
    
	protected BirdWaiter allDead = new BirdWaiter(CounterType.Alive,false, 0, BirdType.All);
	protected BirdWaiter allDeadExceptTentacles = new BirdWaiter(CounterType.Alive,true, 0, BirdType.Tentacles);

	protected static int waveNumber;
	protected const float lowHeight = -0.6f;
	protected const float medHeight = 0f;
	protected const float highHeight = 0.6f;
	protected static float[] heights;
	protected Vector2[] duckSpawnPoints;

    const float wavePauseTime = 10f;
	protected const bool right = true;
	protected const bool left = false;
	#region SpawnDelegates
	protected SpawnDelegate SpawnAtRandom(BirdType birdType){
		return ()=>{
            Vector2 spawnPoint;
            if (birdType == BirdType.DuckLeader) {
                spawnPoint= SpawnPoint(Bool.TossCoin(), .5f *lowHeight, .5f *highHeight);
            }
            else if (birdType == BirdType.Tentacles || birdType == BirdType.Crow) {
                spawnPoint = Vector2.zero;
            }
            else {
                spawnPoint = SpawnPoint(Bool.TossCoin(), lowHeight, highHeight);
            }
            SpawnBirds(birdType, spawnPoint, (DuckDirection)UnityEngine.Random.Range(0, Enum.GetNames(typeof(DuckDirection)).Length));
		};
	}

    protected Dictionary<BirdType, SpawnDelegate> BirdSpawnDelegates = new Dictionary<BirdType, SpawnDelegate>();
    #endregion

    void OnLevelWasLoaded(int level) {
        if (level == (int)Scenes.Menu) {
            StopAllCoroutines();
        }
    }
    IWaveUI waveUI;
    void Awake(){
		heights = new float[]{lowHeight, medHeight, highHeight};
		duckSpawnPoints = new Vector2[6];
		for (int i=0; i<6; i++){
			duckSpawnPoints[i] = SpawnPoint(i%2==0,heights[Mathf.FloorToInt(i/2)]);
		}

        for (int i = 0; i < Enum.GetNames(typeof(BirdType)).Length - 1; i++) {
            BirdSpawnDelegates.Add((BirdType)i, SpawnAtRandom((BirdType)i));
        }
        waveUI = FindObjectOfType<WaveUI>().GetComponent<IWaveUI>();
    }

	IEnumerator IWaveRunnable.RunWave(){
        yield return StartCoroutine (StartWave());
		yield return StartCoroutine (GenerateBirds());
		yield return StartCoroutine (FinishWave());
	}

    IEnumerator StartWave() {
        yield return StartCoroutine(waveUI.AnimateWaveStart(MyWaveName));
    }
    protected virtual IEnumerator GenerateBirds() { yield return null; }
    IEnumerator FinishWave(){
		yield return new WaitForSeconds(2f);
		SpawnBirds (BirdType.BirdOfParadise, SpawnPoint(right,lowHeight));
		yield return StartCoroutine(WaitFor(allDeadExceptTentacles,true));
        yield return StartCoroutine(waveUI.AnimateWaveEnd(MyWaveName));
		waveNumber++;
		ScoreSheet.Resetter.ResetWaveCounters();
	}

	/// <summary> Spawn Birds
	/// </summary>
	public void SpawnBirds(BirdType birdType, Vector2 spawnPoint, DuckDirection duckDir = DuckDirection.UpRight){ 
		int direction = spawnPoint.x<0 ? 1 : -1;

		if (birdType == BirdType.Eagle){
			spawnPoint= new Vector2(-Constants.WorldDimensions.x *5f,0f);
		}

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
		return new Vector2 (((startOnRight ? 1:-1) * Constants.WorldDimensions.x), UnityEngine.Random.Range (y1, y2) * Constants.WorldDimensions.y);
	}

	protected IEnumerator MassProduce(SpawnDelegate spawn, int birdCount){
		for (int i=0; i<birdCount; i++){
			spawn();
			yield return new WaitForSeconds(1f);
		}
	}

	protected IEnumerator ProduceDucks(int numDucks){
		List<Vector2> duckSpawnList = new List<Vector2>(duckSpawnPoints);
		for (int i=0; i<numDucks; i++){
			yield return new WaitForSeconds(1f);
			int chosenPoint = UnityEngine.Random.Range(0,duckSpawnList.Count);
			SpawnBirds(BirdType.Duck,duckSpawnList[chosenPoint], DuckDirectionGenerator(duckSpawnList[chosenPoint]));
			duckSpawnList.RemoveAt(chosenPoint);
            if (duckSpawnList.Count == 0) {
                duckSpawnList = new List<Vector2>(duckSpawnPoints);
            }
		}
	}

	/// <summary> Will output DuckDirection based on duck's spawning position
	/// </summary>
	DuckDirection DuckDirectionGenerator(Vector2 spawnPoint){
		if (spawnPoint.y==0){
			bool goUp = Bool.TossCoin();
			return spawnPoint.x>0 ? (goUp ? DuckDirection.UpLeft : DuckDirection.DownLeft) :
				(goUp ? DuckDirection.UpRight : DuckDirection.DownRight);
		}
		else if (spawnPoint.y>0){
			return spawnPoint.x>0 ? DuckDirection.DownLeft : DuckDirection.DownRight;
		}
		else{
			return spawnPoint.x>0 ? DuckDirection.UpLeft : DuckDirection.UpRight;
		}
	}

	protected IEnumerator FlyPigeonsAsDuckLeader(){
		SpawnBirds (BirdType.Pigeon, SpawnPoint(right,0));
		for (int i=0; i<3; i++){
			yield return new WaitForSeconds (.5f);
			SpawnBirds (BirdType.Pigeon, SpawnPoint(right,.1f * (i+1)));
			SpawnBirds (BirdType.Pigeon, SpawnPoint(right,-.1f * (i+1)));
		}
		SpawnBirds (BirdType.DuckLeader, SpawnPoint(right,0));
	}

	protected IEnumerator Produce1Wait3(SpawnDelegate spawn){
		spawn();
		yield return StartCoroutine (WaitFor (allDeadExceptTentacles, true));
		yield return StartCoroutine (MassProduce (spawn,3));
		yield return StartCoroutine (WaitFor (allDeadExceptTentacles, true));
	}
		
	/// <summary> Wait until at most "birdsRemaining" "birdTypes" remain alive on screen
	/// </summary>
	protected IEnumerator WaitFor(BirdWaiter birdWaiter, bool waitExtra=false){
		while (birdWaiter.wait){
			yield return null;
		}
		FinishWaiting(birdWaiter);
		if (waitExtra) {
            yield return StartCoroutine (WaitUntilTimeRange());
        }
	}

	/// <summary> Wait between minTime and maxTime seconds
	/// </summary>
	protected IEnumerator WaitUntilTimeRange(float minTime=1f, float maxTime=2f){
		yield return new WaitForSeconds(UnityEngine.Random.Range(minTime,maxTime));
	}

	protected IEnumerator WaitInParallel(params BirdWaiter[] birdWaiters){
		List<BirdWaiter> birdWaitList = new List<BirdWaiter>(birdWaiters);
		while (birdWaitList.Count>0){
			for (int i=0; i<birdWaitList.Count; i++){
				if (!birdWaitList[i].wait){
					FinishWaiting(birdWaitList[i]);
					birdWaitList.Remove(birdWaitList[i]);
				}
			}
			yield return null;
		}
	}

	void FinishWaiting(BirdWaiter birdWaiter){
		if (birdWaiter.Perform!=null){
			StartCoroutine (birdWaiter.Perform);
		}
		else{
			if (birdWaiter.Spawn!=null){
				birdWaiter.Spawn();
			}
		}
	}
}