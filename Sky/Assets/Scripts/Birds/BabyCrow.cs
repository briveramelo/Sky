using UnityEngine;
using System.Collections;

public class BabyCrow : MonoBehaviour {

	public GetHurt getHurtScript;

	public Animator babyCrowAnimator;
	public Rigidbody2D rigbod;
	public Transform basketTransform;

	public Vector3[] shifty; 
	public Vector3 pixelScale;
	public Vector3 pixelScaleReversed;

	public Vector2 moveFullDir;

	public string crowString;

	public float triggerShiftDistance;
	public float speedDistance;
	public float zeroDistance;
	public float currentSpeed;
	public float minSpeed;
	public float switchTime;
	public float moveSpeed;

	public int shifts;
	public int maxShifts;
	public int i;

	public bool switching;
	public bool shifting;
	public bool shiftingSequence;

	// Use this for initialization
	void Awake () {
		getHurtScript = GetComponent<GetHurt> ();
		rigbod = GetComponent<Rigidbody2D> ();
		babyCrowAnimator = GetComponent<Animator> ();
		moveSpeed = 2f;
		switchTime = 0.25f;
		pixelScale = Vector3.one * 3.125f;
		pixelScaleReversed = new Vector3 (-3.125f,3.125f,1f);
		triggerShiftDistance = 0.05f;
		basketTransform = GameObject.Find ("Jai").transform;
		shifty = new Vector3[]{
			new Vector3 (-1.0f, 0.7f, 0f),
			new Vector3 (1.3f , 0.7f, 0f),
			new Vector3 (0.1f,  -1f,0f)
		};
		i = 0;
		maxShifts = 5;
		speedDistance = .3f;
		zeroDistance = 0.05f;
		minSpeed = 0.55f;
		shifts = 0;
		shiftingSequence = true;
		shifting = true;
		crowString = "Prefabs/Birds/Murder";
		switching = false;
		StartCoroutine (FlyFree ());
	}

	public IEnumerator FlyFree(){
		yield return new WaitForSeconds (25f);
		shiftingSequence = false;
	}

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
		moveFullDir = (basketTransform.position + shifty [i] - transform.position);
		if (moveFullDir.magnitude<triggerShiftDistance){
			StartCoroutine (ShiftSpots());
		}
	}

	public IEnumerator ShiftSpots(){
		if (shifting){
			shifting = false;
			babyCrowAnimator.SetInteger("AnimState",1);
			yield return new WaitForSeconds (2f);
			i++;
			shifts++;
			if (i>2){
				i=0;
			}
			shifting = true;
			babyCrowAnimator.SetInteger("AnimState",0);
			if (shifts>maxShifts){
				shiftingSequence = false;
			}
		}
		yield return null;
	}

	void FlyAway(){
		moveFullDir = (Vector3.right * 9f - transform.position);
		if (moveFullDir.magnitude<triggerShiftDistance){
			getHurtScript.summonCrows = false;
			Destroy(gameObject);
		}
	}

	void CorrectSpeed(){
		if (moveFullDir.magnitude<speedDistance){
			currentSpeed = moveSpeed * moveFullDir.magnitude;
			if (currentSpeed<minSpeed){
				currentSpeed = minSpeed;
			}
			if (moveFullDir.magnitude<zeroDistance){
				currentSpeed = 0;
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
				transform.localScale = pixelScale;
			}
			else{
				StartCoroutine ( SwitchDirections());
				transform.localScale = pixelScaleReversed;
			}
		}
	}

	public IEnumerator SwitchDirections(){
		switching = true;
		yield return new WaitForSeconds (switchTime);
		switching = false;
	}

}
