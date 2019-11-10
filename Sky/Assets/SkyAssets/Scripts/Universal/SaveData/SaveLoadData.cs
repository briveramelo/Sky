using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadData : MonoBehaviour
{
    private DataSave _currentDataSave = new DataSave();
    private const int _maxScores = 5;
    private string _folderName => $"{Application.dataPath}/SaveData";
    private string _fileName => "savefile.dat";
    private string _filePath => $"{_folderName}/{_fileName}";
    public DataSave CopyCurrentDataSave() => new DataSave(_currentDataSave);

    private void Awake()
    {
        Load();
    }
    
    private void Save()
    {
        var bf = new BinaryFormatter();
        if (!Directory.Exists(_folderName))
        {
            Directory.CreateDirectory(_folderName);
        }

        var fileStream = File.Create(_filePath);

        bf.Serialize(fileStream, new DataSave(_currentDataSave));
        fileStream.Close();
    }

    private void Load()
    {
        if (File.Exists(_filePath))
        {
            var bf = new BinaryFormatter();
            var fileStream = File.Open(_filePath, FileMode.Open);
            _currentDataSave = new DataSave((DataSave) bf.Deserialize(fileStream));
            fileStream.Close();
        }
        else
        {
            _currentDataSave = new DataSave();
        }
    }

    public void PromptSave(StoryScore newStoryScore)
    {
        _currentDataSave.StoryScores.Sort();
        var isNewHighScore = _currentDataSave.StoryScores.Count == _maxScores ? newStoryScore.CompareTo(_currentDataSave.StoryScores[_currentDataSave.StoryScores.Count - 1]) < 0 : true;
        AddNewHighScore(ref _currentDataSave.StoryScores, newStoryScore, isNewHighScore);
    }

    public void PromptSave(EndlessScore newEndlessScore)
    {
        _currentDataSave.EndlessScores.Sort();
        var isNewEndless = _currentDataSave.EndlessScores.Count == _maxScores ? newEndlessScore.CompareTo(_currentDataSave.EndlessScores[_currentDataSave.EndlessScores.Count - 1]) < 0 : true;
        AddNewHighScore(ref _currentDataSave.EndlessScores, newEndlessScore, isNewEndless);
    }

    private void AddNewHighScore<T>(ref List<T> myList, T newEntry, bool isNewHighScore)
    {
        if (myList.Count < _maxScores)
        {
            myList.Add(newEntry);
            myList.Sort();
        }
        else
        {
            if (isNewHighScore)
            {
                myList.Remove(myList[_maxScores - 1]);
                myList.Add(newEntry);
                myList.Sort();
            }
        }

        Save();
    }
}

[Serializable]
public class DataSave
{
    public List<StoryScore> StoryScores = new List<StoryScore>();
    public List<EndlessScore> EndlessScores = new List<EndlessScore>();

    public DataSave(DataSave dataToStore)
    {
        StoryScores = dataToStore.StoryScores;
        EndlessScores = dataToStore.EndlessScores;
    }

    public DataSave()
    {
    }
}