using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GenericFunctions;

public class SaveLoadData : MonoBehaviour {

	public static SaveLoadData dataStorage;

	public WaveManager waveManager;

	public string playerName;
	public int birdKillCount;
	public int[] allBirdsKillCount;
	public int waveNumber;
	public int saveNumber;

	public int[] allBirdsAliveCount;
	public int birdAliveCount;



	public List<NameScore> highScores;

	public NameScore[] topScores;
	public string champ;
	//savenumber
	public int mostKills;
	public int[] allMostKills;
	public int highestWave;

	public bool saved;
	public bool loaded;

	void Awake(){
		if (dataStorage == null){
			DontDestroyOnLoad(gameObject);
			dataStorage = this;
		}
		else if (dataStorage != this){
			Destroy(gameObject);
		}
		waveManager = GetComponent<WaveManager> ();
		highScores = new List<NameScore> ();
		allBirdsKillCount = new int[Constants.birdNamePrefabs.Length];
		allBirdsAliveCount = new int[Constants.birdNamePrefabs.Length];
		Load ();

		if (highScores.ToArray().Length>0){
			highScores.Sort();
			topScores = highScores.ToArray();

			champ = topScores [topScores.Length - 1].playerName;
			mostKills = topScores [topScores.Length - 1].birdKillCount;
			allMostKills = topScores [topScores.Length - 1].allBirdsKillCount;
			highestWave = topScores [topScores.Length - 1].waveNumber;
			saveNumber = topScores [topScores.Length - 1].saveNumber;
		}
	}

	public void Save(){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream fileStream = File.Create(Application.dataPath + "/SaveData/playerInfo.dat");
		PlayerData playerData = new PlayerData();

		playerData.highScores = highScores;
		bf.Serialize (fileStream, playerData);
		fileStream.Close ();
	}
	
	public void Load(){
		if (File.Exists(Application.dataPath + "/SaveData/playerInfo.dat")){
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream fileStream = File.Open(Application.dataPath + "/SaveData/playerInfo.dat",FileMode.Open);
			PlayerData playerData = (PlayerData)bf.Deserialize(fileStream);
			fileStream.Close();

			highScores = playerData.highScores;
			saveNumber = playerData.highScores.ToArray().Length;
			loaded = true;
		}
	}

	public IEnumerator PromptSave(){
		saveNumber++;

		birdKillCount = waveManager.killCount;
		allBirdsKillCount = waveManager.allKillCount;
		waveNumber = waveManager.waveNumber;

		highScores.Add( new NameScore(playerName, birdKillCount, allBirdsKillCount, waveNumber, saveNumber));
		Save();
		yield return null;
	}
}

[Serializable]
class PlayerData{
	public List<NameScore> highScores;
}
