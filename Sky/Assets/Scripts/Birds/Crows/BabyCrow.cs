using UnityEngine;
using System.Collections;
using GenericFunctions;

public class BabyCrow : Bird {

	[SerializeField] private Animator babyCrowAnimator;

	private Vector2[] basketOffsets = new Vector2[]{
		new Vector2 (-.8f, 0.4f),
		new Vector2 (.8f, 0.4f),
	};

	private Vector2 moveDir;
	private float dist2Target;

	private float triggerShiftDistance = 0.1f;
	private float speedDistance = .3f;
	private float zeroDistance = 0.05f;
	private float minSpeed = 0.71f;
	private float moveSpeed = 2f;
	private float currentSpeed;

	private int currentShift;
	private int shiftsHit;
	private int maxShifts = 5;
	private int basketOffsetIndex;
	private int BasketOffsetIndex{
		get{return basketOffsetIndex;}
		set{
			basketOffsetIndex = value;
			if (basketOffsetIndex>1)
				basketOffsetIndex =0;
		}
	}
	private enum AnimState{
		Flying=0,
		Looking=1
	}
	bool killedYoung;
			
	protected override void Awake () {
		birdStats = new BirdStats(BirdType.BabyCrow);
		StartCoroutine(ApproachShifts());
		base.Awake();
	}

	IEnumerator ApproachShifts(){
		while (currentShift<maxShifts){
			dist2Target = Vector2.Distance(Constants.jaiTransform.position + (Vector3)basketOffsets [BasketOffsetIndex],transform.position);
			moveDir = (Constants.jaiTransform.position + (Vector3)basketOffsets [BasketOffsetIndex] - transform.position).normalized;
			currentSpeed = CorrectSpeed ();
			rigbod.velocity = moveDir * currentSpeed;

			if (dist2Target<triggerShiftDistance && shiftsHit == currentShift){
				StartCoroutine (ShiftSpots());
			}
			yield return null;
		}
		rigbod.velocity = Vector2.zero;
		StartCoroutine (FlyAway());
	}

	IEnumerator ShiftSpots(){
		shiftsHit++;
		babyCrowAnimator.SetInteger("AnimState",(int)AnimState.Looking);
		yield return StartCoroutine (LookBackAndForth(transform.position.x<Constants.balloonCenter.position.x));
		babyCrowAnimator.SetInteger("AnimState",(int)AnimState.Flying);
		currentShift++;
		BasketOffsetIndex++;
	}

	IEnumerator FlyAway(){
		dist2Target = Vector2.Distance(Vector3.right * Constants.worldDimensions.x * 1.2f, transform.position);

		while (dist2Target > triggerShiftDistance){
			dist2Target = Vector2.Distance(Vector3.right * Constants.worldDimensions.x * 1.2f, transform.position);
			moveDir = (Vector3.right * Constants.worldDimensions.x * 1.2f - transform.position).normalized;
			currentSpeed = CorrectSpeed ();
			rigbod.velocity = moveDir * currentSpeed;
			yield return null;
		}
		Destroy(gameObject);
	}

	float CorrectSpeed(){
		if (dist2Target<speedDistance){
			currentSpeed = moveSpeed * dist2Target;
			if (dist2Target<zeroDistance){
				return 0;
			}
			if (currentSpeed<minSpeed){
				return minSpeed;
			}
			return currentSpeed;
		}
		else{
			return moveSpeed;
		}
	}

	IEnumerator LookBackAndForth(bool faceDir){
		for (int i=0; i<7; i++){
			transform.Face4ward(faceDir);
			yield return new WaitForSeconds (Random.Range(0.33f,.75f));
			faceDir = !faceDir;
		}
	}
		
	protected override void PayTheIronPrice (){
		Incubator.Instance.SpawnNextBird(BirdType.Crow);
	}
}