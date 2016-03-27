using UnityEngine;
using System.Collections;
using GenericFunctions;
using System;

public interface IBasketToBalloon {
	void DetachFromBasket();
	void AttachToBasket(Vector2 newPosition);
	IEnumerator BecomeInvincible();
	int BalloonNumber{get;set;}
}
public class Balloon : MonoBehaviour, IBasketToBalloon{

	[SerializeField] private GameObject rope;
	[SerializeField] PixelPerfectSprite pixelPerfect;
	[SerializeField] private SpriteRenderer mySprite;
	[SerializeField] private Sprite[] balloonSprites;
	[SerializeField] private RuntimeAnimatorController[] balloonAnimators;
	[SerializeField] private CircleCollider2D balloonCollider;
	[SerializeField] private Animator balloonAnimator;
	[SerializeField] private AudioSource popNoise;

	private int balloonNumber;
	private float moveSpeed = 0.75f;
	private float popTime = 30f;

	void Awake () {
		int randomBalloon = UnityEngine.Random.Range(0,balloonSprites.Length);
		mySprite.sprite = balloonSprites[randomBalloon];
		balloonAnimator.runtimeAnimatorController = balloonAnimators[randomBalloon];
		if (!transform.parent){
			StartCoroutine (FloatUp());
		}
	}

	#region IBasketToBalloon
	int IBasketToBalloon.BalloonNumber{get {return balloonNumber;} set{balloonNumber =value;}}
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
	}

	IEnumerator IBasketToBalloon.BecomeInvincible(){
		balloonCollider.enabled = false;
		yield return new WaitForSeconds(1.5f);
		balloonCollider.enabled = true;
	}
	#endregion

	IEnumerator FloatUp(){
		float startTime = Time.realtimeSinceStartup;
		while (true){
			transform.position += Vector3.up * moveSpeed * Time.deltaTime;
			if (Time.realtimeSinceStartup-startTime>popTime){
				Destroy (gameObject);
			}
			yield return null;
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (balloonCollider.isActiveAndEnabled && col.gameObject.layer == Constants.birdLayer || col.gameObject.layer == Constants.spearLayer){//bird layer pops free balloon
			Pop();
		}
	}

	void Pop(){
		Handheld.Vibrate ();
		GameClock.Instance.SlowTime(.5f,.75f);
		GameCamera.Instance.ShakeTheCamera();
		StopAllCoroutines(); //specifically, stop the balloon from floating up

		if (gameObject.layer == Constants.balloonLayer) ((IBalloonToBasket)(Basket.Instance)).ReportPoppedBalloon(this);
		transform.parent = null;
		balloonCollider.enabled = false;
		balloonAnimator.SetInteger("AnimState",1);
		popNoise.Play ();
		Destroy (rope);
		Destroy (gameObject,Constants.time2Destroy);
	}

	void OnDestroy(){
		StopAllCoroutines ();
	}
}
