using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GenericFunctions;

public class SaveLoadData : MonoBehaviour {

	public static SaveLoadData dataStorage;

	public int birdKillCount;
	public string playerName;
	public List<NameScore> highScores;
	public int allTimeRecord;
	public string champ;
	public NameScore[] topScores;
	
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

		if (highScores.ToArray().Length>0){
			highScores.Sort ();
			topScores = highScores.ToArray();
			champ = topScores [topScores.Length - 1].playerName;
			allTimeRecord = topScores [topScores.Length - 1].birdKillCount;
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
			loaded = true;
		}
	}

	public IEnumerator PromptSave(){
		highScores.Add( new NameScore(playerName, birdKillCount));
		Save();
		yield return null;
	}
}

[Serializable]
class PlayerData{
	public List<NameScore> highScores;
}
