using System.Collections;
using System.Collections.Generic;
using System.IO;
using BRM.FileSerializers;
using BRM.FileSerializers.Interfaces;
using TMPro;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class SaveButton : Selector
    {
        [SerializeField] private TextMeshProUGUI _test;
        private string _folderName => $"{Application.dataPath}/Assets/SkyAssets/WaveData/";
        private string _fileName => $"{_test.text}.json";
        private string _filePath => Path.Combine(_folderName, _fileName);
        
        //private IWriteFiles _fileWriter = new TextFileSerializer();
        protected override void OnClick()
        {
            base.OnClick();
        }
    }
}
