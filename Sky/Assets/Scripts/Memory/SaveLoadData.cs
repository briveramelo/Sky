using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GenericFunctions;

public class SaveLoadData : MonoBehaviour {

	public static SaveLoadData Instance;
	
	private string playerName;
	private int saveNumber;
	
	private List<NameScore> highScores;

	//TOP STUFF
	private NameScore[] topScores;
	private string champ;
	private int mostPoints;
	private int[] allMostPoints;
	private int mostKills;
	private int[] allMostKills;
	private int highestWave;

	private bool saved;
	private bool loaded;

	void Awake(){
		if (Instance == null){
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
		else if (Instance != this){
			Destroy(gameObject);
		}
		highScores = new List<NameScore> ();
		Load ();
	}

	void Load(){
		if (File.Exists(Application.dataPath + "/SaveData/savefile.dat")){
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream fileStream = File.Open(Application.dataPath + "/SaveData/savefile.dat",FileMode.Open);
			DataSave dataSave = (DataSave)bf.Deserialize(fileStream);
			fileStream.Close();

			highScores = dataSave.highScores;
			saveNumber = dataSave.highScores.ToArray().Length;


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

	public void PromptSave(/*NameScore nameScore*/){		
		//highScores.Add(nameScore);
		Save();
	}
	
	void Save(){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream fileStream = File.Create(Application.dataPath + "/SaveData/savefile.dat");
		DataSave dataSave = new DataSave();
		
		dataSave.highScores = highScores;
		bf.Serialize (fileStream, dataSave);
		fileStream.Close ();
	}
	
}

[Serializable]
class DataSave{
	public List<NameScore> highScores;
}
