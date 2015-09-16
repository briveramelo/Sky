using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Bat : MonoBehaviour {

	public Rigidbody2D rigbod;
	public Transform jaiTransform;
	public float vel;
	public float orbAng;
	public float targetAng;
	public float dist2Spot;
	public float distanceDelta;
	public float curvature;

	public float orbitSpeed;
	public float[] orbitXDistances;
	public float[] orbitYDistances;
	public float stepDistance;
	public float angleStep;
	public float angleTilt;
	public float speedPhaseShift;

	public Vector3[] targets;
	public Vector3 batPos;
	public Vector3 orbVec;
	public Vector3 targetSpot;

	public Vector2 moveDir;

	public int batXLayer;
	public int batYLayer;
	public int frameDelay;
	public int windowLength;
	public int i;
	public int used;

	public bool startingOff;
	public bool clockwise;
	public bool orbiting;


	// Use this for initialization
	void Awake () {
		rigbod = GetComponent<Rigidbody2D> ();
		startingOff = true;
		jaiTransform = GameObject.Find ("Jai").transform;
		orbitSpeed = 2.8f;
		frameDelay = 4;
		windowLength = 20;
		orbitXDistances = new float[]{
			2f, 3f, 4f
		};
		orbitYDistances = new float[]{
			2f, 3f, 4f
		};
		stepDistance = 0.5f;
		angleStep = 4f;
		dist2Spot = 10f;
		targets = new Vector3[windowLength];
		ShuffleDirections ();
		FindStartTarget ();
		FindTarget ();
		StartCoroutine (Orbit ());
	}

	void FindStartTarget(){
		orbAng = ConvertAnglesAndVectors.ConvertVector3Angle (transform.position - (jaiTransform.position + Constants.balloonOffset));
	}

	void ShuffleDirections(){
		angleTilt = Random.Range (-30f,30f);
		speedPhaseShift = Random.Range (-20f,20f);
		batXLayer = Random.Range (0,3);
		batYLayer = batXLayer == 1 ? 1+Mathf.RoundToInt(Mathf.Sign (Random.insideUnitCircle.x)) : 1;
		clockwise = Random.insideUnitCircle.x > 0;
		Invoke ("ShuffleDirections", Random.Range (2f, 4f));
	}

	void FindTarget(){
		if (!startingOff){
			if (clockwise){
				targetAng = orbAng+90f;
			}
			else{
				targetAng = orbAng-90f;
			}
			orbAng = Mathf.LerpAngle(orbAng,targetAng,Time.deltaTime*angleStep);
		}
		orbVec = new Vector3 ( orbitXDistances[batXLayer] * Mathf.Cos((orbAng+angleTilt) * Mathf.Deg2Rad), orbitYDistances[batYLayer] * Mathf.Sin(orbAng * Mathf.Deg2Rad),0f);
		ResetLagWindow ();
	}

	void ResetLagWindow(){
		targets[i] = jaiTransform.position + Constants.balloonOffset + orbVec;
		i++;
		if (i > windowLength-1) {
			if (startingOff){
				startingOff = false;
			}
			for (int queef = 0; queef<frameDelay; queef++){
				targets[queef] = targets[windowLength-frameDelay-1+queef];
			}
			i=frameDelay;
			used = 0;
		}
		used++;
	}

	public IEnumerator Orbit(){
		orbiting = true;
		while (orbiting) {
			vel = rigbod.velocity.magnitude;
			batPos = transform.position;
			dist2Spot = Vector2.Distance(targets[used],batPos);

			if (dist2Spot<stepDistance || startingOff){
				FindTarget();
			}

			moveDir = (targets[used] - batPos).normalized;
			curvature = (orbitXDistances[batXLayer] * orbitYDistances[batYLayer]) / Mathf.Pow ((orbitXDistances[batXLayer]*orbitXDistances[batXLayer] * Mathf.Sin((orbAng+speedPhaseShift) * Mathf.Deg2Rad) * Mathf.Sin((orbAng+speedPhaseShift) * Mathf.Deg2Rad) + orbitYDistances[batYLayer]*orbitYDistances[batYLayer] * Mathf.Cos(orbAng * Mathf.Deg2Rad) * Mathf.Cos(orbAng * Mathf.Deg2Rad)),1.5f);
			rigbod.velocity = moveDir * orbitSpeed / curvature;
			yield return null;
		}
		yield return null;
	}
}
