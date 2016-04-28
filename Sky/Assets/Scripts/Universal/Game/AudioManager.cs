using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    [SerializeField] AudioSource myAudioSource; static AudioSource MyAudioSource;

    void Awake () {
        if (MyAudioSource == null) {
            MyAudioSource = myAudioSource;
        }
	}

    public static void PlayAudio(AudioClip audioClip) {
        MyAudioSource.PlayOneShot(audioClip);
    }
    public static void PlayReadyDelayed(float delay) {
        MyAudioSource.PlayDelayed(delay);
    }
    public static IEnumerator PlayDelayedAudio(AudioClip audioClip, float delay) {
        yield return new WaitForSeconds(delay);
        MyAudioSource.PlayOneShot(audioClip);
    }
	
}
