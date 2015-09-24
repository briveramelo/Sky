using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class NameScore : IComparable<NameScore> {

	public int saveNumber;
	public string playerName;
	public int birdKillCount;
	public int[] allBirdsKillCount;
	public int waveNumber;

	public NameScore (string newName, int currentBirdKillCount, int[] currentAllBirdsKillCount, int newWaveNumber, int newSaveNumber){
		playerName = newName;
		birdKillCount = currentBirdKillCount;
		allBirdsKillCount = currentAllBirdsKillCount;
		saveNumber = newSaveNumber;
		waveNumber = newWaveNumber;
	}

	public int CompareTo(NameScore other/*,bool score*/){
		if (other==null){
			return 1;
		}

//		if (score){
			return birdKillCount - other.birdKillCount;
//		}

//		return saveNumber - other.saveNumber;
	}
	

}
