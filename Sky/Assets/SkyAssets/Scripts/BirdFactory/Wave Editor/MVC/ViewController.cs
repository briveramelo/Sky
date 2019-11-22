using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public abstract class SelectorViewController<TView> : Selector where TView : MonoBehaviour
    {
        protected abstract TView View { get; }
    }
    public abstract class ViewController<TView> : MonoBehaviour where TView : MonoBehaviour
    {
        protected abstract TView View { get; }
    }
}