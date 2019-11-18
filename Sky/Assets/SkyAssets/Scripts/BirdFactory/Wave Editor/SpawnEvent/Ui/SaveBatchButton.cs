using System.IO;
using BRM.DebugAdapter;
using BRM.FileSerializers;
using BRM.FileSerializers.Interfaces;
using BRM.TextSerializers;
using TMPro;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class SaveBatchButton : Selector
    {
        [SerializeField] private TMP_InputField _batchNameInput;
        private string _folderName => $"{Application.dataPath}/SkyAssets/WaveData/Batches/";
        private string _fileName => $"{_batchNameInput.text}.json";
        private string _filePath => Path.Combine(_folderName, _fileName);

        private BatchDataMarshal _batchDataMarshal;
        private IWriteFiles _fileWriter = new TextFileSerializer(new UnityJsonSerializer(), new UnityDebugger());

        public void SetDataMarshal(BatchDataMarshal dataMarshal)
        {
            _batchDataMarshal = dataMarshal;
        }

        protected override void OnClick()
        {
            base.OnClick();
            string filePath = FileUtilities.GetUniqueFilePath(_filePath);
            _fileWriter.Write(filePath, _batchDataMarshal.Data);
        }

        private void Update()
        {
            if (_batchDataMarshal == null)
            {
                return;
            }

            _button.interactable = _batchDataMarshal.IsDataReady;
        }
    }
}