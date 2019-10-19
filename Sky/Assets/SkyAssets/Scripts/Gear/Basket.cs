﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GenericFunctions;
using System.Linq;
using UnityEngine.SceneManagement;

public interface ITentacleToBasket
{
    void KnockDown(float downForce);
    void LoseAllBalloons();
    void AttachToTentacles(Transform tentaclesTransform);
    void DetachFromTentacles();
}

public interface IBalloonToBasket
{
    void ReportPoppedBalloon(IBasketToBalloon poppedBalloon);
}

public class Basket : Singleton<Basket>, IBalloonToBasket, ITentacleToBasket
{
    public static ITentacleToBasket TentacleToBasket;

    [SerializeField] private Rigidbody2D _rigbod;
    [SerializeField] private Transform _balloonCenter;
    [SerializeField] private Transform _basketCenter;
    [SerializeField] private Balloon[] _balloonScripts;
    [SerializeField] private BoxCollider2D _basketCollider;
    [SerializeField] private Collider2D[] _boundingColliders;
    [SerializeField] private GameObject _balloonReplacement;
    [SerializeField] private List<SpriteRenderer> _mySprites;
    [SerializeField] private AudioClip _invincible, _ready, _rebirth;
    [SerializeField] private BasketEngine _basketEngine;

    private List<IBasketToBalloon> _balloons;
    private Vector2[] _relativeBalloonPositions;
    private int _continuesRemaining = 1;
    private const float _invincibleTime = 1.5f;
    protected override bool _destroyOnLoad => true;

    protected override void Awake()
    {
        base.Awake();
        TentacleToBasket = this;
        Constants.BalloonCenter = _balloonCenter;
        Constants.BasketTransform = _basketCenter;
        _balloons = new List<IBasketToBalloon>(_balloonScripts);
        _relativeBalloonPositions = new Vector2[3];
        for (var i = 0; i < _balloons.Count; i++)
        {
            _balloons[i].BalloonNumber = i;
            _relativeBalloonPositions[i] = _balloonScripts[i].transform.position - Constants.JaiTransform.position;
        }
    }

    #region IBalloonToBasket

    void IBalloonToBasket.ReportPoppedBalloon(IBasketToBalloon poppedBalloon)
    {
        _balloons.Remove(poppedBalloon);
        ScoreSheet.Tallier.TallyThreat(Threat.BalloonPopped);
        GrantBalloonInvincibility();
        PlayRecoverySounds();
        if (_balloons.Count < 1)
        {
            StartCoroutine(FallToDeath());
        }
    }

    #endregion

    private void GrantBalloonInvincibility()
    {
        for (var i = 0; i < _balloons.Count; i++)
        {
            StartCoroutine(_balloons[i].BecomeInvincible());
        }

        StartCoroutine(FlashColor(_invincibleTime));
    }

    private void PlayRecoverySounds()
    {
        AudioManager.PlayAudio(_invincible);
        if (_balloons.Count >= 1)
        {
            AudioManager.PlayReadyDelayed(_invincible.length);
        }
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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == Constants.BalloonFloatingLayer)
        {
            if (_balloons.Count < 3)
            {
                CollectNewBalloon(col.gameObject.GetComponent<IBasketToBalloon>());
            }
        }
    }

    private void CollectNewBalloon(IBasketToBalloon newBalloon)
    {
        var balloonNumbers = new List<int>(new[] {0, 1, 2});
        _balloons.ForEach(balloon => balloonNumbers.Remove(balloon.BalloonNumber));
        var newBalloonNumber = balloonNumbers[0];

        newBalloon.AttachToBasket(_relativeBalloonPositions[newBalloonNumber]);
        newBalloon.BalloonNumber = newBalloonNumber;
        _balloons.Add(newBalloon);
        ScoreSheet.Tallier.TallyThreat(Threat.BalloonGained);
    }

    #region ITentacleToBasket

    void ITentacleToBasket.KnockDown(float downForce)
    {
        _rigbod.AddForce(Vector2.down * downForce);
    }

    void ITentacleToBasket.LoseAllBalloons()
    {
        _rigbod.velocity = Vector2.zero;
        for (var i = 0; i < _balloons.Count; i++)
        {
            _balloons[i].DetachFromBasket();
        }

        _balloons.Clear();
        StartCoroutine(FallToDeath());
    }

    void ITentacleToBasket.AttachToTentacles(Transform tentaclesTransform)
    {
        _rigbod.velocity = Vector2.zero;
        _rigbod.isKinematic = true;
        _basketCollider.enabled = false;
        ScoreSheet.Tallier.TallyThreat(Threat.BasketGrabbed);
        transform.parent = tentaclesTransform;
    }

    void ITentacleToBasket.DetachFromTentacles()
    {
        transform.parent = null;
        transform.localScale = Vector3.one;
        _basketCollider.enabled = true;
        _rigbod.isKinematic = false;
        ScoreSheet.Tallier.TallyThreat(Threat.BasketReleased);
    }

    #endregion

    private IEnumerator FinishPlay()
    {
        ScoreSheet.Reporter.ReportScores();
        yield return StartCoroutine(ScoreSheet.Reporter.DisplayTotal());
        SceneManager.LoadScene(Scenes.Menu);
    }

    private IEnumerator FallToDeath()
    {
        _rigbod.gravityScale = 1;
        ((IDie) _basketEngine).Die();
        _boundingColliders.ToList().ForEach(col => col.enabled = false);
        yield return new WaitForSeconds(_invincibleTime);
        if (_continuesRemaining > 0)
        {
            _continuesRemaining--;
            FindObjectOfType<Continuer>().DisplayContinueMenu(true);
        }
        else
        {
            StartCoroutine(FinishPlay());
        }

        yield return null;
    }

    public void ComeBackToLife()
    {
        _rigbod.gravityScale = 0;
        ((IDie) _basketEngine).Rebirth();
        PlayRebirthSounds();
        transform.position = Vector3.zero;
        _rigbod.velocity = Vector2.zero;
        _boundingColliders.ToList().ForEach(col => col.enabled = true);
        IBasketToBalloon newBalloon;
        for (var i = 0; i < 3; i++)
        {
            newBalloon = Instantiate(_balloonReplacement, Vector3.zero, Quaternion.identity).GetComponent<IBasketToBalloon>();
            CollectNewBalloon(newBalloon);
        }

        GrantBalloonInvincibility();
    }

    private void PlayRebirthSounds()
    {
        AudioManager.PlayAudio(_rebirth);
    }
}