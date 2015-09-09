using UnityEngine;
using System.Collections;
using System.Linq;

public class Crow : MonoBehaviour {

	public Rigidbody2D rigbod;
	public BalloonBasket balloonBasketScript;
	public Murder murderScript;
	public BoxCollider2D crowBox;

	public Transform targetTransform;

	public Vector3 startPosition;
	public Vector3 targetPosition;
	public Vector3 extraTargetHeight;
	public Vector2 moveDir;
	public Vector2 correctionVector;

	public bool isKiller;
	public bool swooping;
	public bool turned;
	public bool committed;
	public bool reset;

	public int crowNumber;

	public float moveSpeed;
	public float sin45;
	public float correctionScalar;
	public float turnDistance;
	public float commitDistance;
	public float resetDistance;
	public float lastDistance;
	public float currentDistance;
	

	// Use this for initialization
	void Awake () {
		rigbod = GetComponent<Rigidbody2D> ();
		crowBox = GetComponent<BoxCollider2D> ();
		crowBox.enabled = false;
		balloonBasketScript = GameObject.Find ("BalloonBasket").GetComponent<BalloonBasket>();
		murderScript = transform.parent.gameObject.GetComponent<Murder>();
		startPosition = transform.position;
		moveSpeed = 7.5f;
		turnDistance = .9f;
		commitDistance = 6f;
		resetDistance = 20f;
		sin45 = Mathf.Sin (Mathf.Deg2Rad * 75f);
		correctionScalar = sin45 / (sin45 - 1f);
		correctionVector = new Vector2 (sin45, sin45 + correctionScalar);
		int i = 0;
		targetTransform = GameObject.Find ("Jai").transform;
		extraTargetHeight = Vector3.up * 1.3f;
		lastDistance = 100f;
	}

	void Update(){
		if (swooping){
			currentDistance = Vector3.Distance(targetTransform.position + extraTargetHeight,transform.position);
			if (committed){
				if ((currentDistance < turnDistance || currentDistance>lastDistance) && !turned){
					if (!isKiller){
						moveDir = (rigbod.velocity.normalized * correctionScalar + correctionVector);
					}
					turned = true;
					StartCoroutine (PauseToReset());
				}
				else if (currentDistance > resetDistance && !reset){
					reset = true;
				}
			}
			else{
				moveDir = targetTransform.position + extraTargetHeight - transform.position;
				if (currentDistance < commitDistance){
					committed = true;
					targetPosition = targetTransform.position + extraTargetHeight;
				}
			}

			rigbod.velocity = (moveDir).normalized * moveSpeed;
			lastDistance = Vector3.Distance(targetTransform.position + extraTargetHeight,transform.position);
		}
	}

	public IEnumerator PauseToReset(){
		while (!reset){
			yield return null;
		}
		StartCoroutine (ResetPosition ());
	}

	public IEnumerator ResetPosition(){
		swooping = false;
		isKiller = false;
		turned = false;
		committed = false;
		reset = false;
		rigbod.velocity = Vector2.zero;
		crowBox.enabled = false;
		transform.position = startPosition;
		yield return null;
	}

	void OnDestroy(){
		murderScript.crowScripts [crowNumber].turned = true;
		murderScript.crowsToGo = murderScript.crowsToGo.Where (number => number!=crowNumber).ToArray();
	}
}
