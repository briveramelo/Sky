using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public abstract class DataMarshal<T> : MonoBehaviour, IHoldData where T : class
    {
        public abstract T GetData { get; }
        public abstract bool IsDataReady { get; }
    }
    public interface IHoldData
    {
        bool IsDataReady { get; }
    }
}