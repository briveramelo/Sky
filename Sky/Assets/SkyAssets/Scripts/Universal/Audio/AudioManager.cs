using System;
using UnityEngine;
using System.Collections.Generic;
using BRM.EventBrokers;
using BRM.EventBrokers.Interfaces;

public enum AudioClipType
{
    GenerateGuts,
    ButtonPress,
    Pause,
    UnPause,
    BalloonPop,
    SpearUse=10,
    BalloonsInvincible,
    BalloonsVincible,
    BasketRebirth,
}

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _generateGutsClip;
    [SerializeField] private AudioClip _pauseClip;
    [SerializeField] private AudioClip _unPauseClip;
    [SerializeField] private AudioClip _balloonPopClip;
    [SerializeField] private AudioClip _spearUseClip;
    [SerializeField] private AudioClip _balloonsInvincibleClip;
    [SerializeField] private AudioClip _balloonsVincibleClip;
    [SerializeField] private AudioClip _basketRebirthClip;

    private static Dictionary<AudioClipType, AudioSourcePool> _audioSourcePools = new Dictionary<AudioClipType, AudioSourcePool>();
    private static AudioSource _fallbackAudio;
    private IBrokerEvents _eventBroker = new StaticEventBroker();

    private void Start()
    {
        _eventBroker.Subscribe<PauseData>(OnPause);
        _fallbackAudio = _audioSource;
        
        //world
        _audioSourcePools.Add(AudioClipType.GenerateGuts, new AudioSourcePool(new GameObject(nameof(_generateGutsClip)), transform, _generateGutsClip, 13, 3, false));
        _audioSourcePools.Add(AudioClipType.SpearUse, new AudioSourcePool(new GameObject(nameof(_spearUseClip)), transform, _spearUseClip, 3, 1, false));
        _audioSourcePools.Add(AudioClipType.BalloonPop, new AudioSourcePool(new GameObject(nameof(_balloonPopClip)), transform, _balloonPopClip, 1, 1, false));
        _audioSourcePools.Add(AudioClipType.BalloonsInvincible, new AudioSourcePool(new GameObject(nameof(_balloonsInvincibleClip)), transform, _balloonsInvincibleClip, 1, 1, false));
        _audioSourcePools.Add(AudioClipType.BalloonsVincible, new AudioSourcePool(new GameObject(nameof(_balloonsVincibleClip)), transform, _balloonsVincibleClip, 1, 1, false));
        _audioSourcePools.Add(AudioClipType.BasketRebirth, new AudioSourcePool(new GameObject(nameof(_basketRebirthClip)), transform, _basketRebirthClip, 1, 1, false));
        
        //ui
        _audioSourcePools.Add(AudioClipType.ButtonPress, new AudioSourcePool(new GameObject(nameof(_pauseClip)), transform, _pauseClip, 1, 1, true));
        _audioSourcePools.Add(AudioClipType.Pause, new AudioSourcePool(new GameObject(nameof(_pauseClip)), transform, _pauseClip, 1, 1, true));
        _audioSourcePools.Add(AudioClipType.UnPause, new AudioSourcePool(new GameObject(nameof(_unPauseClip)), transform, _unPauseClip, 1, 1, true));
    }

    private void OnDestroy()
    {
        _eventBroker.Unsubscribe<PauseData>(OnPause);
    }

    private void OnPause(PauseData data)
    {
        AudioListener.pause = data.IsPaused;
    }

    public static float PlayAudio(AudioClipType clipType, float delay = -1f)
    {
        if (_audioSourcePools.TryGetValue(clipType, out var pool))
        {
            var audioSource = pool.GetAvailable();
            if (delay > 0f)
            {
                audioSource.PlayDelayed(delay);
                return audioSource.clip.length + delay;
            }

            audioSource.Play();
            return audioSource.clip.length;
        }
        
        Debug.LogError("No Audio Clip found");
        return 0f;
    }
}