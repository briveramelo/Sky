using UnityEngine;
using System.Collections;
using GenericFunctions;
using System.Collections.Generic;

public interface IBasketToBalloon
{
    void DetachFromBasket();
    void AttachToBasket(Vector2 newPosition);
    IEnumerator BecomeInvincible();
    int BalloonNumber { get; set; }
}

public class Balloon : MonoBehaviour, IBasketToBalloon
{
    private static class AnimState
    {
        public const int Wiggle = 1;
    }

    [SerializeField] private GameObject _rope;
    [SerializeField] private PixelPerfectSprite _pixelPerfect;
    [SerializeField] private SpriteRenderer _mySprite;
    [SerializeField] private Sprite[] _balloonSprites;
    [SerializeField] private RuntimeAnimatorController[] _balloonAnimators;
    [SerializeField] private CircleCollider2D _balloonCollider;
    [SerializeField] private CircleCollider2D _boundsCollider;
    [SerializeField] private Animator _balloonAnimator;
    [SerializeField] private List<SpriteRenderer> _mySprites;
    
    private const float _moveSpeed = 0.55f;
    private const float _popTime = 30f;
    
    private Coroutine _floatUpRoutine;
    private int _balloonNumber;

    private void Awake()
    {
        var randomBalloon = Random.Range(0, _balloonSprites.Length);
        _mySprite.sprite = _balloonSprites[randomBalloon];
        _balloonAnimator.runtimeAnimatorController = _balloonAnimators[randomBalloon];
        if (!transform.parent)
        {
            _boundsCollider.enabled = false;
            StartCoroutine(((IBasketToBalloon) this).BecomeInvincible());
            _floatUpRoutine = StartCoroutine(FloatUp());
        }
    }

    #region IBasketToBalloon

    int IBasketToBalloon.BalloonNumber
    {
        get => _balloonNumber;
        set => _balloonNumber = value;
    }

    void IBasketToBalloon.DetachFromBasket()
    {
        transform.SetParent(null);
        gameObject.layer = Layers.BalloonFloatingLayer;
        StartCoroutine(FloatUp());
    }

    void IBasketToBalloon.AttachToBasket(Vector2 newPosition)
    {
        StopAllCoroutines(); //specifically, stop the balloon from floating up
        transform.SetParent(Basket.Instance.transform);
        gameObject.layer = Layers.BalloonLayer;
        _rope.layer = Layers.BalloonBoundsLayer;
        transform.position = (Vector2) Constants.JaiTransform.position + newPosition;
        _pixelPerfect.enabled = false;
        _boundsCollider.enabled = true;
    }

    IEnumerator IBasketToBalloon.BecomeInvincible()
    {
        _balloonCollider.enabled = false;
        yield return StartCoroutine(FlashColor(1.5f));
        _balloonCollider.enabled = true;
    }

    private IEnumerator FlashColor(float invincibleTime)
    {
        var isVisible = false;
        var invisible = Color.clear;
        var visible = Color.white;
        var timePassed = 0f;
        var invisibleTime = 0.1f;
        var visibleTime = 0.2f;
        while (timePassed < invincibleTime)
        {
            _mySprites.ForEach(sprite => sprite.color = isVisible ? visible : invisible);
            var timeToWait = isVisible ? visibleTime : invisibleTime;
            yield return new WaitForSeconds(timeToWait);
            timePassed += timeToWait;
            isVisible = !isVisible;
        }

        _mySprites.ForEach(sprite => sprite.color = visible);
    }

    #endregion

    private IEnumerator FloatUp()
    {
        var startTime = Time.time;
        while (true)
        {
            transform.position += Time.deltaTime * _moveSpeed * Vector3.up;
            if (Time.time - startTime > _popTime)
            {
                Destroy(gameObject);
            }

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_balloonCollider.isActiveAndEnabled && col.gameObject.layer == Layers.BirdLayer)
        {
            //bird layer pops free balloon
            Pop();
        }
    }

    public void Pop()
    {
        if (gameObject.layer == Layers.BalloonLayer)
        {
            ((IBalloonToBasket) Basket.Instance).ReportPoppedBalloon(this);
            ScoreSheet.Tallier.TallyThreat(Threat.BalloonPopped);
            
            Handheld.Vibrate();
            GameClock.Instance.SlowTime(.5f, .75f);
            GameCamera.Instance.ShakeTheCamera();
            if (_floatUpRoutine!=null)
            {
                StopCoroutine(_floatUpRoutine);
            }
            transform.SetParent(null);
        }
        else
        {
            ScoreSheet.Tallier.TallyBalloonPoints(transform.position);
        }

        _balloonCollider.enabled = false;
        _boundsCollider.enabled = false;
        _balloonAnimator.SetInteger(Constants.AnimState, AnimState.Wiggle);
        AudioManager.PlayAudio(AudioClipType.BalloonPop);
        Destroy(_rope);
        Destroy(gameObject, 2f);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}