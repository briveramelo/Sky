using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System;
using System.Linq;
public class Endless_Wave : Wave {

	protected override IEnumerator RunWave(){
		while (true){
			while (true){
				yield return null;
			}
			yield return null;
		}
	}

	void Awake(){
		CheckToUnlock();
	}

	#region BirdType Collections

	OrderedDictionary lockedBirds = new OrderedDictionary(){
		{BirdType.Shoebill,		0f},
		{BirdType.Pigeon, 		4f},
		{BirdType.Albatross, 	9f},
		{BirdType.Seagull, 		14f},
		{BirdType.Duck, 		19f},
		{BirdType.Pelican, 		24f},
		{BirdType.Bat, 			29f}
	};
	List<BirdType> unlockedBirds = new List<BirdType>();

	BirdType[] ignoreBirds = new BirdType[]{
		BirdType.All,
		BirdType.BirdOfParadise,
		BirdType.Crow
	};
	BirdType[] bossBirds = new BirdType[]{
		BirdType.Eagle,
		BirdType.Tentacles,
		BirdType.BabyCrow,
		BirdType.DuckLeader
	};
	#endregion

	void UnlockNextBird(){
		BirdType unlockedBird = (BirdType)lockedBirds.Cast<DictionaryEntry>().ElementAt(0).Key;
		unlockedBirds.Add(unlockedBird);
		lockedBirds.Remove(unlockedBird);
	}

	float timeOfLastUnlock =0f;
	float timeBeforeNextUnlock =0f;
	void CheckToUnlock(){
		if (Time.time - timeOfLastUnlock > timeBeforeNextUnlock){
			timeOfLastUnlock = Time.time;
			timeBeforeNextUnlock = (float)lockedBirds.Cast<DictionaryEntry>().ElementAt(0).Value;
			UnlockNextBird();
		}
		Invoke("CheckToUnlock",5f);
	}
}
