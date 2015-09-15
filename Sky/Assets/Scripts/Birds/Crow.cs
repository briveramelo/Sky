using UnityEngine;
using System.Collections;
using System.Linq;
using PixelArtRotation;
using GenericFunctions;

public class Crow : MonoBehaviour {

	public Basket basketScript;
	public PixelRotation pixelRotationScript;
	public Murder murderScript;

	public Rigidbody2D rigbod;
	public CircleCollider2D crowCollider;
	public Transform targetTransform;
	public Animator crowAnimator;

	public Vector3 startPosition;
	public Vector3 targetPosition;

	public Vector2 moveDir;

	public float moveSpeed;
	public float turnDistance;
	public float commitDistance;
	public float resetDistance;
	public float lastDistance;
	public float currentDistance;
	public float triggerDistance;

	public int crowNumber;
	public int newAngle;
	public int currentAngle;
	public int maxAngleDelta;
	public int angleDelta;
	public int targetAngle;
	public int rotationSpeed;

	public bool isKiller;
	public bool swooping;
	public bool turned;
	public bool turning;
	public bool committed;
	public bool reset;
	public bool triggeredNextCrow;
	

	// Use this for initialization
	void Awake () {
		rigbod = GetComponent<Rigidbody2D> ();
		crowAnimator = GetComponent<Animator> ();
		crowCollider = GetComponent<CircleCollider2D> ();
		pixelRotationScript = GetComponent<PixelRotation> ();
		crowCollider.enabled = false;
		basketScript = GameObject.Find ("BalloonBasket").GetComponent<Basket>();
		murderScript = transform.parent.gameObject.GetComponent<Murder>();
		moveSpeed = 7f;
		turnDistance = 4.5f;
		commitDistance = 5f; 
		triggerDistance = 6f;
		resetDistance = 20f;//needs to be greater than commitDistance
		RandomizeRedirection ();
		targetTransform = GameObject.Find ("Jai").transform;
		lastDistance = 100f;
	}

	void RandomizeRedirection(){
		int i = Random.Range(0,2);
		if (i==0){
			maxAngleDelta = 60;
			rotationSpeed = 5;
		}
		else{
			maxAngleDelta = -60;
			rotationSpeed = -5;
		}
	}

	void Update(){
		if (swooping){ //approach balloons
			currentDistance = Vector3.Distance(targetTransform.position + Constants.balloonOffset,transform.position);
			if (currentDistance < triggerDistance && !triggeredNextCrow){ //trigger next Crow
				triggeredNextCrow = true;
			}
			if (committed){ //targetPoint is now fixed
				if (currentDistance < turnDistance && !turned && !turning){ //trigger reset position
					currentAngle = ConvertAnglesAndVectors.ConvertVector2IntAngle(rigbod.velocity);
					targetAngle = currentAngle + maxAngleDelta;
					turned = true;
					if (!isKiller){
						turning = true;
						crowAnimator.SetInteger("AnimState",1);
					}
					StartCoroutine (TriggerReset());
				}
				else if (currentDistance > resetDistance && !reset){
					reset = true;
				}
				else if (turning){ //turning
					currentAngle = ConvertAnglesAndVectors.ConvertVector2IntAngle(rigbod.velocity);
					newAngle = currentAngle + rotationSpeed;
					angleDelta += rotationSpeed;
					moveDir = ConvertAnglesAndVectors.ConvertAngleToVector2(newAngle);
					if (Mathf.Abs(angleDelta)>Mathf.Abs (maxAngleDelta)){
						turning = false;
						crowAnimator.SetInteger("AnimState",0);
					}
				}
			}
			else{
				moveDir = targetTransform.position + Constants.balloonOffset - transform.position;
				if (currentDistance < commitDistance){
					committed = true;
					targetPosition = targetTransform.position + Constants.balloonOffset;
				}
			}

			rigbod.velocity = (moveDir).normalized * moveSpeed;
			pixelRotationScript.Angle = ConvertAnglesAndVectors.ConvertVector2IntAngle(rigbod.velocity,rigbod.velocity.x);
			AnimateIt();
			lastDistance = Vector3.Distance(targetTransform.position + Constants.balloonOffset,transform.position);
		}
	}

	void AnimateIt(){
		if (rigbod.velocity.x<0){
			transform.localScale = Constants.Pixel625(false);
		}
		else{
			transform.localScale = Constants.Pixel625(true);
		}
	}

	public IEnumerator TriggerReset(){
		while (!reset){
			yield return null;
		}
		StartCoroutine (ResetPosition ());
	}

	public IEnumerator ResetPosition(){
		swooping = false;
		isKiller = false;
		turned = false;
		turning = false;
		committed = false;
		reset = false;
		triggeredNextCrow = false;
		angleDelta = 0;
		rigbod.velocity = Vector2.zero;
		crowCollider.enabled = false;
		crowAnimator.SetInteger("AnimState",0);
		RandomizeRedirection ();
		transform.position = startPosition;
		yield return null;
	}

	void OnDestroy(){
		if (isKiller){
			murderScript.killerIsAlive = false;
		}
		murderScript.crowScripts [crowNumber].turned = true;
		murderScript.crowsToGo = murderScript.crowsToGo.Where (number => number!=crowNumber).ToArray();
	}
}
