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
	public float resetDistance;

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
			new Vector3(0f,-5.5f,0f),
			new Vector3(3f,.5f,0f),
			new Vector3(-3,.5f,0f),
			new Vector3(0f,3f,0f)
		};
		distanceThreshold = 0.2f;
		moveSpeeds = new float[]{
			2.5f, 3f, 3f, 9f
		};
		curveAngles = new float[]{
			20f, 30f, 30f, 10f
		}; //negative means clockwise, positive means counter-clockwise
		spotsHit = new bool[4];
		clockwiseSteps = new bool[4];
		resetDistance = 4.5f;
		ticker = 0;
		StartCoroutine(GetSet());
	}

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

	void CheckToMoveOn(){
		if (ticker == 0 || ticker == 3){
			if (Mathf.Abs (transform.position.x-targetPosition.x)<distanceThreshold){
				spotsHit[ticker] = true;
			}
		}
		else if (ticker == 1 || ticker== 2){
			if (transform.position.y>targetPosition.y){
				spotsHit[ticker] = true;
			}
		}
	}

	public IEnumerator GetSet(){
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
				rigbod.velocity = moveDir * moveSpeeds[1];
				yield return null;
			}
			ticker++;
			if (ticker ==1){
				if (goLeft){
					ticker = 2;
				}
			}
			else if (ticker == 2){
				ticker =3;
			}
			yield return null;
		}
		rigbod.velocity = Vector2.zero;
		yield return new WaitForSeconds (0.3f);
		//pelicanAnimator.SetInteger("AnimState",1);
		yield return new WaitForSeconds (0.2f);
		StartCoroutine (DiveBomb ());
	}
	
	public IEnumerator DiveBomb(){
		rigbod.velocity = Vector2.down * moveSpeeds [3];
		while (transform.position.y>-6f){
			yield return null;
		}
		//pelicanAnimator.SetInteger("AnimState",0);
		rigbod.velocity = Vector2.zero;
		pelicanCollider.enabled = false;
		yield return new WaitForSeconds (2f);
		StartCoroutine (GetSet ());
		while (transform.position.y<-5f){
			yield return null;
		}
		pelicanCollider.enabled = true;
		yield return null;
	}

}
