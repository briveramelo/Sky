using System;
using UnityEngine;
using System.Collections;
using GenericFunctions;
using Random = UnityEngine.Random;

public interface IStabbable
{
    void GetStabbed();
}

public interface ISensorToTentacle
{
    void GoForTheKill();
    void ResetPosition(bool defeated);
}

public interface ITipToTentacle
{
    void PullDownTheKill();
}

public interface IReleasable
{
    void ReleaseJai();
}

public class Tentacles : Bird, ISensorToTentacle, IStabbable, ITipToTentacle, IReleasable
{
    public static Tentacles Instance;
    public static IStabbable StabbableTentacle;
    public static IReleasable Releaser;

    [SerializeField] private Transform _parentTran;
    [SerializeField] private TentaclesSensor _tentaclesSensor;
    [SerializeField] private TentaclesTip _tentaclesTip;
    [SerializeField] private Transform _tipTransform;

    protected override BirdType MyBirdType => BirdType.Tentacles;
    
    private const int _stabs2Retreat = 7;
    private const float _descendSpeed = 1.2f;
    private const float _attackSpeed = 1.7f;
    private const float _resetSpeed = 1.45f;
    
    private IToggleable _jaiSensorToggler;
    private IToggleable _tipToggler;
    private IJaiDetector _jaiDetector;
    private IFreezable _basketEngineFreezable;
    private IFreezable _jaiFreezable;
    private IDie _jaiDeath;
    private WeaponStats _fakeWeapon = new WeaponStats();

    private Vector2 _targetSpot;
    private Vector2 _moveDir;
    private Vector2 _homeSpot;
    private float _defeatedHeight;
    private float _resetHeight;
    private int _stabsTaken;
    private bool _holdingJai;
    private State _currentState;
    private State CurrentState
    {
        get => _currentState;
        set
        {
            //Debug.LogError("Setting State to " + value);
            _currentState = value;
        }
    }

    private enum State
    {
        Idling=0,
        MovingHome=1,
        ApproachingBasket=2,
        AttemptingSubmerge=3
    }

    protected override void Awake()
    {
        base.Awake();

        Instance = this;
        StabbableTentacle = this;
        Releaser = this;
        _tipToggler = _tentaclesTip;
        _jaiSensorToggler = _tentaclesSensor;
        _jaiDetector = _tentaclesSensor;

        _homeSpot = new Vector2(0f, -ScreenSpace.WorldEdge.y - .25f);
        _resetHeight = .5f/4f + _homeSpot.y;
        _defeatedHeight = .25f/4f + _homeSpot.y;
        CurrentState = State.Idling;
    }

    private void Start()
    {
        _basketEngineFreezable = FindObjectOfType<BasketEngine>();
        var jai = FindObjectOfType<Jai>();
        _jaiFreezable = jai;
        _jaiDeath = jai;
    }

    private void FaceTowardBasket(bool toward)
    {
        var pos = transform.position;
        var basketPos = Constants.BasketTransform.position;
        transform.FaceForward(toward ? basketPos.x - pos.x > 0 : basketPos.x - pos.x < 0);
    }

    #region ISensorToTentacle

    void ISensorToTentacle.GoForTheKill()
    {
        StartCoroutine(MoveTowardJai());
    }

    private IEnumerator MoveTowardJai()
    {
        CurrentState = State.ApproachingBasket;
        while (_jaiDetector.JaiInRange && !_holdingJai)
        {
            _targetSpot = Constants.BasketTransform.position - Vector3.one * 0.05f;
            _moveDir = _targetSpot - (Vector2) _tipTransform.position;
            _rigbod.velocity = Constants.SpeedMultiplier * _attackSpeed * _moveDir.normalized;
            FaceTowardBasket(true);
            yield return null;
        }
    }

    void ISensorToTentacle.ResetPosition(bool defeated)
    {
        StartCoroutine(MoveBackHome(defeated));
    }

