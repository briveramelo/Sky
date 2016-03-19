using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Bat : Bird {

	// Simulate a bat's erratic flight pattern
	// Set to approach and orbit the player and his vulnerable balloons

	// Intentional following-lag is introduced to allow for collision between bat / balloon
		// From a game design standpoint, this forces the player to slow movement to avoid this dangerous collision


	#region Initialize Variables
	private Vector2[] targetPositions;
	private Vector2 ellipseTrace;
	private Vector2 moveDir;
	private Vector2 batPos;

	private float[] orbitalRadii = new float[]{1.25f, 1.5f, 1.75f};

	private float approachSpeed = 2.8f;
	private float orbitDistance = 1.5f;
	private float stepDistance = 0.4f;

	private float dist2Target;
	private float ellipseTilt;
	private float speedPhaseShift;
	private float ellipseAng;
	private float curvature;

	private int positionWindowLength = 20;
	private int realTimeIndex;
	private int targetIndex;
	private int xRadiusIndex;
	private int yRadiusIndex;

	private bool clockwise;
	#endregion

	protected override void Awake () {
		birdStats = new BirdStats(BirdType.Bat);
		targetPositions = new Vector2[positionWindowLength];
		StartCoroutine (Approach ());
		base.Awake();
	}

	// Flap straight towards the first point on the elliptical perimeter
	// Start orbiting once close enough
	private IEnumerator Approach(){
		dist2Target = 10f;

		while (true){
			UpdatePositionIndices();
			batPos = (Vector2)transform.position;
			targetPositions[realTimeIndex] = (Vector2)Constants.balloonCenter.position - orbitDistance * rigbod.velocity.normalized;
			dist2Target = Vector2.Distance(targetPositions[targetIndex], batPos);

			// Once close enough, stop approaching and start orbiting
			if (dist2Target<orbitDistance){
				StartCoroutine (Orbit());
				break;
			}

			moveDir = (targetPositions[targetIndex] - batPos).normalized;
			rigbod.velocity = moveDir * approachSpeed;
			transform.FaceForward (transform.position.x > Constants.balloonCenter.position.x);
			yield return null;
		}
	}

	void UpdatePositionIndices(){
		realTimeIndex++;
		targetIndex++;
		if (realTimeIndex >= positionWindowLength)
			ResetTargetPositionWindow ();
	}

	// Overwrite the first few frames of the bat's positional-targetting array with the last few
	// Set the targetIndex "frameDelay" #frames before the realTime Index
	// In effect, create a following delay to allow for bat/balloon collision
	void ResetTargetPositionWindow(){
		int frameDelay=3;
		for (int i = 0; i<frameDelay; i++)
			targetPositions[i] = targetPositions[positionWindowLength-frameDelay-1+i];
		
		realTimeIndex = frameDelay;
		targetIndex = 0;
	}

	// Move in an ellipse
	// Set speed proportional to the curvature of the point on the ellipse
	private IEnumerator Orbit(){
		ShuffleOrbitalPhase();
		ellipseAng = ConvertAnglesAndVectors.ConvertVector2FloatAngle(-rigbod.velocity);
		targetPositions[realTimeIndex] = FindEllipsePosition();

		while (true) {
			batPos = (Vector2)transform.position;
			dist2Target = Vector2.Distance(targetPositions[targetIndex], batPos);
			if (dist2Target<stepDistance){
				UpdatePositionIndices();
				ellipseAng = FindTargetAngle();
				targetPositions[realTimeIndex] = FindEllipsePosition();
			}
			
			moveDir = (targetPositions[targetIndex] - batPos).normalized; 
			curvature = (orbitalRadii[xRadiusIndex] * orbitalRadii[yRadiusIndex]) / 
				Mathf.Pow ((orbitalRadii[xRadiusIndex]*orbitalRadii[xRadiusIndex] * 
					Mathf.Sin((ellipseAng+speedPhaseShift) * Mathf.Deg2Rad) * Mathf.Sin((ellipseAng+speedPhaseShift) * Mathf.Deg2Rad) + 
					orbitalRadii[yRadiusIndex] * orbitalRadii[yRadiusIndex] * 
					Mathf.Cos(ellipseAng * Mathf.Deg2Rad) * Mathf.Cos(ellipseAng * Mathf.Deg2Rad)),1.5f);
			float orbitSpeed = approachSpeed / curvature;

			rigbod.velocity = moveDir * orbitSpeed;
			transform.FaceForward (transform.position.x > Constants.balloonCenter.position.x);
			yield return null;
		}
	}

	// Set properties of the ellipse
	// Randomize these properties periodically for erratic flight
	void ShuffleOrbitalPhase(){
		ellipseTilt = Random.Range (-30f,30f);
		speedPhaseShift = ellipseTilt * 2f / 3f;
		xRadiusIndex = Random.Range (0,3);
		yRadiusIndex = xRadiusIndex == 1 ? 1+(int)Mathf.Sign (Random.insideUnitCircle.x) : 1;
		clockwise = Random.value > 0.5f;
		Invoke ("ShuffleOrbitalPhase", Random.Range (2f, 4f));
	}

	// Help define the elliptical pattern
	float FindTargetAngle(){
		float angleStep = 4f;
		float targetAng = clockwise ? ellipseAng+90f : ellipseAng-90f;
		return Mathf.LerpAngle( ellipseAng, targetAng, Time.deltaTime * angleStep);
	}

	// Set an elliptical pattern around the balloons
	// Update position indices & ensure the Bat is facing the player
	Vector2 FindEllipsePosition(){
		ellipseTrace = new Vector2 ( orbitalRadii[xRadiusIndex] * Mathf.Cos((ellipseAng+ellipseTilt) * Mathf.Deg2Rad),
			orbitalRadii[yRadiusIndex] * Mathf.Sin(ellipseAng * Mathf.Deg2Rad));
		return (Vector2)Constants.balloonCenter.position + ellipseTrace;
	}
}
