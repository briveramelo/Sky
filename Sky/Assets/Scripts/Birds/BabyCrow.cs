using UnityEngine;
using System.Collections;

public class BabyCrow : MonoBehaviour {

	public float moveSpeed;
	public Vector2 moveFullDir;
	public Rigidbody2D rigbod;
	public Transform balloonBasketTransform;
	public Vector3[] shifty; 
	public int i;
	public bool shifting;
	public bool shiftingSequence;
	public int shifts;
	public int maxShifts;
	public string crowString;
	public float triggerShiftDistance;
	public float speedDistance;
	public float currentSpeed;
	public float minSpeed;
	public Vector3 pixelScale;
	public Vector3 pixelScaleReversed;
	public float switchTime;
	public bool switching;
	public bool summonCrows;
	public SummonTheCrows summonTheCrowsScript;

	// Use this for initialization
	void Awake () {
		rigbod = GetComponent<Rigidbody2D> ();
		summonTheCrowsScript = GameObject.Find ("WorldBounds").GetComponent<SummonTheCrows> ();
		moveSpeed = 2f;
		switchTime = 0.25f;
		pixelScale = Vector3.one * 3.125f;
		pixelScaleReversed = new Vector3 (-3.125f,3.125f,1f);
		triggerShiftDistance = 0.05f;
		balloonBasketTransform = GameObject.Find ("Jai").transform;
		shifty = new Vector3[]{
			new Vector3 (-.9f, 0.1f, 0f),
			new Vector3 (1.1f , 0.1f, 0f),
			new Vector3 (0.1f,  -1.5f,0f)
		};
		i = 0;
		maxShifts = 5;
		speedDistance = .3f;
		minSpeed = .6f;
		shifts = 0;
		shiftingSequence = true;
		summonCrows = true;
		shifting = false;
		crowString = "Prefabs/Birds/Murder";
		switching = false;
	}

	public IEnumerator FlyFree(){
		yield return new WaitForSeconds (25f);
		shiftingSequence = false;
	}

	// Update is called once per frame
	void Update () {
		if (shiftingSequence){
			ApproachShifts ();
		}
		else{
			FlyAway();
		}
		CorrectSpeed ();
		FaceCorrectly ();
		rigbod.velocity = moveFullDir.normalized * currentSpeed;
	}

	void ApproachShifts(){
		moveFullDir = (balloonBasketTransform.position + shifty [i] - transform.position);
		if (moveFullDir.magnitude<triggerShiftDistance){
			StartCoroutine (ShiftSpots());
		}
	}

	void FlyAway(){
		moveFullDir = (Vector3.one * 9f - transform.position);
		if (moveFullDir.magnitude<triggerShiftDistance){
			summonCrows = false;
			Destroy(gameObject);
		}
	}

	void CorrectSpeed(){
		if (moveFullDir.magnitude<speedDistance){
			currentSpeed = moveSpeed * moveFullDir.magnitude;
			if (currentSpeed<minSpeed){
				currentSpeed = minSpeed;
			}
		}
		else{
			currentSpeed = moveSpeed;
		}
	}

	void FaceCorrectly(){
		if (!switching){
			if (rigbod.velocity.x>0){
				StartCoroutine ( SwitchDirections());
				transform.localScale = pixelScaleReversed;
			}
			else{
				StartCoroutine ( SwitchDirections());
				transform.localScale = pixelScale;
			}
		}
	}

	public IEnumerator SwitchDirections(){
		switching = true;
		yield return new WaitForSeconds (switchTime);
		switching = false;
	}

	void OnDestroy(){
		if (summonCrows){
			StartCoroutine (summonTheCrowsScript.Murder());
		}
	}

	public IEnumerator ShiftSpots(){
		if (!shifting){
			shifting = true;
			yield return new WaitForSeconds (2f);
			i++;
			shifts++;
			if (i>2){
				i=0;
			}
			shifting = false;
			if (shifts>maxShifts){
				shiftingSequence = false;
			}
		}
		yield return null;
	}


}
