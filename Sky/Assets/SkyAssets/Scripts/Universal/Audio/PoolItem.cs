using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public interface IPool<T>
{
    T GetAvailable();
    void Reset();
}

public class AudioSourcePool : PoolItem<AudioSource>
{
    public string Name => _gameObject.name;
    private GameObject _gameObject;
    private Transform _transform => _gameObject.transform;
    private AudioClip _clip;
    private bool _isUnscaledTime;
    private uint _expansionIncrement;
    private uint _startingPoolSize;
    
    public AudioSourcePool(GameObject gameObject, Transform parent, AudioClip clip, uint startingPoolSize, uint expansionIncrement, bool isUnscaledTime)
    {
        _gameObject = gameObject;
        _transform.SetParent(parent);
        _clip = clip;
        _startingPoolSize = startingPoolSize;
        _expansionIncrement = expansionIncrement;
        _isUnscaledTime = isUnscaledTime;
        Initialize(startingPoolSize);
    }

    public override AudioSource GetAvailable()
    {
        var available = _poolItems.Find(item => !item.isPlaying);
        if (available == null)
        {
            ExpandPool(_expansionIncrement);
            return GetAvailable();
        }

        return available;
    }

    public override void Reset()
    {
        for (int i = 0; i < _poolItems.Count; i++)
        {
            Object.Destroy(_poolItems[i]);
        }

        Initialize(_startingPoolSize);
    }

    protected override void ExpandPool(uint numToAdd)
    {
        Assert.IsTrue(numToAdd > 0);
        for (int i = 0; i < numToAdd; i++)
        {
            var source = _gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.ignoreListenerPause = _isUnscaledTime;
            source.clip = _clip;
            _poolItems.Add(source);
        }
    }
}

public abstract class PoolItem<T> : IPool<T>
{
    protected List<T> _poolItems;

    protected void Initialize(uint startingPoolSize)
    {
        Assert.IsTrue(startingPoolSize > 0);
        _poolItems = new List<T>();
        ExpandPool(startingPoolSize);
    }

    public abstract T GetAvailable();
    public abstract void Reset();
    protected abstract void ExpandPool(uint numToAdd);
}