using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Pelican : MonoBehaviour {

	public Rigidbody2D rigbod;
	public Animator pelicanAnimator;
	public Collider2D pelicanCollider;

	public Transform jaiTransform;

	public Vector3[] setPositions;
	public Vector3 targetPosition;

	public Vector2 moveDir;
	
	public float[] moveSpeeds;
	public float[] curveAngles;
	public float distanceThreshold;
	public float distanceToTarget;
	public float angleDelta;
	public float outputAngle;
	public float inputAngle;
	public float maxDistance;

	public int ticker;

	public bool[] spotsHit;
	public bool[] clockwiseSteps;
	public bool goLeft;

	// Use this for initialization
	void Awake () {
		jaiTransform = GameObject.Find ("Jai").transform;
		rigbod = GetComponent<Rigidbody2D> ();
		pelicanCollider = GetComponent<Collider2D> ();
		pelicanAnimator = GetComponent<Animator> ();
		setPositions = new Vector3[]{
			new Vector3(0f,-3.5f,0f),
			new Vector3(1.75f,.25f,0f),
			new Vector3(-1.75f,.25f,0f),
			new Vector3(0f,1.25f,0f)
		};
		distanceThreshold = 0.2f;
		moveSpeeds = new float[]{
			2.5f, 3f, 3f, 3f, 9f
		};
		curveAngles = new float[]{
			20f, 30f, 30f, 5f
		}; //negative means clockwise, positive means counter-clockwise
		spotsHit = new bool[4];
		clockwiseSteps = new bool[4];
		ticker = 0;
		StartCoroutine(SwoopAround());
	}

	//bring in that nice curve to his flight pattern
	Vector2 CurveItIn(Vector2 inputVector, Vector3 targetSpot, float maxAngleDelta, bool clockwise){
		inputAngle = ConvertAnglesAndVectors.ConvertVector2FloatAngle (inputVector);
		if (distanceToTarget>maxDistance){
			maxDistance = distanceToTarget;
		}
		angleDelta = maxAngleDelta;
		if (!clockwise){
			angleDelta = -angleDelta;
		}

		outputAngle = inputAngle + angleDelta;
		return ConvertAnglesAndVectors.ConvertAngleToVector2 (outputAngle).normalized;
	}

	//set his flight pattern from the getgo
	void NextSteps(){
		goLeft = false;
		if ((targetPosition.x>transform.position.x && targetPosition.y<transform.position.y)){//it's downright
			clockwiseSteps = new bool[]{false,false,false,false};
		}
		else if (targetPosition.x>transform.position.x && targetPosition.y>transform.position.y){ //it's upright
			clockwiseSteps = new bool[]{true,false,false,false};
		}
		else if ((targetPosition.x<transform.position.x && targetPosition.y<transform.position.y)){ //it's downleft
			clockwiseSteps = new bool[]{true,true,true,true};
			goLeft = true;
		}
		else { //it's upleft
			clockwiseSteps = new bool[]{false,true,true,true};
			goLeft = true;
		}
	}

	//determine if he should move on to the next checkpoint / inflection point in his curved flight
	void CheckToMoveOn(){
		if (ticker == 0 || ticker == 3){
			if (Mathf.Abs (transform.position.x-targetPosition.x)<distanceThreshold){
				spotsHit[ticker] = true;
			}
		}
		else if (ticker == 1 || ticker == 2){
			if (transform.position.y>targetPosition.y-.1f){
				spotsHit[ticker] = true;
			}
		}
	}

	//move from one checkpoint to another
	public IEnumerator SwoopAround(){
		ticker = 0;
		spotsHit = new bool[4];
		targetPosition = jaiTransform.position + Constants.balloonOffset + setPositions [ticker];
		NextSteps();
		while (ticker<4){	
			maxDistance = Vector3.Distance(transform.position,targetPosition);
			targetPosition = jaiTransform.position + Constants.balloonOffset + setPositions [ticker];
			while (!spotsHit[ticker]) {
				targetPosition = jaiTransform.position + Constants.balloonOffset + setPositions [ticker];
				distanceToTarget = Vector3.Distance(transform.position,targetPosition);
				moveDir = CurveItIn((targetPosition - transform.position).normalized, targetPosition, curveAngles[ticker],clockwiseSteps[ticker]);
				CheckToMoveOn();
				rigbod.velocity = moveDir * moveSpeeds[ticker];
				yield return null;
			}
			ticker++;
			//decide whether to go left or go right based on that NextSteps function (ticker = 1 means right, ticker = 2 means left)
			if (ticker == 1){
				if (goLeft){
					ticker = 2;
				}
			}
			else if (ticker == 2){
				ticker = 3;
			}
			else if (ticker == 4){
				rigbod.velocity = Vector2.zero;
			}
			yield return null;
		}
		yield return new WaitForSeconds (0.3f);
		//pelicanAnimator.SetInteger("AnimState",1);
		yield return new WaitForSeconds (0.2f);
		StartCoroutine (DiveBomb ());
	}

	//plunge to certain balloon popping glory
	public IEnumerator DiveBomb(){
		rigbod.velocity = Vector2.down * moveSpeeds [4];
		while (transform.position.y>-Constants.worldDimensions.y-1f){
			yield return null;
		}
		//pelicanAnimator.SetInteger("AnimState",0);
		rigbod.velocity = Vector2.zero;
		pelicanCollider.enabled = false;
		yield return new WaitForSeconds (2f);
		StartCoroutine (SwoopAround ());
		while (transform.position.y<-Constants.worldDimensions.y){
			yield return null;
		}
		pelicanCollider.enabled = true;
		yield return null;
	}

}
