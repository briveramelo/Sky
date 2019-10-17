using UnityEngine;
using System.Collections;
using PixelArtRotation;
using GenericFunctions;

public class Eagle : Bird {

    [SerializeField] EagleFriends eagleFriends; ITriggerSpawnable myEagleFriends;
	[SerializeField] PixelRotation pixelRotationScript;
	Vector3 attackDir;

	private Vector2[] startPos = new Vector2[]{
		new Vector2(-Constants.WorldDimensions.x,-Constants.WorldDimensions.y) * 1.2f,
		new Vector2(Constants.WorldDimensions.x,-Constants.WorldDimensions.y) * 1.2f
	};
	private Vector2[] moveDir;
		
	protected override void Awake () {
		base.Awake();
		myEagleFriends = eagleFriends;
		moveDir = new[]{
			Constants.ScreenDimensions.normalized,
			new Vector2(-Constants.ScreenDimensions.x,Constants.ScreenDimensions.y).normalized,
		};
		StartCoroutine (InitiateAttack (1f));
	}

	IEnumerator InitiateAttack(float waitTime){
        yield return null;
        myEagleFriends.TriggerSpawnEvent();
		yield return new WaitForSeconds(waitTime);
		StartCoroutine (SweepUp (true));
	}

	IEnumerator SweepUp(bool first){
		float moveSpeed = 5f;
		transform.FaceForward(first);
		transform.position = startPos [first ? 0 : 1];
		rigbod.velocity = moveDir [first ? 0 : 1] * moveSpeed;
		pixelRotationScript.Angle = ConvertAnglesAndVectors.ConvertVector2IntAngle (moveDir [first ? 0 : 1]);
		yield return new WaitForSeconds(first ? 4f : 6f);
		if (first){
			StartCoroutine (SweepUp (false));
		}
		else{
			rigbod.velocity = Vector2.zero;
			Strike ();
		}
	}
		
	void Strike(){
		float xStartPoint = 20f;
		while (Mathf.Abs(xStartPoint)>Constants.WorldDimensions.x){
			xStartPoint = Constants.balloonCenter.position.x + Random.Range (-Constants.WorldDimensions.x, Constants.WorldDimensions.x) * .15f;
		}

		float strikeSpeed = 9f;
		transform.position = new Vector2 (xStartPoint, Constants.WorldDimensions.y * 1.2f);
		attackDir = (Constants.balloonCenter.position - transform.position).normalized;
		rigbod.velocity = attackDir * strikeSpeed;
		transform.FaceForward(attackDir.x>0);
		pixelRotationScript.Angle = ConvertAnglesAndVectors.ConvertVector2IntAngle (attackDir);

		StartCoroutine (InitiateAttack(5f));
	}
}
