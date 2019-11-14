using UnityEngine;
using System.Collections.Generic;
using System;
using BRM.BinarySerializers;
using BRM.DebugAdapter;
using BRM.FileSerializers;
using BRM.FileSerializers.Interfaces;

public class SaveLoadData : MonoBehaviour
{
    public DataSave CopyCurrentDataSave() => new DataSave(_currentDataSave);
    
    private const int _maxScores = 5;
    
    private DataSave _currentDataSave = new DataSave();
    #if UNITY_EDITOR
    private string _folderName => $"{Application.dataPath}/SaveData";
    #else
    private string _folderName => $"{Application.persistentDataPath}/SaveData";
    #endif
    private string _fileName => "savefile.dat";
    private string _filePath => $"{_folderName}/{_fileName}";
    
    private IWriteFiles _fileWriter;
    private IReadFiles _fileReader;

    private void Awake()
    {
        var binaryFileSerializer = new BinaryFileSerializer(new SystemBinarySerializer(), new UnityDebugger());
        _fileWriter = binaryFileSerializer;
        _fileReader = binaryFileSerializer;
        
        Load();
    }
    
    private void Save()
    {
        _fileWriter.Write(_filePath, _currentDataSave);
    }

    private void Load()
    {
        _currentDataSave = _fileReader.Read<DataSave>(_filePath) ?? new DataSave();
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