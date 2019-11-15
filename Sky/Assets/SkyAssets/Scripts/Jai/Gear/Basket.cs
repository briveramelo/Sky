using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GenericFunctions;
using System.Linq;
using BRM.EventBrokers;
using BRM.EventBrokers.Interfaces;
using UnityEngine.SceneManagement;

public interface ITentacleToBasket
{
    void KnockDown(float downForce);
    void AttachToTentacles(Transform tentaclesTransform);
    void DetachFromTentacles();
}

public interface IBalloonToBasket
{
    void ReportPoppedBalloon(IBasketToBalloon poppedBalloon);
}

public class Basket : Singleton<Basket>, IBalloonToBasket, ITentacleToBasket, IDie
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
    [SerializeField] private BasketEngine _basketEngine;

    private IBrokerEvents _eventBroker = new StaticEventBroker();
    private IPublishEvents _eventPublisher = new StaticEventBroker();
    
    private Transform _startingParentTransform;
    private List<IBasketToBalloon> _balloons;
    private Vector2[] _relativeBalloonPositions;
    private int _continuesRemaining = 1;
    private const int _numStartBalloons = 3;
    private const float _invincibleTime = 1.5f;
    protected override bool _destroyOnLoad => true;

    protected override void Awake()
    {
        base.Awake();
        _eventBroker.Subscribe<ContinueData>(OnContinue);
        _startingParentTransform = transform.parent;
        TentacleToBasket = this;
        EasyAccess.BalloonCenter = _balloonCenter;
        EasyAccess.BasketTransform = _basketCenter;
    }

    private void Start()
    {
        _balloons = new List<IBasketToBalloon>(_balloonScripts);
        _relativeBalloonPositions = new Vector2[_numStartBalloons];
        for (var i = 0; i < _balloons.Count; i++)
        {
            _balloons[i].BalloonNumber = i;
            _relativeBalloonPositions[i] = _balloonScripts[i].transform.position - EasyAccess.JaiTransform.position;
        }
    }

    private void OnDestroy()
    {
        _eventBroker.Unsubscribe<ContinueData>(OnContinue);
    }

    private void OnContinue(ContinueData data)
    {
        ((IDie) this).Rebirth();
    }

    #region IBalloonToBasket

    void IBalloonToBasket.ReportPoppedBalloon(IBasketToBalloon poppedBalloon)
    {
        _balloons.Remove(poppedBalloon);
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
        var invincibleClipLength = AudioManager.PlayAudio(AudioClipType.BalloonsInvincible);
        if (_balloons.Count >= 1)
        {
            AudioManager.PlayAudio(AudioClipType.BalloonsVincible, invincibleClipLength);
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
        if (col.gameObject.layer == Layers.BalloonFloatingLayer)
        {
            if (_balloons.Count < _numStartBalloons)
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

    void ITentacleToBasket.AttachToTentacles(Transform tentaclesTransform)
    {
        _rigbod.velocity = Vector2.zero;
        _rigbod.isKinematic = true;
        _basketCollider.enabled = false;
        ScoreSheet.Tallier.TallyThreat(Threat.BasketGrabbed);
        transform.SetParent(tentaclesTransform);
    }

    void ITentacleToBasket.DetachFromTentacles()
    {
        transform.SetParent(_startingParentTransform);
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

    void IDie.Die()
    {
        _rigbod.velocity = Vector2.zero;
        for (var i = 0; i < _balloons.Count; i++)
        {
            _balloons[i].DetachFromBasket();
        }

        _balloons.Clear();
        StartCoroutine(FallToDeath());
    }

    void IDie.Rebirth()
    {
        _rigbod.gravityScale = 0;
        ((IDie) _basketEngine).Rebirth();
        AudioManager.PlayAudio(AudioClipType.BasketRebirth);
        transform.position = Vector3.zero;
        _rigbod.velocity = Vector2.zero;
        _boundingColliders.ToList().ForEach(col => col.enabled = true);
        for (var i = 0; i < _numStartBalloons; i++)
        {
            var newBalloon = Instantiate(_balloonReplacement, Vector3.zero, Quaternion.identity).GetComponent<IBasketToBalloon>();
            CollectNewBalloon(newBalloon);
        }

        GrantBalloonInvincibility();
    }
}