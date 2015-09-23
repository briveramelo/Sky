using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Seagull : MonoBehaviour {

	public Joyfulstick joyfulstickScript;

	public Transform jaiTransform;

	public Rigidbody2D rigbod;

	public Vector3 targetPosition;
	public Vector3 orbVec;

	public Vector2 pooDistanceRange;

	public float currentSpeed;
	public float minMoveSpeed;
	public float moveSpeedHeight;
	public float placeSpeed;
	public float swoopSpread;
	public float swoopHeight;
	public float swoopFocus;
	public float orbAng;
	public float targetAng;
	public float minPoopTimeDelay;
	public float poopForce;
	public float xDist;
	public float xDistLast;
	public float angleStep;
	public float nextDistance;
	public float nextDistanceHeight;
	public float minNextDistance;
	public float minBoostDistance;
	public float speedBoostFactor;
	public float lastTimePooped;

	public bool swooping;
	public bool settingUp;
	public bool clockwise;
	public bool pooped;
	public bool directHit;
	public bool tempPause;

	// Use this for initialization
	void Awake () {
		settingUp = true;
		rigbod = GetComponent<Rigidbody2D> ();
		jaiTransform = GameObject.Find ("Jai").transform;
		joyfulstickScript = GameObject.Find ("StickHole").GetComponent<Joyfulstick> ();
		minMoveSpeed = 1.5f;
		moveSpeedHeight = 4f;
		nextDistanceHeight = .3f;
		minNextDistance = 0.2f;
		minBoostDistance = 0.6f;
		speedBoostFactor = 1f;
		currentSpeed = minMoveSpeed;
		placeSpeed = 3f;
		angleStep = 4f;
		nextDistance = 0.2f;
		minPoopTimeDelay = 4f;
		poopForce = 70f;
		pooDistanceRange = new Vector2 (1f,1.5f);
		swoopFocus = 2.5f;
		StartCoroutine (GetIntoPlace ());
		StartCoroutine (Poop ());
	}

	void FindStartTarget(){
		orbAng = ConvertAnglesAndVectors.ConvertVector3Angle (transform.position - (jaiTransform.position + Constants.seagullOffset));
		swoopSpread = swoopFocus * Mathf.Cos(Mathf.Deg2Rad * orbAng) / (Mathf.Sin(Mathf.Deg2Rad * orbAng) *  Mathf.Sin(Mathf.Deg2Rad * orbAng) + 1f);
		swoopHeight = swoopSpread * Mathf.Sin(Mathf.Deg2Rad * orbAng) /2f;
		nextDistance = RaisedCos(minNextDistance,nextDistanceHeight,orbAng);
		orbVec = new Vector3 (swoopSpread,swoopHeight,0f);
	}

	public IEnumerator GetIntoPlace(){
		FindStartTarget ();
		while (settingUp){
			targetPosition = orbVec + jaiTransform.position + Constants.seagullOffset;
			rigbod.velocity = (targetPosition-transform.position).normalized * placeSpeed;
			if (Vector2.Distance(targetPosition,transform.position)<nextDistance){
				settingUp = false;
				swooping = true;
				StartCoroutine (SwoopOverhead());
			}
			yield return null;
		}
		yield return null;
	}

	float RaisedCos(float minSpeed, float height, float ang){
		return height / 2f * Mathf.Cos (2f * Mathf.Deg2Rad * (ang + 90f)) + (minSpeed + height / 2f);
	}

	public IEnumerator SwoopOverhead(){
		while (swooping){
			float distanceAway = Vector2.Distance(targetPosition,transform.position);
			if (distanceAway<nextDistance){
				if (clockwise){
					targetAng = orbAng+90f;
				}
				else{
					targetAng = orbAng-90f;
				}
				orbAng = Mathf.LerpAngle(orbAng,targetAng,Time.deltaTime*angleStep);
				swoopSpread = swoopFocus * Mathf.Cos(Mathf.Deg2Rad * orbAng) / (Mathf.Sin(Mathf.Deg2Rad * orbAng) *  Mathf.Sin(Mathf.Deg2Rad * orbAng) + 1f);
				swoopHeight = swoopSpread * Mathf.Sin(Mathf.Deg2Rad * orbAng) /2f;
				currentSpeed = RaisedCos(minMoveSpeed,moveSpeedHeight,orbAng);
				nextDistance = RaisedCos(minNextDistance,nextDistanceHeight,orbAng);
				orbVec = new Vector3 (swoopSpread,swoopHeight,0f);
				targetPosition = orbVec + jaiTransform.position + Constants.seagullOffset;
			}
			else if (distanceAway>minBoostDistance){
				currentSpeed += distanceAway * speedBoostFactor;
			}
			rigbod.velocity = (targetPosition-transform.position).normalized * currentSpeed;
			yield return null;
		}
		yield return null;
	}

	public IEnumerator Poop(){
		yield return new WaitForSeconds (minPoopTimeDelay);
		while (!pooped){
			if (tempPause){
				tempPause = false;
				yield return new WaitForSeconds (minPoopTimeDelay);
			}
			xDist = Mathf.Abs (transform.position.x-jaiTransform.position.x);
			if (xDist>pooDistanceRange[0] && xDist<pooDistanceRange[1] && Mathf.Sign(rigbod.velocity.x)==Mathf.Sign(-transform.position.x+jaiTransform.position.x)){
				Seagull[] seagullScripts = FindObjectsOfType<Seagull>();
				float actualLastPoopTime = 30f;
				foreach (Seagull seagullScript in seagullScripts){
					float lastGuysPooTime = Time.realtimeSinceStartup-seagullScript.lastTimePooped;
					if (lastGuysPooTime<actualLastPoopTime){
						actualLastPoopTime = lastGuysPooTime;
					}
				}
				if (joyfulstickScript.pooOnYou<3 && !directHit && actualLastPoopTime>minPoopTimeDelay){
					GameObject pooNugget = Instantiate (Resources.Load (Constants.pooNuggetPrefab),transform.position,Quaternion.identity) as GameObject;
					pooNugget.GetComponent<Rigidbody2D>().velocity = rigbod.velocity;
					pooNugget.GetComponent<PooNugget>().seagullScript = this;
					pooped = true;
					lastTimePooped = Time.realtimeSinceStartup;
				}
			}
			yield return null;
		}
		pooped = false;
		StartCoroutine (Poop ());
		yield return null;
	}

	void OnDestroy(){
		StopAllCoroutines();
	}
}
