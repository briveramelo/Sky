using UnityEngine;
using System.Collections;
using GenericFunctions;

public class PooSlide : MonoBehaviour {

	public GameObject maskCamera;

	public WorldEffects worldEffectsScript;
	public Seagull gullScript;
	public Joyfulstick joyfulstickScript;
	public MaskCamera maskCameraScript;

	public Animator pooAnimator;
	public Camera maskCameraCamera;

	public Rigidbody2D rigbod;

	public float slideSpeed;

	public int targetPooInt;

	public bool sliding;

	void Awake(){
		rigbod = GetComponent<Rigidbody2D> ();
		pooAnimator = GetComponent<Animator> ();
		worldEffectsScript = GameObject.Find ("WorldBounds").GetComponent<WorldEffects> ();
		targetPooInt = worldEffectsScript.targetPooInt;
		worldEffectsScript.targetPooInt++;
		if (worldEffectsScript.targetPooInt>Constants.pooSplatPrefabs.Length-1){
			worldEffectsScript.targetPooInt = 0;
		}
		joyfulstickScript = GameObject.Find ("StickHole").GetComponent<Joyfulstick> ();
		joyfulstickScript.pooOnYou++;
		maskCamera = Instantiate (Resources.Load ("Prefabs/World/MaskCamera"), Vector3.zero, Quaternion.identity) as GameObject;
		maskCamera.transform.parent = transform;
		maskCameraCamera = maskCamera.GetComponent<Camera> ();
		RenderTexture rt = Resources.Load (Constants.pooSplatRenderTextures [targetPooInt], typeof(RenderTexture)) as RenderTexture;
//		foreach (Camera cam in FindObjectsOfType<Camera>()){
//			if (cam.targetTexture == rt && cam != maskCameraCamera){
//				Destroy (cam.transform.parent.gameObject);
//			}
//		}
		maskCameraCamera.targetTexture = rt;
		maskCameraScript = maskCamera.GetComponent<MaskCamera> ();
		maskCameraScript.pooSliderTransform = transform;
		maskCameraScript.startDifference = transform.position;
		maskCameraScript.firstFrame = true;
		slideSpeed = .39f;
		sliding = true;
		StartCoroutine (SlideDown ());
		Destroy (gameObject,15f);
		Invoke ("PooAgain", 8f);
		StartCoroutine (AllowCleansing ());
	}

	void PooAgain(){
		if (gullScript){
			gullScript.directHit = false;
		}
	}

	public IEnumerator AllowCleansing(){
		while (pooAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime<1f){
			yield return null;
		}
		Destroy (pooAnimator);
		GetComponent<SpriteRenderer>().sprite = Resources.Load (Constants.pooSplatLastSprites[targetPooInt], typeof(Sprite)) as Sprite;
	}

	public IEnumerator SlideDown(){
		while (sliding){
			rigbod.velocity = Vector2.down * slideSpeed;
			yield return null;
		}
	}

	void OnDestroy(){
		joyfulstickScript.pooOnYou--;
		StopAllCoroutines ();
	}
}
