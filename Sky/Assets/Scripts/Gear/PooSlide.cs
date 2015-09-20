using UnityEngine;
using System.Collections;

public class PooSlide : MonoBehaviour {

	public Seagull gullScript;
	public Joyfulstick joyfulstickScript;
	public MaskCamera maskCameraScript;
	public Rigidbody2D rigbod;
	public float slideSpeed;
	public bool sliding;

	void Awake(){
		rigbod = GetComponent<Rigidbody2D> ();
		joyfulstickScript = GameObject.Find ("StickHole").GetComponent<Joyfulstick> ();
		joyfulstickScript.pooOnYou++;
		maskCameraScript = GameObject.Find ("Mask Camera").GetComponent<MaskCamera> ();
		maskCameraScript.pooSliderTransform = transform;
		maskCameraScript.startDifference = transform.position;
		maskCameraScript.firstFrame = true;
		//StartCoroutine (maskCameraScript.ResetMask (GetComponent<SpriteRenderer>()));
		slideSpeed = .385f;
		sliding = true;
		StartCoroutine (SlideDown ());
		Destroy (gameObject,60f);
		foreach (PooSlide pooSlideScript in FindObjectsOfType<PooSlide>()){
			if (pooSlideScript && pooSlideScript!=this){
				Destroy(pooSlideScript.gameObject);
			}
		}
	}

	public IEnumerator SlideDown(){
		while (sliding){
			rigbod.velocity = Vector2.down * slideSpeed;
			yield return null;
		}
	}

	void OnDestroy(){
		joyfulstickScript.pooOnYou--;
		if (gullScript){
			gullScript.directHit = false;
		}
		StopAllCoroutines ();
	}
}
