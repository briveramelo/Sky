using UnityEngine;
using System.Collections;
using GenericFunctions;

public class PooNugget : MonoBehaviour {

	public WorldEffects worldEffectsScript;
	public Seagull seagullScript;

	public SpriteRenderer nuggetSprite;
	
	public bool splat;

	// Use this for initialization
	void Awake () {
		worldEffectsScript = GameObject.Find ("WorldBounds").GetComponent<WorldEffects> ();
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
		GameObject pooSplat = Instantiate (Resources.Load (Constants.pooSplatPrefabs[worldEffectsScript.targetPooInt])/*Random.insideUnitCircle.x > 0 ? 0 :1])*/, Vector3.zero, Quaternion.identity) as GameObject;
		PooSlide pooSlideScript = pooSplat.GetComponent<PooSlide> ();
		pooSlideScript.gullScript = seagullScript;
		nuggetSprite.enabled = false;

		if (seagullScript){
			seagullScript.directHit = true;
		}
		Destroy(gameObject);
		yield return null;
	}
}
