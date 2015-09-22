using UnityEngine;
using System.Collections;
using GenericFunctions;

public class BabyCrow : MonoBehaviour {

	public GetHurt getHurtScript;

	public Animator babyCrowAnimator;
	public Rigidbody2D rigbod;
	public Transform jaiTransform;

	public Vector3[] shifty; 

	public Vector2 moveFullDir;

	public string crowString;

	public float triggerShiftDistance;
	public float speedDistance;
	public float zeroDistance;
	public float currentSpeed;
	public float minSpeed;
	public float moveSpeed;

	public int shifts;
	public int maxShifts;
	public int i;

	public bool switching;
	public bool shifting;
	public bool shiftingSequence;
	public bool faceDir;

	// Use this for initialization
	void Awake () {
		getHurtScript = GetComponent<GetHurt> ();
		rigbod = GetComponent<Rigidbody2D> ();
		babyCrowAnimator = GetComponent<Animator> ();
		moveSpeed = 2f;
		triggerShiftDistance = 0.1f;
		jaiTransform = GameObject.Find ("Jai").transform;
		shifty = new Vector3[]{
			new Vector3 (-.8f, 0.4f, 0f),
			new Vector3 (.8f, 0.4f, 0f),
			new Vector3 (-0.05f,  -1f,0f)
		};
		i = 0;
		maxShifts = 5;
		speedDistance = .3f;
		zeroDistance = 0.05f;
		minSpeed = 0.7f;
		shifts = 0;
		shiftingSequence = true;
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
		if (!shifting){
			transform.Face4ward(rigbod.velocity.x>0);
		}
		CorrectSpeed ();
		rigbod.velocity = moveFullDir.normalized * currentSpeed;
	}

	void ApproachShifts(){
		moveFullDir = (jaiTransform.position + shifty [i] - transform.position);
		if (moveFullDir.magnitude<triggerShiftDistance){
			StartCoroutine (ShiftSpots());
		}
	}

	public IEnumerator ShiftSpots(){
		if (!shifting){
			shifting = true;
			StartCoroutine (SwitchDirections());
			babyCrowAnimator.SetInteger("AnimState",1);
			yield return new WaitForSeconds (2f);
			i++;
			shifts++;
			if (i>2){
				i=0;
			}
			shifting = false;
			while (switching){
				yield return null;
			}
			babyCrowAnimator.SetInteger("AnimState",0);
			if (shifts>maxShifts){
				shiftingSequence = false;
			}
		}
		yield return null;
	}

	void FlyAway(){
		moveFullDir = (Vector3.right * Constants.worldDimensions.x * 1.2f - transform.position);
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

	public IEnumerator SwitchDirections(){
		if (shifting){
			switching = true;
			faceDir = !faceDir;
			transform.Face4ward(faceDir);
			yield return new WaitForSeconds (Random.Range(0.33f,.75f));
			switching = false;
			StartCoroutine (SwitchDirections());
		}
	}

}
