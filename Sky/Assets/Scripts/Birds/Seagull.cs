using UnityEngine;
using System.Collections;
using GenericFunctions;
using System.Collections.Generic;

public interface ISeagull {
    void UpdateSeagullNumber(int seagullNumber);
}

public class Seagull : Bird, ISeagull {

    [SerializeField] GameObject pooNugget;

    bool movingRight;
    float sinPeriodShift = Random.Range(0f,5f);

    int seagullNumber;
	const float moveSpeed = 3f;
	float lastTimePooped;
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

        FindObjectOfType<SeagullSyncer>().GetComponent<ISeagullSyncer>().RecordNewSeagull(this);
		StartCoroutine (GetIntoPlace ());
		StartCoroutine (Poop ());
	}

    void ISeagull.UpdateSeagullNumber(int seagullNumber) {
        this.seagullNumber = seagullNumber;
        TargetCenterPosition = targetPositions[seagullNumber % targetPositions.Length];
        movingRight = transform.position.x < TargetCenterPosition.x;
        transform.FaceForward(!movingRight);
    }
    #endregion

    IEnumerator GetIntoPlace(){
		while (true){
			rigbod.velocity = (TargetCenterPosition-(Vector2)transform.position).normalized * moveSpeed;
			if (Vector2.Distance(TargetCenterPosition,transform.position)<0.2f){
                StartCoroutine (SwoopOverhead());
				break;
			}
			yield return new WaitForFixedUpdate();
		}
	}

    #region SwoopOverhead
    IEnumerator SwoopOverhead(){
        rigbod.velocity = Vector2.zero;
        startTime = Time.time;
        StartCoroutine(LerpShift());
		while (true){
            transform.position = new Vector2 (FindXPosition(), FindYPosition());
            yield return new WaitForFixedUpdate();
		}
	}

    float shift;
    float startTime;
    const float targetShift = -22.5f;
    IEnumerator LerpShift() {
        float xVel=0;
        while (Mathf.Abs(shift - targetShift)>0.1f) {
            shift = Mathf.SmoothDamp(shift, targetShift, ref xVel, 1.5f);
            yield return new WaitForEndOfFrame();
        }
        shift = targetShift;
    }

    float FindYPosition(){
        return (0.6f) * Mathf.Sin(2*Mathf.PI/(5f) * (Time.time - startTime)) + TargetCenterPosition.y;
	}

	float FindXPosition(){
        return xSpread * Mathf.Sin(2*Mathf.PI/(5f*2f) * (Time.time - startTime) +Mathf.Deg2Rad * shift) + TargetCenterPosition.x;
	}
    #endregion


    IEnumerator Poop(){
		float minPoopTimeDelay = 4f;
        Vector2 pooDistanceRange = new Vector2 (1f,1.5f);
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
	}

    protected override void DieUniquely() {
        FindObjectOfType<SeagullSyncer>().GetComponent<ISeagullSyncer>().ReportSeagullDown(this);
        base.DieUniquely();
    }
}
