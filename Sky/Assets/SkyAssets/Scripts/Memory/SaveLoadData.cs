using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadData : MonoBehaviour {

	private DataSave _currentDataSave = new DataSave();
	private const int _maxScores = 5;
	
	public DataSave CopyCurrentDataSave() {
        return new DataSave(_currentDataSave);
    }

    private void Awake(){
		Load ();
	}

    private void Load(){
		if (File.Exists(Application.dataPath + "/SaveData/savefile.dat")){
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream fileStream = File.Open(Application.dataPath + "/SaveData/savefile.dat",FileMode.Open);
			_currentDataSave = new DataSave((DataSave)bf.Deserialize(fileStream));
			fileStream.Close();
		}
        else {
            _currentDataSave = new DataSave();
        }
	}

	public void PromptSave(StoryScore newStoryScore){
        _currentDataSave.StoryScores.Sort();
        bool isNewHighScore = _currentDataSave.StoryScores.Count == _maxScores ? newStoryScore.CompareTo(_currentDataSave.StoryScores[_currentDataSave.StoryScores.Count-1]) < 0 : true;
        AddNewHighScore(ref _currentDataSave.StoryScores, newStoryScore, isNewHighScore);
	}
    public void PromptSave(EndlessScore newEndlessScore){
        _currentDataSave.EndlessScores.Sort();
        bool isNewEndless = _currentDataSave.EndlessScores.Count == _maxScores ? newEndlessScore.CompareTo(_currentDataSave.EndlessScores[_currentDataSave.EndlessScores.Count-1]) < 0 : true;
        AddNewHighScore(ref _currentDataSave.EndlessScores, newEndlessScore, isNewEndless);
	}

    private void AddNewHighScore<T>(ref List<T> myList, T newEntry, bool isNewHighScore) {
        if (myList.Count<_maxScores) {
            myList.Add(newEntry);
            myList.Sort();
        }
        else {
            if (isNewHighScore) {
                myList.Remove(myList[_maxScores-1]);
                myList.Add(newEntry);
                myList.Sort();
            }
        }
		Save();
    }

    private void Save(){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream fileStream = File.Create(Application.dataPath + "/SaveData/savefile.dat");

		bf.Serialize (fileStream, new DataSave(_currentDataSave));
		fileStream.Close ();
	}
	
}

[Serializable]
public class DataSave{
	public List<StoryScore> StoryScores = new List<StoryScore>();
    public List<EndlessScore> EndlessScores = new List<EndlessScore>();

    public DataSave(DataSave dataToStore) {
        StoryScores = dataToStore.StoryScores;
        EndlessScores = dataToStore.EndlessScores;
    }
    public DataSave() { }
}
