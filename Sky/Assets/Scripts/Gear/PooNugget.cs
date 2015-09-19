using UnityEngine;
using System.Collections;
using GenericFunctions;

public class PooNugget : MonoBehaviour {

	public bool splat;
	public PooFinger pooFingerScript;
	public Joyfulstick joyfulstickScript;
	public Seagull seagullScript;
	public SpriteRenderer pooSprite;

	// Use this for initialization
	void Awake () {
		pooFingerScript = GameObject.Find ("PooFinger").GetComponent<PooFinger> ();
		joyfulstickScript = GameObject.Find ("StickHole").GetComponent<Joyfulstick> ();
		pooSprite = GetComponent<SpriteRenderer> ();
		Invoke ("CheckForHit", 3f);
	}

	void CheckForHit(){
		if (!splat){
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.layer == Constants.faceLayer && !splat){
			splat = true;
			StartCoroutine (PooSplatter());
		}
	}

	public IEnumerator PooSplatter(){
		GameObject pooSmear = Instantiate (Resources.Load (Constants.pooSmearPrefab), Vector3.zero, Quaternion.identity) as GameObject;
		joyfulstickScript.pooOnYou++;
		StartCoroutine (pooFingerScript.CheckForSufficientCleaning (gameObject, pooSmear, seagullScript));
		pooSprite.enabled = false;
		Destroy(GetComponent<Rigidbody2D> ());
		if (seagullScript){
			seagullScript.directHit = true;
		}
		yield return null;
	}
}
