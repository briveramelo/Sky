using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GenericFunctions;

public class SaveLoadData : WaveManager {

	public static SaveLoadData dataStorage;
	
	public string playerName;
	public int saveNumber;
	
	public List<NameScore> highScores;

	//TOP STUFF
	public NameScore[] topScores;
	public string champ;
	public int mostPoints;
	public int[] allMostPoints;
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
		highScores = new List<NameScore> ();
		Load ();
	}

	public void Load(){
		if (File.Exists(Application.dataPath + "/SaveData/playerInfo.dat")){
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream fileStream = File.Open(Application.dataPath + "/SaveData/playerInfo.dat",FileMode.Open);
			PlayerData playerData = (PlayerData)bf.Deserialize(fileStream);
			fileStream.Close();

			highScores = playerData.highScores;
			saveNumber = playerData.highScores.ToArray().Length;


			if (highScores.ToArray().Length>0){
				DisplayChampStats();
			}

			loaded = true;
		}
	}

	void DisplayChampStats(){
		highScores.Sort();
		topScores = highScores.ToArray();
		
		champ = topScores [topScores.Length - 1].playerName;
		mostPoints = topScores [topScores.Length - 1].points;
		allMostPoints = topScores [topScores.Length - 1].allPoints;
		mostKills = topScores [topScores.Length - 1].birdKillCount;
		allMostKills = topScores [topScores.Length - 1].allBirdsKillCount;
		highestWave = topScores [topScores.Length - 1].waveNumber;
	}

	public IEnumerator PromptSave(){
		saveNumber++;
		
		highScores.Add( new NameScore(playerName, points, allPoints, killCount, allKillCount, waveNumber, saveNumber));
		Save();
		yield return null;
	}
	
	public void Save(){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream fileStream = File.Create(Application.dataPath + "/SaveData/playerInfo.dat");
		PlayerData playerData = new PlayerData();
		
		playerData.highScores = highScores;
		bf.Serialize (fileStream, playerData);
		fileStream.Close ();
	}
	
}

[Serializable]
class PlayerData{
	public List<NameScore> highScores;
}
