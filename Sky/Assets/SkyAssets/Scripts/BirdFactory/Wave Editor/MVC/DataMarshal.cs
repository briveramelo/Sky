using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public abstract class DataMarshal<TModel, TView> : MonoBehaviour, IHoldData where TModel : class, new() where TView : MonoBehaviour
    {
        public void CacheData()
        {
            _cachedData = Data;
        }
        public void CacheData(TModel data)
        {
            _cachedData = data;
        }
        
        private TModel _cachedData;
        public TModel GetCachedData() => _cachedData ?? (_cachedData = new TModel());//todo: nothing to do here. Just want to call out this simple "lazy" implementation!
        public abstract TModel Data { get; set; }
        public abstract bool IsDataReady { get; }
        
        protected abstract TView View { get; }
    }
    
    public interface IHoldData
    {
        bool IsDataReady { get; }
    }
}