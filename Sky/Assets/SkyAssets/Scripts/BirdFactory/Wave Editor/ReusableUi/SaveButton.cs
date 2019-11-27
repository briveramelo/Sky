using BRM.DebugAdapter;
using BRM.FileSerializers;
using BRM.FileSerializers.Interfaces;
using BRM.TextSerializers;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public abstract class SaveButton<TMarshal, TData, TView> : FileButton<TMarshal, TData, TView> where TMarshal : DataMarshal<TData, TView> where TData : class, new() where TView : MonoBehaviour
    {
        protected IWriteFiles _fileWriter = new TextFileSerializer(new UnityJsonSerializer(), new UnityDebugger());
        protected override bool _isButtonInteractable => _dataMarshal != null && _dataMarshal.IsDataReady;

        protected override void OnClick()
        {
            string filePath = FileUtilities.GetUniqueFilePath(_filePath);
            _fileWriter.Write(filePath, _dataMarshal.Data);
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
            base.OnClick();
        }
    }
}