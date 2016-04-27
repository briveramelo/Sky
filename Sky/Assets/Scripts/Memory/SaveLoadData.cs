using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadData : MonoBehaviour {

    public DataSave CopyCurrentDataSave() {
        return new DataSave(currentDataSave);
    }

    DataSave currentDataSave;
    const int maxScores = 5;

	void Awake(){
		Load ();
	}

	void Load(){
		if (File.Exists(Application.dataPath + "/SaveData/savefile.dat")){
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream fileStream = File.Open(Application.dataPath + "/SaveData/savefile.dat",FileMode.Open);
			currentDataSave = new DataSave((DataSave)bf.Deserialize(fileStream));
			fileStream.Close();
		}
        else {
            currentDataSave = new DataSave();
        }
	}

	public void PromptSave(StoryScore NewStoryScore){
        currentDataSave.storyScores.Sort();
        bool isNewHighScore = currentDataSave.storyScores.Count == maxScores ? NewStoryScore.CompareTo(currentDataSave.storyScores[currentDataSave.storyScores.Count-1]) < 0 : true;
        AddNewHighScore(ref currentDataSave.storyScores, NewStoryScore, isNewHighScore);
	}
    public void PromptSave(EndlessScore NewEndlessScore){
        currentDataSave.endlessScores.Sort();
        bool isNewEndless = currentDataSave.endlessScores.Count == maxScores ? NewEndlessScore.CompareTo(currentDataSave.endlessScores[currentDataSave.endlessScores.Count-1]) < 0 : true;
        AddNewHighScore(ref currentDataSave.endlessScores, NewEndlessScore, isNewEndless);
	}

    void AddNewHighScore<T>(ref List<T> MyList, T NewEntry, bool isNewHighScore) {
        if (MyList.Count<maxScores) {
            MyList.Add(NewEntry);
            MyList.Sort();
        }
        else {
            if (isNewHighScore) {
                MyList.Remove(MyList[maxScores-1]);
                MyList.Add(NewEntry);
                MyList.Sort();
            }
        }
		Save();
    }
	
	void Save(){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream fileStream = File.Create(Application.dataPath + "/SaveData/savefile.dat");

		bf.Serialize (fileStream, new DataSave(currentDataSave));
		fileStream.Close ();
	}
	
}

[Serializable]
public class DataSave{
	public List<StoryScore> storyScores = new List<StoryScore>();
    public List<EndlessScore> endlessScores = new List<EndlessScore>();

    public DataSave(DataSave DataToStore) {
        this.storyScores = DataToStore.storyScores;
        this.endlessScores = DataToStore.endlessScores;
    }
    public DataSave() { }
}
