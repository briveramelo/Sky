using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    
    private static AudioSource _myAudioSource;

    private void Awake()
    {
        _myAudioSource = _audioSource;
    }

    public static void PlayAudio(AudioClip audioClip)
    {
        _myAudioSource.PlayOneShot(audioClip);
    }

    public static void PlayDelayed(float delay)
    {
        _myAudioSource.PlayDelayed(delay);
    }
}