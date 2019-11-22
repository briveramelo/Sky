using System.IO;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public abstract class FileButton<TMarshal, TData, TView> : Selector where TMarshal : DataMarshal<TData, TView> where TData : class, new() where TView : MonoBehaviour
    {
        protected abstract string _folderName { get; }
        protected abstract string _fileName { get; }
        protected abstract bool _isButtonInteractable { get; }
        
        protected TMarshal _dataMarshal;
        
        protected string _filePath => Path.Combine(_folderName, _fileName);

        public void SetDataMarshal(TMarshal dataMarshal)
        {
            _dataMarshal = dataMarshal;
        }

        private void Update()
        {
            _button.interactable = _isButtonInteractable;
        }
    }
}