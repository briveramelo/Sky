using UnityEngine;
using System.Collections;
using PixelArtRotation;
using GenericFunctions;

public interface IMurderToCrow{
	void InitializeCrow(int crowNum);
	void TakeFlight(Vector2 crowPosition);
	bool ReadyToFly{get;}
}

public class Crow : Bird, IMurderToCrow {

	#region Initialize Variables
	ICrowToMurder murderInterface;
	[SerializeField] Murder murder;
	[SerializeField] PixelRotation pixelRotationScript;
	[SerializeField] Animator crowAnimator;

	protected override void Awake(){
		base.Awake();
		murderInterface = murder;
	}

	private enum CrowStates{
		Flapping = 0,
		Gliding = 1
	}
			
	Vector2 startPosition;
	Vector2 moveDir;

	float moveSpeed = 4.5f;
	float turnDistance = 2.5f;
	float commitDistance = 4f;
	float resetDistance = 15f;
	float requestNextCrowDistance = 4.5f;
	float currentDistance;

	bool isKiller;
	bool readyToFly = true;
    bool hasRequestedNext;
    public int myCrowNum;
	#endregion

	#region IMurderToCrow Interface
	void IMurderToCrow.InitializeCrow(int crowNum){
		isKiller = crowNum == 5;
        myCrowNum = crowNum;
        commitDistance = isKiller ? commitDistance : 3f;
	}
	void IMurderToCrow.TakeFlight(Vector2 crowPosition){
        SetPosition(crowPosition);
        hasRequestedNext = false;
        readyToFly = false;
		birdCollider.enabled = true;
		StartCoroutine ( TargetBalloons() );
		StartCoroutine ( RequestNextCrow());
	}
	bool IMurderToCrow.ReadyToFly{get{return readyToFly;}}
    void SetPosition(Vector2 crowPosition) {
        startPosition = crowPosition;
		transform.position = crowPosition;
    }
	#endregion

	IEnumerator RequestNextCrow(){
		while (currentDistance > requestNextCrowDistance){
			yield return null;
		}
		yield return null;
		murderInterface.SendNextCrow();
        hasRequestedNext = true;
	}

	IEnumerator TargetBalloons(){
		currentDistance=10f;
		while (currentDistance > commitDistance){
			currentDistance = Vector3.Distance(Constants.balloonCenter.position,transform.position);
			moveDir = (Constants.balloonCenter.position - transform.position).normalized;
			Swoop();
			yield return null;
		}
		yield return StartCoroutine (BeeLine());
		StartCoroutine (TriggerReset());
		if (!isKiller){
			StartCoroutine (TurnAwayFromBalloons());
		}
	}

	IEnumerator BeeLine(){
		while (currentDistance > turnDistance){
			currentDistance = Vector3.Distance(Constants.balloonCenter.position,transform.position);
			yield return null;
		}
	}

	IEnumerator TurnAwayFromBalloons(){
		crowAnimator.SetInteger("AnimState",(int)CrowStates.Gliding);
		int rotationSpeed = Bool.TossCoin() ? 4  : -4;
		int angleDelta = 0;
		int startAngle = ConvertAnglesAndVectors.ConvertVector2IntAngle(rigbod.velocity, false);
		while (Mathf.Abs(angleDelta)<60){
			angleDelta += rotationSpeed;
			moveDir = ConvertAnglesAndVectors.ConvertAngleToVector2(startAngle + angleDelta).normalized;
			Swoop();
			yield return null;
		}
		crowAnimator.SetInteger("AnimState",(int)CrowStates.Flapping);
	}

	#region Swoop Helper Functions
	void Swoop(){
		rigbod.velocity = moveDir * moveSpeed;
		pixelRotationScript.Angle = ConvertAnglesAndVectors.ConvertVector2IntAngle(rigbod.velocity);
		transform.FaceForward(rigbod.velocity.x>0);
	}

	IEnumerator TriggerReset(){
		while (currentDistance < resetDistance){
			currentDistance = Vector3.Distance(Constants.balloonCenter.position,transform.position);
			yield return null;
		}

		rigbod.velocity = Vector2.zero;
		birdCollider.enabled = false;
		transform.position = startPosition;
		readyToFly = true;
	}
	#endregion

	protected override void DieUniquely(){
		murderInterface.ReportCrowDown(this);
        if (!hasRequestedNext) {
            murderInterface.SendNextCrow();
        }
		base.DieUniquely();
	}
}