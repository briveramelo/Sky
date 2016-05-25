using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Seagull : Bird {

    [SerializeField] GameObject pooNugget;

    bool movingRight;
    float sinPeriodShift = Random.Range(0f,5f);

    int mySeagullNumber;
    static int totalSeagulls;
	const float moveSpeed = 3f;
    Vector2 TargetCenterPosition;
    float xSpread = 4;

    Vector2[] targetPositions = new Vector2[] {
        new Vector2 (0f,2.25f),
        new Vector2 (.25f,2.35f),
        new Vector2 (-.5f,2.25f),
        new Vector2 (.5f,2.25f),
        new Vector2 (-.25f,2.35f),
        new Vector2 (0f,2.35f),
    };

    #region WakeUp
    protected override void Awake () {
		base.Awake();

        InitializeThisSeagull();
		StartCoroutine (GetIntoPlace ());
		StartCoroutine (Poop ());
	}

    void InitializeThisSeagull() {
        mySeagullNumber = totalSeagulls;
        TargetCenterPosition = targetPositions[mySeagullNumber % targetPositions.Length];
        movingRight = transform.position.x < TargetCenterPosition.x;
        startedMovingRight = movingRight;
        transform.FaceForward(!movingRight);
    }
    #endregion

    IEnumerator GetIntoPlace(){
		while (true){
			rigbod.velocity = (TargetCenterPosition-(Vector2)transform.position).normalized * moveSpeed;
			if (Vector2.Distance(TargetCenterPosition,transform.position)<0.1f){
                StartCoroutine (SwoopOverhead());
				break;
			}
			yield return null;
		}
	}

    #region SwoopOverhead
    bool startedMovingRight;
    IEnumerator SwoopOverhead(){
        rigbod.velocity = Vector2.zero;
        rigbod.isKinematic = true;
        startTime = Time.time;
        StartCoroutine(LerpShift(startedMovingRight));
		while (true){
            transform.FaceForward(GetXVelocity()<0);
            transform.position = new Vector2 (GetXPosition(), GetYPosition());
            yield return null;
		}
	}
    float shift;
    float startTime;
    IEnumerator LerpShift(bool startedGoingRight) {
        float targetShift = -22.5f;
        float xVel=0;
        while (Mathf.Abs(shift - targetShift)>0.1f) {
            shift = Mathf.SmoothDamp(shift, targetShift, ref xVel, 2f);
            yield return new WaitForEndOfFrame();
        }
        shift = targetShift;
    }

    float GetYPosition(){
        return (0.6f) * Mathf.Sin(2f*Mathf.PI/(5f) * (Time.time - startTime)) + TargetCenterPosition.y;
	}
    float GetYVelocity(){
        return (0.6f) * 2f*Mathf.PI/(5f) * Mathf.Sin(2f*Mathf.PI/(5f) * (Time.time - startTime)) + TargetCenterPosition.y;
	}

    float GetXPosition(){
        return (startedMovingRight ? 1 :-1) * xSpread * Mathf.Sin(2f*Mathf.PI/(5f*2f) * (Time.time - startTime) + Mathf.Deg2Rad * shift) + TargetCenterPosition.x;
	}
    float GetXVelocity() {
        return (startedMovingRight ? 1 :-1) * 2f*Mathf.PI/(5f*2f) * xSpread * Mathf.Sign(Mathf.Cos(2f*Mathf.PI/(5f*2f) * (Time.time - startTime) + Mathf.Deg2Rad * shift));
    }
    #endregion

    static int activePooCams;
    public static void LogPooCam(bool hit) {
        activePooCams+= hit ?1:-1;
    }
    IEnumerator Poop(){
		float minPoopTimeDelay = 1f;
        Vector2 pooDistanceRange = new Vector2 (1f,1.5f) * 0.8f;
		yield return new WaitForSeconds (minPoopTimeDelay);
		while (true){
			float xDist = Mathf.Abs (transform.position.x-Constants.jaiTransform.position.x);
            float lastTimePooped=0f;
			if (xDist > pooDistanceRange[0] && xDist < pooDistanceRange[1] && Mathf.Sign(GetXVelocity())==Mathf.Sign(-transform.position.x+Constants.jaiTransform.position.x)){
                if (activePooCams<5) {
                    if (Time.time>(lastTimePooped + minPoopTimeDelay)){
					    (Instantiate (pooNugget,transform.position,Quaternion.identity) as GameObject).GetComponent<PooNugget>().InitializePooNugget(new Vector2 (GetXVelocity(), GetYVelocity()));
                        lastTimePooped = Time.time;
					    break;
				    }
                }
			}
			yield return null;
		}
		StartCoroutine (Poop ());
	}
}