    private IEnumerator MoveBackHome(bool defeated)
    {
        CurrentState = State.MovingHome;
        var finishHeight = defeated ? _resetHeight : _defeatedHeight;

        while (_tipTransform.position.y > finishHeight && (defeated ? true : !_jaiDetector.JaiInRange))
        {
            _rigbod.velocity = Constants.SpeedMultiplier * _resetSpeed * ((Vector3) _homeSpot - _tipTransform.position).normalized;
            FaceTowardBasket(!defeated);
            yield return null;
        }

        _rigbod.velocity = Vector2.zero;
        _jaiSensorToggler.ToggleSensor(true);
    }

    #endregion

    #region ITipToTentacle

    void ITipToTentacle.PullDownTheKill()
    {
        GrabBasket();
        StartCoroutine(MoveDown(OnSubmergeBasket));
    }

    private void GrabBasket()
    {
        _stabsTaken = 0;
        _holdingJai = true;
        _tipToggler.ToggleSensor(false);
        
        _basketEngineFreezable.IsFrozen = true;
        _jaiFreezable.IsFrozen = true;
        Basket.TentacleToBasket.AttachToTentacles(transform);
        ScoreSheet.Tallier.TallyThreat(Threat.BasketGrabbed);

        GameClock.Instance.SlowTime(0.4f, 0.5f);
        GameCamera.Instance.ShakeTheCamera();
        Constants.WorldCollider.enabled = false;
    }

    private IEnumerator MoveDown(Action onSubmerge)
    {
        CurrentState = State.AttemptingSubmerge;
        while (_tipTransform.position.y > _defeatedHeight && _stabsTaken < _stabs2Retreat)
        {
            _rigbod.velocity = Constants.SpeedMultiplier * _descendSpeed * Vector2.down;
            yield return null;
        }
        onSubmerge?.Invoke();
    }

    private void OnSubmergeBasket()
    {
        if (_stabsTaken < _stabs2Retreat)
        {
            (Basket.Instance as IDie).Die();
            _jaiDeath.Die();
        }

        Constants.WorldCollider.enabled = true;
    }

    #endregion

    #region IStabbable

    void IStabbable.GetStabbed()
    {
        _stabsTaken++;
        TakeDamage(ref _fakeWeapon);
        if (_stabsTaken >= _stabs2Retreat)
        {
            ReleaseBasket();
        }
    }

    #endregion

    void IReleasable.ReleaseJai()
    {
        if (_holdingJai)
        {
            ReleaseBasket();
        }
    }

    private void ReleaseBasket()
    {
        _holdingJai = false;
        _basketEngineFreezable.IsFrozen = false;
        _jaiFreezable.IsFrozen = false;
        Basket.TentacleToBasket.DetachFromTentacles();
        Basket.TentacleToBasket.KnockDown(5f);

        _jaiSensorToggler.ToggleSensor(false);
        StartCoroutine(Delay.DelayAction(() => _tipToggler.ToggleSensor(true), 1.5f));
        StartCoroutine(MoveBackHome(true));
    }
    #region TakeDamage

    protected override int TakeDamage(ref WeaponStats weaponStats)
    {
        var holding = weaponStats.Velocity == Vector2.zero;
        Vector2 spawnSpot;
        Vector2 gutVel;
        int damageDealt;
        if (holding)
        {
            gutVel = new Vector2(-Mathf.Sign(transform.localScale.x) * Random.value, Random.value).normalized;
            spawnSpot = _tipTransform.position;
            damageDealt = 0;
        }
        else
        {
            gutVel = weaponStats.Velocity;
            spawnSpot = _birdCollider.bounds.ClosestPoint(weaponStats.WeaponCollider.transform.position);
            damageDealt = weaponStats.Damage;
        }

        BirdStats.Health -= damageDealt;
        Instantiate(_guts, spawnSpot, Quaternion.identity).GetComponent<IBleedable>().GenerateGuts(ref BirdStats, gutVel);
        return damageDealt;
    }

    #endregion

    protected override void OnDeath()
    {
        if (Constants.WorldCollider != null)
        {
            Constants.WorldCollider.enabled = true;
        }

        if (_holdingJai)
        {
            ReleaseBasket();
        }

        Destroy(_parentTran.gameObject);
    }
}