using System.IO;
using BRM.DebugAdapter;
using BRM.FileSerializers;
using BRM.FileSerializers.Interfaces;
using BRM.TextSerializers;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public abstract class SaveButton<TMarshal, TData, TView> : Selector where TMarshal : DataMarshal<TData, TView> where TData : class, new() where TView : MonoBehaviour
    {
        protected abstract string _folderName { get; }
        protected abstract string _fileName { get; }
        
        protected TMarshal _dataMarshal;
        
        private string _filePath => Path.Combine(_folderName, _fileName);
        private IWriteFiles _fileWriter = new TextFileSerializer(new UnityJsonSerializer(), new UnityDebugger());

        public void SetDataMarshal(TMarshal dataMarshal)
        {
            _dataMarshal = dataMarshal;
        }

        protected override void OnClick()
        {
            string filePath = FileUtilities.GetUniqueFilePath(_filePath);
            _fileWriter.Write(filePath, _dataMarshal.Data);
        #if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
        #endif
            base.OnClick();
        }

        private void Update()
        {
            if (_dataMarshal == null)
            {
                return;
            }

            _button.interactable = _dataMarshal.IsDataReady;
        }
    }
}