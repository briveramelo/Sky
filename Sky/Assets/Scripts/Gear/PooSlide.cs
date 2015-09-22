using UnityEngine;
using System.Collections;

public class PooSlide : MonoBehaviour {

	public GameObject maskCamera;
	public Seagull gullScript;
	public Joyfulstick joyfulstickScript;
	public MaskCamera maskCameraScript;
	public Camera maskCameraCamera;
	public Rigidbody2D rigbod;
	public float slideSpeed;
	public bool sliding;

	void Awake(){
		rigbod = GetComponent<Rigidbody2D> ();
		joyfulstickScript = GameObject.Find ("StickHole").GetComponent<Joyfulstick> ();
		joyfulstickScript.pooOnYou++;
		maskCamera = Instantiate (Resources.Load ("Prefabs/World/MaskCamera"), Vector3.zero, Quaternion.identity) as GameObject;
		maskCameraCamera = maskCamera.GetComponent<Camera> ();
		//maskCameraCamera.targetTexture = Resources.Load ("Materials/PooSplat1_RenderTexture", typeof(RenderTexture)) as RenderTexture;

		maskCameraScript = maskCamera.GetComponent<MaskCamera> ();
		maskCameraScript.pooSliderTransform = transform;
		maskCameraScript.startDifference = transform.position;
		maskCameraScript.firstFrame = true;
		//StartCoroutine (maskCameraScript.ResetMask (GetComponent<SpriteRenderer>()));
		slideSpeed = .39f;
		sliding = true;
		StartCoroutine (SlideDown ());
		Destroy (gameObject,60f);
		Destroy (maskCamera,60f);
//		foreach (PooSlide pooSlideScript in FindObjectsOfType<PooSlide>()){
//			if (pooSlideScript!=null && pooSlideScript!=this){
//				//Destroy(pooSlideScript.gameObject);
//			}
//		}
		//Destroy (GetComponent<Animator> (),1f);
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
