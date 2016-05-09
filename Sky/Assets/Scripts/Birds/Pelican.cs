using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Pelican : Bird {

	[SerializeField] Animator pelicanAnimator;

    Vector2[] setPositions;

	Vector2 targetPosition { get { return (Vector2)Constants.balloonCenter.position + setPositions[(int)pelPost]; } }

	enum PP{Below =0, Right =1, Left =2, Above=3, End=4}
    PP pelPost;

	protected override void Awake () {
        pelicanAnimator.SetInteger("AnimState", Random.Range(0, 2));
		base.Awake();
        float periodLength = 2f;
        setPositions = new Vector2[]{
		    new Vector2 (0f, -2.2f),
		    new Vector2 (periodLength, -.8f),
		    new Vector2 (-periodLength, -.8f),
		    new Vector2 (0f, 2f)
	    };
		
		StartCoroutine(SwoopAround());
	}

    const float moveSpeed = 2f;
    float timeSinceStartedDiving =0;
	//Move from one checkpoint to another
	IEnumerator SwoopAround(){
        pelicanAnimator.SetInteger("AnimState", (int)PelAnimState.Flapping);
        pelPost = 0;
        timeSinceStartedDiving = 0;
		bool[] spotsHit = new bool[4];
		while ((int)pelPost<setPositions.Length){
            if (pelPost == PP.Right) {
                bool goRight = transform.position.x > Constants.balloonCenter.position.x;
                if (goRight) {
                    pelPost = PP.Left;
                }
            }
            while (!spotsHit[(int)pelPost]) {
                if (pelPost == PP.Above) {
                    CheckToAnimateDive();
                    if (pelicanAnimator.GetInteger("AnimState") == (int)PelAnimState.Diving) {
                        if (Time.time - timeSinceStartedDiving > 1f) {
                            break;
                        }
                    };
                }
                rigbod.velocity = GetVelocity().normalized * moveSpeed;
                transform.FaceForward(rigbod.velocity.x > 0);
				spotsHit[(int)pelPost] = ShouldIMoveToNextPost();
				yield return null;
			}
			pelPost++;
			yield return null;
		}
        bool rightSide = Bool.TossCoin();
		StartCoroutine (DiveBomb (rightSide));
	}

    float distanceToAnimateDive = 1.4f;
    void CheckToAnimateDive() {
        float distanceAway = Vector3.Distance(transform.position, targetPosition);
        if (distanceAway < distanceToAnimateDive && pelicanAnimator.GetInteger("AnimState")==(int)PelAnimState.Flapping) {
            pelicanAnimator.SetInteger("AnimState", (int)PelAnimState.Diving);
            timeSinceStartedDiving = Time.time;
        }
    }

    Vector2 GetVelocity() {
        Vector2 moveDir = targetPosition - (Vector2)transform.position;
        if (pelPost == PP.Right || pelPost == PP.Left) {
            moveDir.x = Mathf.Clamp(moveDir.x * moveDir.x, moveDir.x * .2f, moveDir.x);
            moveDir.y = Mathf.Clamp( 1f/ moveDir.y , moveDir.y * .2f, moveDir.y);
        }
        else if (pelPost == PP.Above) {
            moveDir.x = Mathf.Clamp( 1f/ (moveDir.x) , moveDir.x * .2f, moveDir.x);
            moveDir.y = Mathf.Clamp(moveDir.y * moveDir.y, moveDir.y * .2f, moveDir.y);
        }
        return Vector2.Lerp(rigbod.velocity, moveDir, Time.deltaTime * 10f);
    }
	
    const float distanceThreshold = 0.25f;
	//determine if he should move on to the next checkpoint in his flight
	bool ShouldIMoveToNextPost(){
		if (pelPost == PP.Below || pelPost == PP.Above){
			if (Mathf.Abs (transform.position.x-targetPosition.x)<distanceThreshold){
				return true;
			}
		}
		else if (pelPost == PP.Left || pelPost == PP.Right){
			if (transform.position.y>targetPosition.y-.1f){
				return true;
			}
		}
        return false;
	}

	//plunge to (un)certain balloon-popping glory
	IEnumerator DiveBomb(bool goingRight){
        pelicanAnimator.SetInteger("AnimState", (int)PelAnimState.Down);
        float diveAngle = goingRight ? -80f : 260f;
        rigbod.velocity = ConvertAnglesAndVectors.ConvertAngleToVector2(diveAngle) * 6f;
        transform.FaceForward(rigbod.velocity.x > 0);
		while (transform.position.y>-Constants.WorldDimensions.y-1f){
			yield return null;
		}
		rigbod.velocity = Vector2.zero;
		birdCollider.enabled = false;
		yield return new WaitForSeconds (2f);
		StartCoroutine (SwoopAround ());
		while (transform.position.y<-Constants.WorldDimensions.y){
			yield return null;
		}
		birdCollider.enabled = true;
	}
    enum PelAnimState {
        Flapping =0,
        Diving= 1,
        Down=2
    }
}