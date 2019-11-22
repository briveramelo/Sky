using System.IO;
using BRM.DebugAdapter;
using BRM.FileSerializers;
using BRM.FileSerializers.Interfaces;
using BRM.TextSerializers;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public abstract class LoadButton<TMarshal, TData, TView> : FileButton<TMarshal, TData, TView> where TMarshal : DataMarshal<TData, TView> where TData : class, new() where TView : MonoBehaviour
    {
        protected IReadFiles _fileReader = new TextFileSerializer(new UnityJsonSerializer(), new UnityDebugger());
        protected override bool _isButtonInteractable => File.Exists(_filePath);
    }
}