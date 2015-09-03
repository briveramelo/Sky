using UnityEngine;
using System.Collections;

public class GutSplosion : MonoBehaviour {

	public AudioSource gutSplode;

	// Use this for initialization
	void Awake () {
		gutSplode = GetComponent<AudioSource> ();
		StartCoroutine (DestroySelf ());
	}
	
	public IEnumerator DestroySelf(){
		while (gutSplode.isPlaying){
			yield return null;
		}
		Destroy (gameObject);
	}
}
