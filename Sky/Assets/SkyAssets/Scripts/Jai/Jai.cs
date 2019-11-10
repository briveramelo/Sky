using System;
using UnityEngine;
using System.Collections;
using BRM.EventBrokers;
using BRM.EventBrokers.Interfaces;
using GenericFunctions;

public enum WeaponType
{
    Spear = 0,
    Lightning = 1,
    Flail = 2,
    None = 3
}

public class Jai : MonoBehaviour, IFreezable, IDie
{
    private static class Throw
    {
        public const int Idle = 0;
        public const int Down = 1;
        public const int Up = 2;
    }
    [SerializeField] private GameObject[] _weaponPrefabs = new GameObject[3];
    [SerializeField] private Transform[] _weaponSpawnParents;
    [SerializeField] private Animator _jaiAnimator;
    [SerializeField, Range(0,1400f)] private float _throwForceMagnitude = 1400f; //Force with which Jai throws the spear
    
    private const string _jaiName = nameof(Jai);
    private const float _distToThrow = .03f;

    private IBrokerEvents _eventBroker = new StaticEventBroker();
    
    private Weapon _myWeapon;
    private IUsable _weaponTrigger;
    private WeaponType _myWeaponType;

    private Vector2 _startingTouchPoint;
    private int _currentFingerId = Constants.UnusedFingerId;
    private bool _attacking;
    private bool _stabbing;
    private bool _isFrozen;
    private bool _isAlive;

    bool IFreezable.IsFrozen
    {
        get => _isFrozen;
        set => _isFrozen = value;
    }
    
    private void Awake()
    {
        _isAlive = true;
        Constants.JaiTransform = transform;
    }

    private void Start()
    {
        _eventBroker.Subscribe<ContinueData>(OnContinue);
        _eventBroker.Subscribe<WeaponGrabbedData>(OnCollectWeapon);
        OrderedTouchEventRegistry.Instance.OnTouchWorldBegin(typeof(Jai), OnTouchWorldBegin, true);
        OrderedTouchEventRegistry.Instance.OnTouchWorldEnd(typeof(Jai), OnTouchWorldEnd, true);
    }

    private void OnDestroy()
    {
        if (TouchInputManager.Instance == null)
        {
            return;
        }

        _eventBroker.Unsubscribe<ContinueData>(OnContinue);
        _eventBroker.Unsubscribe<WeaponGrabbedData>(OnCollectWeapon);
        OrderedTouchEventRegistry.Instance.OnTouchWorldBegin(typeof(Jai), OnTouchWorldBegin, false);
        OrderedTouchEventRegistry.Instance.OnTouchWorldEnd(typeof(Jai), OnTouchWorldEnd, false);
    }

    private void OnContinue(ContinueData data)
    {
        Rebirth();
    }

    public void Die()
    {
        _isAlive = false;
        _isFrozen = true;
    }

    public void Rebirth()
    {
        _isAlive = true;
        _isFrozen = false;
    }

    private void OnCollectWeapon(WeaponGrabbedData data)
    {
        _myWeaponType = data.Collectable.Collect();
        GenerateNewWeapon(_myWeaponType);

        switch (_myWeaponType)
        {
            case WeaponType.Spear:
                StartCoroutine(AnimateCollectSpear());
                break;
            case WeaponType.Lightning:
                StartCoroutine(AnimateCollectLightning());
                break;
            case WeaponType.Flail:
                StartCoroutine(AnimateCollectFlail());
                break;
            case WeaponType.None:
            default:
                Debug.LogError("unknown weapon type collected: " + _myWeaponType);
                break;
        }
    }

    private IEnumerator AnimateCollectSpear()
    {
        Debug.Log("Collected A Spear!");
        yield return null;
    }

    private IEnumerator AnimateCollectLightning()
    {
        Debug.Log("Collected Lightning!");
        yield return null;
    }

    private IEnumerator AnimateCollectFlail()
    {
        Debug.Log("Collected A Flail!");
        yield return null;
    }

    private void OnTouchWorldBegin(int fingerId, Vector2 worldPosition)
    {
        if (Pauser.Paused || !TouchInputManager.Instance.TryClaimFingerId(fingerId, _jaiName) || !_isAlive)
        {
            return;
        }

        _currentFingerId = fingerId;
        if (!_isFrozen)
        {
            _startingTouchPoint = worldPosition;
        }
        else if (!_stabbing)
        {
            StartCoroutine(StabTheBeast());
        }
    }

    private void OnTouchWorldEnd(int fingerId, Vector2 worldPosition)
    {
        if (Pauser.Paused || fingerId != _currentFingerId || _isFrozen || !_isAlive)
        {
            return;
        }

        _currentFingerId = Constants.UnusedFingerId;
        TouchInputManager.Instance.ReleaseFingerId(fingerId, _jaiName);
        
        var swipeDir = worldPosition - _startingTouchPoint;
        var releaseDist = swipeDir.magnitude;
        if (!_attacking)
        {
            if (releaseDist > _distToThrow && _myWeapon != null)
            {
                _weaponTrigger.UseMe(swipeDir.normalized * _throwForceMagnitude);
                StartCoroutine(AnimateUseWeapon(swipeDir));
            }
        }
    }

    private IEnumerator AnimateUseWeapon(Vector2 attackDir)
    {
        _attacking = true;
        switch (_myWeaponType)
        {
            case WeaponType.None:
                break;
            case WeaponType.Spear:
                StartCoroutine(PullOutNewSpear(Constants.Time2ThrowSpear));
                yield return StartCoroutine(AnimateThrowSpear(attackDir));
                break;
            case WeaponType.Lightning:
                yield return StartCoroutine(AnimateCastLightning(attackDir));
                break;
            case WeaponType.Flail:
                yield return StartCoroutine(AnimateSwingFlail(attackDir));
                break;
        }

        _attacking = false;
    }

    private IEnumerator AnimateThrowSpear(Vector2 throwDir)
    {
        var throwState = throwDir.y <= .2f ? Throw.Down : Throw.Up;
        transform.FaceForward(throwDir.x > 0);

        _jaiAnimator.SetInteger(Constants.AnimState, throwState);
        yield return new WaitForSeconds(Constants.Time2ThrowSpear);
        _jaiAnimator.SetInteger(Constants.AnimState, Throw.Idle);
    }

    private IEnumerator AnimateCastLightning(Vector2 swipeDir)
    {
        Debug.Log("Animating Lightning Strike!");
        yield return new WaitForSeconds(Constants.Time2StrikeLightning);
    }

    private IEnumerator AnimateSwingFlail(Vector2 swipeDir)
    {
        Debug.Log("Swing Mace for now");
        yield return null;
    }


    private IEnumerator StabTheBeast()
    {
        _stabbing = true;
        //jaiAnimator.SetInteger(Constants.AnimState,5);
        Tentacles.StabbableTentacle.GetStabbed(); //stab the tentacle!
        yield return new WaitForSeconds(.1f);
        _stabbing = false;
        //jaiAnimator.SetInteger(Constants.AnimState,0);
    }

    private IEnumerator PullOutNewSpear(float time2Wait)
    {
        yield return new WaitForSeconds(time2Wait);
        GenerateNewWeapon(WeaponType.Spear);
    }

    private void GenerateNewWeapon(WeaponType weaponType)
    {
        _myWeapon = Instantiate(_weaponPrefabs[(int) weaponType], _weaponSpawnParents[(int)weaponType], false).GetComponent<Weapon>();
        _weaponTrigger = _myWeapon;
    }
}