using UnityEngine;
using System.Collections;
using GenericFunctions;

public interface ISeagull {
    void UpdateSeagullNumber(int seagullNumber, int totalSeagulls);
}

public class Seagull : Bird, ISeagull {

    float periodLength = 3f;
    float sinPeriodShift;
    int seagullNumber;
    Vector2 targetPosition {
        get {
            //logic about gullNum?
            return new Vector2(0f,2.25f);
        }
    }

    void ISeagull.UpdateSeagullNumber(int seagullNumber, int totalSeagulls) {
        this.seagullNumber = seagullNumber;
        sinPeriodShift = 1;
        

        //do something with that other totalSeagulls number
    }

	[SerializeField] GameObject pooNugget;
    bool movingRight;
    int movingSign{get{return movingRight ? 1 :-1;}}

	const float moveSpeed = 3f;
	float lastTimePooped;

	protected override void Awake () {
		base.Awake();

        seagullNumber = FindObjectOfType<SeagullSyncer>().GetComponent<ISeagullSyncer>().RecordNewSeagull(this);
        movingRight = transform.position.x < targetPosition.x;
        transform.FaceForward(!movingRight);
		StartCoroutine (GetIntoPlace ());
		StartCoroutine (Poop ());
	}

	IEnumerator GetIntoPlace(){
		while (true){
			rigbod.velocity = (targetPosition-(Vector2)transform.position).normalized * moveSpeed;
			if (Vector2.Distance(targetPosition,transform.position)<0.2f){
				StartCoroutine (SwoopOverhead());
				break;
			}
			yield return null;
		}
	}

	IEnumerator SwoopOverhead(){
		bool clockwise = Bool.TossCoin();
		while (true){
			rigbod.velocity = new Vector2(FindXVelocity(), FindYVelocity());
			yield return null;
		}
	}

    float FindYVelocity(){
		return 1.5f * Mathf.Sin(2*Mathf.PI * (1/(periodLength)) * (Time.timeSinceLevelLoad + sinPeriodShift));
	}

    float xSpread = 4;
	float FindXVelocity(){
        float distFromEdge = Mathf.Abs(transform.position.x - (targetPosition.x + movingSign * xSpread));
        if (distFromEdge<0.1f){
			movingRight = !movingRight;
            transform.FaceForward(!movingRight);
		}
        float distSurrogate = Mathf.Clamp(distFromEdge, 0.4f, 1);
        float targetSpeed = distSurrogate * movingSign * moveSpeed;
		return Mathf.Lerp(rigbod.velocity.x, targetSpeed,Time.deltaTime);
	}

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
