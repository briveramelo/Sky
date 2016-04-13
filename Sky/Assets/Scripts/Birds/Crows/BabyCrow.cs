using UnityEngine;
using System.Collections;
using GenericFunctions;

public class BabyCrow : Bird {

	[SerializeField] private Animator babyCrowAnimator;

	private Vector2[] basketOffsets = new Vector2[]{
		new Vector2 (-.8f, 0.1f),
		new Vector2 (.8f, 0.1f),
	};

	private Vector2 moveDir;
	private float dist2Target;

	const float triggerShiftDistance = 0.3f;
	const float minSpeed = 0.71f;
	const float moveSpeed = 2f;

	private int currentShift;
	private int shiftsHit;
	const int maxShifts = 5;
	private int basketOffsetIndex;
	private int BasketOffsetIndex{
		get{return basketOffsetIndex;}
		set{
			basketOffsetIndex = value;
			if (basketOffsetIndex>1){
				basketOffsetIndex =0;
			}
		}
	}
	float CorrectSpeed{
		get{
			return dist2Target<0.4f? Mathf.Lerp(rigbod.velocity.magnitude,0f,Time.deltaTime *2f) : moveSpeed;
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
		transform.FaceForward(transform.position.x<Constants.balloonCenter.position.x);
		while (currentShift<maxShifts){
			dist2Target = Vector2.Distance(Constants.jaiTransform.position + (Vector3)basketOffsets [BasketOffsetIndex],transform.position);
			moveDir = (Constants.jaiTransform.position + (Vector3)basketOffsets [BasketOffsetIndex] - transform.position).normalized;
			rigbod.velocity = moveDir * CorrectSpeed;

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
		dist2Target = Vector2.Distance(Vector3.right * Constants.WorldDimensions.x * 1.2f, transform.position);

		while (dist2Target > triggerShiftDistance){
			dist2Target = Vector2.Distance(Vector3.right * Constants.WorldDimensions.x * 1.2f, transform.position);
			moveDir = (Vector3.right * Constants.WorldDimensions.x * 1.2f - transform.position).normalized;
			rigbod.velocity = moveDir * moveSpeed;
			yield return null;
		}
		Destroy(gameObject);
	}

	IEnumerator LookBackAndForth(bool faceDir){
		for (int i=0; i<7; i++){
			transform.FaceForward(faceDir);
			yield return new WaitForSeconds (Random.Range(0.33f,.75f));
			faceDir = !faceDir;
		}
	}
		
	protected override void DieUniquely (){
		Incubator.Instance.SpawnNextBird(BirdType.Crow);
		base.DieUniquely();
	}
}
