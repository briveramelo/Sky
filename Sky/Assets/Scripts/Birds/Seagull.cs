using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Seagull : Bird {

	[SerializeField] private GameObject pooNugget;

	private Vector2 targetPosition;
	private Vector2 orbVec;

	private Vector2 pooDistanceRange = new Vector2 (1f,1.5f);

	private float minMoveSpeed = 1.5f;
	private float moveSpeedHeight = 4f;
	private float swoopFocus = 2.5f;
	private float verticalOffset = 1.2f;
	private float swoopSpread;
	private float swoopHeight;
	private float orbAng;
	private float nextDistance;
	private float nextDistanceHeight;
	private float minNextDistance = 0.2f;
	private float lastTimePooped;

	protected override void Awake () {
		birdStats = new BirdStats(BirdType.Seagull);

		nextDistanceHeight = .3f;

		nextDistance = 0.2f;
		StartCoroutine (GetIntoPlace ());
		StartCoroutine (Poop ());
		base.Awake();
	}

	IEnumerator GetIntoPlace(){
		float placeSpeed = 3f;
		FindStartTarget ();
		while (true){
			targetPosition = orbVec + (Vector2)Constants.balloonCenter.position + Vector2.up*verticalOffset;
			rigbod.velocity = (targetPosition-(Vector2)transform.position).normalized * placeSpeed;
			if (Vector2.Distance(targetPosition,transform.position)<nextDistance){
				StartCoroutine (SwoopOverhead());
				break;
			}
			yield return null;
		}
		yield return null;
	}

	void FindStartTarget(){
		orbAng = ConvertAnglesAndVectors.ConvertVector3Angle (transform.position - (Constants.balloonCenter.position + Vector3.up*verticalOffset));
		swoopSpread = swoopFocus * Mathf.Cos(Mathf.Deg2Rad * orbAng) / (Mathf.Sin(Mathf.Deg2Rad * orbAng) *  Mathf.Sin(Mathf.Deg2Rad * orbAng) + 1f);
		swoopHeight = swoopSpread * Mathf.Sin(Mathf.Deg2Rad * orbAng) /2f;
		nextDistance = RaisedCos(minNextDistance,nextDistanceHeight,orbAng);
		orbVec = new Vector2 (swoopSpread,swoopHeight);
	}

	float RaisedCos(float minSpeed, float height, float ang){
		return height / 2f * Mathf.Cos (2f * Mathf.Deg2Rad * (ang + 90f)) + (minSpeed + height / 2f);
	}

	IEnumerator SwoopOverhead(){
		float currentSpeed = minMoveSpeed;
		float targetAng = 0;
		float angleStep = 4f;
		float minBoostDistance = 0.6f;
		float speedBoostFactor = 1f;
		bool clockwise = Random.value>0.5f;
		while (true){
			float distanceAway = Vector2.Distance(targetPosition,transform.position);
			if (distanceAway<nextDistance){
				targetAng = clockwise ? orbAng+90f : orbAng-90f;
				orbAng = Mathf.LerpAngle(orbAng,targetAng,Time.deltaTime*angleStep);
				swoopSpread = swoopFocus * Mathf.Cos(Mathf.Deg2Rad * orbAng) / (Mathf.Sin(Mathf.Deg2Rad * orbAng) *  Mathf.Sin(Mathf.Deg2Rad * orbAng) + 1f);
				swoopHeight = swoopSpread * Mathf.Sin(Mathf.Deg2Rad * orbAng) /2f;
				currentSpeed = RaisedCos(minMoveSpeed,moveSpeedHeight,orbAng);
				nextDistance = RaisedCos(minNextDistance,nextDistanceHeight,orbAng);
				orbVec = new Vector2 (swoopSpread,swoopHeight);
				targetPosition = orbVec + (Vector2)Constants.balloonCenter.position + Vector2.up * verticalOffset;
			}
			else if (distanceAway>minBoostDistance){
				currentSpeed += distanceAway * speedBoostFactor;
			}
			rigbod.velocity = (targetPosition-(Vector2)transform.position).normalized * currentSpeed;
			yield return null;
		}
	}

	IEnumerator Poop(){
		float minPoopTimeDelay = 4f;
		yield return new WaitForSeconds (minPoopTimeDelay);
		while (true){
			float xDist = Mathf.Abs (transform.position.x-Constants.jaiTransform.position.x);
			if (xDist>pooDistanceRange[0] && xDist<pooDistanceRange[1] && Mathf.Sign(rigbod.velocity.x)==Mathf.Sign(-transform.position.x+Constants.jaiTransform.position.x)){
				float actualLastPoopTime = 30f;
				foreach (Seagull seagullScript in FindObjectsOfType<Seagull>()){
					float lastGuysPooTime = Time.realtimeSinceStartup-seagullScript.lastTimePooped;
					if (lastGuysPooTime<actualLastPoopTime){
						actualLastPoopTime = lastGuysPooTime;
					}
				}
				if (Constants.poosOnJaisFace<3 && actualLastPoopTime>minPoopTimeDelay){
					(Instantiate (pooNugget,transform.position,Quaternion.identity) as GameObject).GetComponent<PooNugget>().InitializePooNugget(rigbod.velocity);
					lastTimePooped = Time.realtimeSinceStartup;
					break;
				}
			}
			yield return null;
		}
		StartCoroutine (Poop ());
		yield return null;
	}
}
