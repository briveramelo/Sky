using UnityEngine;
using System.Collections;
using GenericFunctions;

public class PooFinger : MonoBehaviour {

	public float pooCleanSpeed;
	public int pooSpotsToClean;
	public Joyfulstick joyfulstickScript;
	public Collider2D pooFingerCollider;
	
	void Awake(){
		pooSpotsToClean = 10;
		pooCleanSpeed = 0.1f;
		joyfulstickScript = GameObject.Find ("StickHole").GetComponent<Joyfulstick> ();
		pooFingerCollider = GetComponent<Collider2D> ();
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.gameObject.layer == Constants.pooSmearLayer){
			col.gameObject.layer = Constants.defaultLater;
			StartCoroutine (WipeThePooAway(col));
		}
	}

	public IEnumerator WipeThePooAway(Collider2D col){
		SpriteRenderer pooSprite = col.GetComponent<SpriteRenderer> ();
		Color startColor = pooSprite.color;
		float percentComplete = 0;
		while (percentComplete<1){
			percentComplete += pooCleanSpeed;
			if (pooSprite){
				pooSprite.color = Color.LerpUnclamped (startColor,Color.clear,percentComplete);
			}
			yield return null;
		}
		if (col){
			Destroy (col.gameObject);
		}
		yield return null;
	}

	public IEnumerator CheckForSufficientCleaning(GameObject pooNugget, GameObject pooSmear, Seagull gullScript){
		pooFingerCollider.enabled = true;
		yield return new WaitForSeconds (1f);
		if (Constants.totalPooSpots-pooSmear.transform.childCount>pooSpotsToClean){//cleaned
			foreach (Collider2D pooCol in pooSmear.GetComponentsInChildren<Collider2D>()){
				StartCoroutine (WipeThePooAway(pooCol));
			}
			joyfulstickScript.pooOnYou--;
			if (joyfulstickScript.pooOnYou<1){
				pooFingerCollider.enabled = false;
			}
			if (gullScript){
				gullScript.directHit = false;
				gullScript.tempPause = true;
			}
			if (pooSmear){
				Destroy(pooSmear,1f);
			}
			if (pooNugget){
				Destroy(pooNugget,1f);
			}
		}
		else{
			StartCoroutine (CheckForSufficientCleaning(pooNugget, pooSmear,gullScript));
		}
		yield return null;
	}
}
