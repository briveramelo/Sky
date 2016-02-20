using UnityEngine;
using System.Collections;
using GenericFunctions;
using System;

public class Balloon : MonoBehaviour, IBasketToBalloon
 {

	[SerializeField] private GameObject rope;
	[SerializeField] private CircleCollider2D balloonCollider;
	[SerializeField] private Animator balloonAnimator;
	[SerializeField] private AudioSource popNoise;

	private int balloonNumber;
	private float moveSpeed = 0.75f;
	private float popTime = 30f;

	void Awake () {
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
		transform.position = (Vector2)Constants.jaiTransform.position + newPosition;
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
		if (col.gameObject.layer == Constants.birdLayer){//bird layer pops free balloon
			Pop();
		}
	}

	void Pop(){
		Handheld.Vibrate ();
		GameClock.Instance.SlowTime(.5f,.75f);
		GameCamera.Instance.ShakeTheCamera();

		Basket.BalloonToBasket.ReportPoppedBalloon(this);
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
