using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class NameScore : IComparable<NameScore> {

	public string playerName;
	public int birdKillCount;

	public NameScore (string newName, int currentbirdKillCount){
		playerName = newName;
		birdKillCount = currentbirdKillCount;
	}

	public int CompareTo(NameScore other){
		if (other == null){
			return 1;
		}
		return birdKillCount - other.birdKillCount;
	}

}
