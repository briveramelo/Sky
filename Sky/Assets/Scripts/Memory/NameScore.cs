using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class NameScore : IComparable<NameScore> {

	public string playerName;
	public int points;
	public int[] allPoints;
	public int birdKillCount;
	public int[] allBirdsKillCount;
	public int waveNumber;

	public NameScore (string newName, int currentPoints, int[] currentAllPoints, int currentBirdKillCount, int[] currentAllBirdsKillCount, int newWaveNumber){
		playerName = newName;
		points = currentPoints;
		allPoints = currentAllPoints;
		birdKillCount = currentBirdKillCount;
		allBirdsKillCount = currentAllBirdsKillCount;
		waveNumber = newWaveNumber;
	}

	//for sorting in an organized/prioritized fashion (sort by score currently)
	public int CompareTo(NameScore other){
		if (other==null){
			return 1;
		}
		return points - other.points;
	}
}
