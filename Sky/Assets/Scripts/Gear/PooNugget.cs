using UnityEngine;
using System.Collections;
using GenericFunctions;

public class PooNugget : MonoBehaviour {

	public bool splat;
	public Seagull seagullScript;
	public SpriteRenderer nuggetSprite;

	// Use this for initialization
	void Awake () {
		nuggetSprite = GetComponent<SpriteRenderer> ();
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
		GameObject pooSplat = Instantiate (Resources.Load (Constants.pooSplatPrefab), Vector3.zero, Quaternion.identity) as GameObject;
		pooSplat.GetComponent<PooSlide>().gullScript = seagullScript;
		nuggetSprite.enabled = false;
		if (seagullScript){
			seagullScript.directHit = true;
		}
		Destroy(gameObject);
		yield return null;
	}
}
