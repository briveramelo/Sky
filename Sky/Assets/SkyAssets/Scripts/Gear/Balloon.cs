using UnityEngine;
using System.Collections;
using GenericFunctions;
using System.Collections.Generic;

public interface IBasketToBalloon {
	void DetachFromBasket();
	void AttachToBasket(Vector2 newPosition);
	IEnumerator BecomeInvincible();
	int BalloonNumber{get;set;}
}
public class Balloon : MonoBehaviour, IBasketToBalloon{

	[SerializeField] GameObject rope;
	[SerializeField] PixelPerfectSprite pixelPerfect;
	[SerializeField] SpriteRenderer mySprite;
	[SerializeField] Sprite[] balloonSprites;
	[SerializeField] RuntimeAnimatorController[] balloonAnimators;
	[SerializeField] CircleCollider2D balloonCollider;
	[SerializeField] CircleCollider2D boundsCollider;
	[SerializeField] Animator balloonAnimator;
    [SerializeField] List<SpriteRenderer> mySprites;
    [SerializeField] AudioClip pop;

	int balloonNumber;
	const float moveSpeed = 0.75f;
	const float popTime = 30f;

	void Awake () {
		int randomBalloon = Random.Range(0,balloonSprites.Length);
		mySprite.sprite = balloonSprites[randomBalloon];
		balloonAnimator.runtimeAnimatorController = balloonAnimators[randomBalloon];
		if (!transform.parent){
			boundsCollider.enabled = false;
            StartCoroutine(((IBasketToBalloon)this).BecomeInvincible());
			StartCoroutine (FloatUp());
		}
	}

	#region IBasketToBalloon
	int IBasketToBalloon.BalloonNumber{get => balloonNumber;
		set => balloonNumber =value;
	}
	void IBasketToBalloon.DetachFromBasket(){
		transform.parent = null;
		gameObject.layer = Constants.balloonFloatingLayer;
		StartCoroutine (FloatUp());
	}

	void IBasketToBalloon.AttachToBasket(Vector2 newPosition){
		StopAllCoroutines(); //specifically, stop the balloon from floating up
		transform.SetParent(Basket.Instance.transform);
		gameObject.layer = Constants.balloonLayer;
		rope.layer = Constants.balloonBoundsLayer;
		transform.position = (Vector2)Constants.jaiTransform.position + newPosition;
		pixelPerfect.enabled = false;
		boundsCollider.enabled = true;
	}

	IEnumerator IBasketToBalloon.BecomeInvincible(){
		balloonCollider.enabled = false;
        yield return StartCoroutine(FlashColor(1.5f));
		balloonCollider.enabled = true;
	}

    IEnumerator FlashColor(float invincibleTime) {
        bool isVisible = false;
        Color invisible = Color.clear;
        Color visible = Color.white;
        float timePassed = 0f;
        float invisibleTime = 0.1f;
        float visibleTime = 0.2f;
        while (timePassed<invincibleTime) {
            mySprites.ForEach(sprite => sprite.color = isVisible ? visible : invisible);
            float timeToWait = isVisible ? visibleTime : invisibleTime;
            yield return new WaitForSeconds(timeToWait);
            timePassed += timeToWait;
            isVisible = !isVisible;
        }
        mySprites.ForEach(sprite => sprite.color = visible);
    } 
	#endregion

	IEnumerator FloatUp(){
		float startTime = Time.time;
		while (true){
			transform.position += Vector3.up * moveSpeed * Time.deltaTime;
			if (Time.time-startTime>popTime){
				Destroy (gameObject);
			}
			yield return null;
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (balloonCollider.isActiveAndEnabled && col.gameObject.layer == Constants.birdLayer){//bird layer pops free balloon
			Pop();
		}
	}

	public void Pop(){
        if (gameObject.layer == Constants.balloonLayer) {
            ((IBalloonToBasket)(Basket.Instance)).ReportPoppedBalloon(this);
		    Handheld.Vibrate ();
		    GameClock.Instance.SlowTime(.5f,.75f);
		    GameCamera.Instance.ShakeTheCamera();
		    StopAllCoroutines(); //specifically, stop the balloon from floating up
		    transform.parent = null;
        }
        else {
            ScoreSheet.Tallier.TallyBalloonPoints(transform.position);
        }
		balloonCollider.enabled = false;
		boundsCollider.enabled = false;
		balloonAnimator.SetInteger("AnimState",1);
        AudioManager.PlayAudio(pop);
		Destroy (rope);
		Destroy (gameObject,Constants.time2Destroy);
	}

	void OnDestroy(){
		StopAllCoroutines ();
	}
}
