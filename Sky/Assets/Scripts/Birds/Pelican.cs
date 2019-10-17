using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Pelican : Bird {

	[SerializeField] Animator pelicanAnimator;

    int currentTarIn;
    Vector3[] setPositions;

	Vector3 targetPosition { get { return Constants.balloonCenter.position + Vector3.right * setPositions[currentTarIn].x * sideMultiplier + Vector3.up * setPositions[currentTarIn].y ; } }
	protected override void Awake () {
        pelicanAnimator.SetInteger("AnimState", Random.Range(0, 2));
		base.Awake();
        float yAbove = 2;
        float yBelow = -2.2f;
        float resolution = 0.1f;
        int totalPoints = (int)((yAbove - yBelow) / resolution);
        setPositions = new Vector3[totalPoints];
        for (int i = 0; i < totalPoints; i++) {
            float iFloat = i;
            float xPoint = -1 *    Mathf.Cos(2f * Mathf.PI * ((iFloat/(totalPoints)))) + 1f;
            float yPoint = -2.1f * Mathf.Cos(2f * Mathf.PI * ((iFloat/(totalPoints*2))));
            Vector3 thisVector = new Vector3(xPoint, yPoint, 0f);
            setPositions[i] = thisVector;
        }

		StartCoroutine(SwoopAround());
	}

    bool isDiving;
    int sideMultiplier;
    float moveSpeed = 2f;
    float heightTrigger = 1.6f;
	//Move from one checkpoint to another
	IEnumerator SwoopAround(){
        pelicanAnimator.SetInteger("AnimState", (int)PelAnimState.Flapping);
        currentTarIn = 0;
        sideMultiplier = transform.position.x < 0 ? 1 : -1;

		while (currentTarIn<setPositions.Length){
            
            rigbod.velocity = GetVelocity();
            float xFromJai = Constants.jaiTransform.position.x - transform.position.x;
            transform.FaceForward(xFromJai > 0);

            if (Vector3.Distance(transform.position, targetPosition)<0.2f) {
			    currentTarIn++;
                if (pelicanAnimator.GetInteger("AnimState") == (int)PelAnimState.Flapping && setPositions[currentTarIn].y>1.2f) {
                    StartCoroutine(TriggerDiveAnimation());
                }
                if (currentTarIn>setPositions.Length) {
                    break;
                }
            }
			yield return null;
		}

		StartCoroutine (DiveBomb (sideMultiplier<0));
	}

    IEnumerator TriggerDiveAnimation() {
        float timeSinceStartedDiving = 0;
        pelicanAnimator.SetInteger("AnimState", (int)PelAnimState.Diving);
        timeSinceStartedDiving = Time.time;
        while (true) {
            if (Time.time - timeSinceStartedDiving > 1f) {
                currentTarIn = setPositions.Length+1;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    Vector2 GetVelocity() {
        return (targetPosition - transform.position).normalized * moveSpeed;
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