using System.IO;
using BRM.DebugAdapter;
using BRM.FileSerializers;
using BRM.FileSerializers.Interfaces;
using BRM.TextSerializers;
using UnityEngine;
using UnityEngine.UI;

namespace BRM.Sky.WaveEditor
{
    public class SaveWaveDataButton : Selector
    {
        [SerializeField] private WaveDataMarshal _waveDataMarshal;
        
        private string _folderName => $"{Application.dataPath}/SkyAssets/WaveData/Waves/";
        private string _fileName => $"{_waveDataMarshal.Data.Name}.json";
        private string _filePath => Path.Combine(_folderName, _fileName);

        private IWriteFiles _fileWriter = new TextFileSerializer(new UnityJsonSerializer(), new UnityDebugger());
        
        protected override void OnClick()
        {
            string filePath = FileUtilities.GetUniqueFilePath(_filePath);
            _fileWriter.Write(filePath, _waveDataMarshal.Data);
            base.OnClick();
        }

        private void Update()
        {
            _button.interactable = _waveDataMarshal.IsDataReady;
        }
    }
}
