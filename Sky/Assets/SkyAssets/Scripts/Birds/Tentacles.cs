using UnityEngine;
using System.Collections;
using GenericFunctions;

public interface IStabbable
{
    void GetStabbed();
}

public interface ISensorToTentacle
{
    IEnumerator GoForTheKill();
    IEnumerator ResetPosition(bool defeated);
}

public interface ITipToTentacle
{
    IEnumerator PullDownTheKill();
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

    [SerializeField] private TentaclesSensor _ts;
    [SerializeField] private Transform _tipTransform;
    [SerializeField] private Collider2D _tipCollider;

    public override BirdType MyBirdType => BirdType.Tentacles;
    
    private ISensorToTentacle _me; //just because I wanted to use ResetPosition locally with less mess...
    private IToggleable _sensor;
    private IJaiDetected _sensorOnJai;
    private IFreezable _inputManager;
    private IFreezable _jai;

    private WeaponStats _fakeWeapon = new WeaponStats();
    private Vector2 _homeSpot;

    private float _descendSpeed = 1f;
    private float _attackSpeed = 1.5f;
    private float _resetSpeed = 1f;
    private float _defeatedHeight;
    private float _resetHeight;

    private int _stabsTaken;
    private const int _stabs2Retreat = 4;

    private bool _holdingJai;

    protected override void Awake()
    {
        base.Awake();

        Instance = this;
        StabbableTentacle = this;
        Releaser = this;
        _me = this;
        _sensor = _ts;
        _sensorOnJai = _ts;

        _homeSpot = new Vector2(0f, -.75f - Constants.ScreenSizeWorldUnits.y);
        _resetHeight = .5f + _homeSpot.y;
        _defeatedHeight = .25f + _homeSpot.y;
    }

    private void Start()
    {
        _inputManager = FindObjectOfType<TouchInputManager>().GetComponent<IFreezable>();
        _jai = FindObjectOfType<Jai>().GetComponent<IFreezable>();
    }

    private void FaceTowardYou(bool toward)
    {
        var pos = transform.position;
        var basketPos = Constants.BasketTransform.position;
        transform.FaceForward(toward ? basketPos.x - pos.x > 0 : basketPos.x - pos.x < 0);
    }

    #region ISensorToTentacle

    IEnumerator ISensorToTentacle.GoForTheKill()
    {
        while (_sensorOnJai.JaiInRange && !_holdingJai)
        {
            _rigbod.velocity = Constants.SpeedMultiplier * _attackSpeed * (Constants.BasketTransform.position - Vector3.one * 0.2f - _tipTransform.position).normalized;
            FaceTowardYou(true);
            yield return null;
        }
    }

    IEnumerator ISensorToTentacle.ResetPosition(bool defeated)
    {
        var finishHeight = defeated ? _resetHeight : _defeatedHeight;

        while (_tipTransform.position.y > finishHeight && (defeated ? true : !_sensorOnJai.JaiInRange))
        {
            _rigbod.velocity = Constants.SpeedMultiplier * _resetSpeed * ((Vector3) _homeSpot - _tipTransform.position).normalized;
            FaceTowardYou(!defeated);
            yield return null;
        }

        _rigbod.velocity = Vector2.zero;
        _sensor.ToggleSensor(true);
    }

    #endregion

    #region ITipToTentacle

    IEnumerator ITipToTentacle.PullDownTheKill()
    {
        _stabsTaken = 0;
        _holdingJai = true;
        _inputManager.IsFrozen = true;
        _jai.IsFrozen = true;
        Basket.TentacleToBasket.AttachToTentacles(transform);
        ScoreSheet.Tallier.TallyThreat(Threat.BasketGrabbed);

        GameClock.Instance.SlowTime(0.4f, 0.5f);
        GameCamera.Instance.ShakeTheCamera();
        Constants.WorldCollider.enabled = false;

        while (_tipTransform.position.y > _defeatedHeight && _stabsTaken < _stabs2Retreat)
        {
            _rigbod.velocity = Constants.SpeedMultiplier * _descendSpeed * Vector2.down;
            yield return null;
        }

        if (_stabsTaken < _stabs2Retreat)
        {
            Basket.TentacleToBasket.LoseAllBalloons();
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
        _inputManager.IsFrozen = false;
        _jai.IsFrozen = false;
        Basket.TentacleToBasket.DetachFromTentacles();
        Basket.TentacleToBasket.KnockDown(5f);

        _sensor.ToggleSensor(false);
        StartCoroutine(DisableTentacles());
        StartCoroutine(_me.ResetPosition(true));
    }

    private IEnumerator DisableTentacles()
    {
        _tipCollider.enabled = false;
        yield return new WaitForSeconds(1.5f);
        _tipCollider.enabled = true;
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

    protected override void DieUniquely()
    {
        if (Constants.WorldCollider != null)
        {
            Constants.WorldCollider.enabled = true;
        }

        if (_holdingJai)
        {
            ReleaseBasket();
        }

        Destroy(transform.parent.gameObject);
    }
}