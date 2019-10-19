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

	[SerializeField] private GameObject _rope;
	[SerializeField] private PixelPerfectSprite _pixelPerfect;
	[SerializeField] private SpriteRenderer _mySprite;
	[SerializeField] private Sprite[] _balloonSprites;
	[SerializeField] private RuntimeAnimatorController[] _balloonAnimators;
	[SerializeField] private CircleCollider2D _balloonCollider;
	[SerializeField] private CircleCollider2D _boundsCollider;
	[SerializeField] private Animator _balloonAnimator;
    [SerializeField] private List<SpriteRenderer> _mySprites;
    [SerializeField] private AudioClip _pop;

    private int _balloonNumber;
    private const float _moveSpeed = 0.75f;
    private const float _popTime = 30f;

    private void Awake () {
		int randomBalloon = Random.Range(0,_balloonSprites.Length);
		_mySprite.sprite = _balloonSprites[randomBalloon];
		_balloonAnimator.runtimeAnimatorController = _balloonAnimators[randomBalloon];
		if (!transform.parent){
			_boundsCollider.enabled = false;
            StartCoroutine(((IBasketToBalloon)this).BecomeInvincible());
			StartCoroutine (FloatUp());
		}
	}

	#region IBasketToBalloon
	int IBasketToBalloon.BalloonNumber{
		get => _balloonNumber;
		set => _balloonNumber =value;
	}
	void IBasketToBalloon.DetachFromBasket(){
		transform.parent = null;
		gameObject.layer = Constants.BalloonFloatingLayer;
		StartCoroutine (FloatUp());
	}

	void IBasketToBalloon.AttachToBasket(Vector2 newPosition){
		StopAllCoroutines(); //specifically, stop the balloon from floating up
		transform.SetParent(Basket.Instance.transform);
		gameObject.layer = Constants.BalloonLayer;
		_rope.layer = Constants.BalloonBoundsLayer;
		transform.position = (Vector2)Constants.JaiTransform.position + newPosition;
		_pixelPerfect.enabled = false;
		_boundsCollider.enabled = true;
	}

	IEnumerator IBasketToBalloon.BecomeInvincible(){
		_balloonCollider.enabled = false;
        yield return StartCoroutine(FlashColor(1.5f));
		_balloonCollider.enabled = true;
	}

	private IEnumerator FlashColor(float invincibleTime) {
        bool isVisible = false;
        Color invisible = Color.clear;
        Color visible = Color.white;
        float timePassed = 0f;
        float invisibleTime = 0.1f;
        float visibleTime = 0.2f;
        while (timePassed<invincibleTime) {
            _mySprites.ForEach(sprite => sprite.color = isVisible ? visible : invisible);
            float timeToWait = isVisible ? visibleTime : invisibleTime;
            yield return new WaitForSeconds(timeToWait);
            timePassed += timeToWait;
            isVisible = !isVisible;
        }
        _mySprites.ForEach(sprite => sprite.color = visible);
    } 
	#endregion

	private IEnumerator FloatUp(){
		float startTime = Time.time;
		while (true){
			transform.position += Vector3.up * _moveSpeed * Time.deltaTime;
			if (Time.time-startTime>_popTime){
				Destroy (gameObject);
			}
			yield return null;
		}
	}

	private void OnTriggerEnter2D(Collider2D col){
		if (_balloonCollider.isActiveAndEnabled && col.gameObject.layer == Constants.BirdLayer){//bird layer pops free balloon
			Pop();
		}
	}

	public void Pop(){
        if (gameObject.layer == Constants.BalloonLayer) {
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
		_balloonCollider.enabled = false;
		_boundsCollider.enabled = false;
		_balloonAnimator.SetInteger("AnimState",1);
        AudioManager.PlayAudio(_pop);
		Destroy (_rope);
		Destroy (gameObject,Constants.Time2Destroy);
	}

	private void OnDestroy(){
		StopAllCoroutines ();
	}
}
