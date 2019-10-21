using System;
using UnityEngine;
using System.Collections;
using GenericFunctions;

public enum WeaponType
{
    Spear = 0,
    Lightning = 1,
    Flail = 2,
    None = 3
}

public class Jai : MonoBehaviour, IFreezable
{
    [SerializeField] private GameObject[] _weaponPrefabs = new GameObject[3];
    [SerializeField] private Animator _jaiAnimator;

    private const float _distToThrow = .03f;

    private int _currentFingerId = -1;
    private Weapon _myWeapon;
    private IUsable _weaponTrigger;
    private WeaponType _myWeaponType;

    private Vector2 _startingTouchPoint;
    private bool _attacking, _stabbing, _beingHeld;
    private const string _jaiName = nameof(Jai);
    private Vector3[] _spawnSpots = 
    {
        new Vector3(0.14f, 0.12f, 0f),
        Vector3.zero,
        Vector3.zero
    };
    private static class Throw
    {
        public const int Idle = 0;
        public const int Down = 1;
        public const int Up = 2;
    }

    bool IFreezable.IsFrozen
    {
        get => _beingHeld;
        set => _beingHeld = value;
    }

    private void Awake()
    {
        Constants.JaiTransform = transform;
    }

    private void Start()
    {
        TouchInputManager.Instance.OnTouchBegin += OnTouchBegin;
        TouchInputManager.Instance.OnTouchEnd += OnTouchEnd;
    }

    private void OnDestroy()
    {
        TouchInputManager.Instance.OnTouchBegin -= OnTouchBegin;
        TouchInputManager.Instance.OnTouchEnd -= OnTouchEnd;
    }

    public IEnumerator CollectNewWeapon(ICollectable collectableWeapon)
    {
        _myWeaponType = collectableWeapon.GetCollected();
        GenerateNewWeapon(_myWeaponType);

        switch (_myWeaponType)
        {
            case WeaponType.None:
                break;
            case WeaponType.Spear:
                yield return StartCoroutine(AnimateCollectSpear());
                break;
            case WeaponType.Lightning:
                yield return StartCoroutine(AnimateCollectLightning());
                break;
            case WeaponType.Flail:
                yield return StartCoroutine(AnimateCollectFlail());
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

    private void OnTouchBegin(int fingerId, Vector2 fingerPosition)
    {
        if (Pauser.Paused || !TouchInputManager.Instance.TryClaimFingerId(fingerId, _jaiName))
        {
            return;
        }

        _currentFingerId = fingerId;
        if (!_beingHeld)
        {
            _startingTouchPoint = fingerPosition;
        }
        else if (!_stabbing)
        {
            StartCoroutine(StabTheBeast());
        }
    }

    private void OnTouchEnd(int fingerId, Vector2 fingerPosition)
    {
        if (Pauser.Paused || fingerId != _currentFingerId)
        {
            return;
        }

        _currentFingerId = -1;
        TouchInputManager.Instance.ReleaseFingerId(fingerId, _jaiName);
        
        var swipeDir = fingerPosition - _startingTouchPoint;
        var releaseDist = swipeDir.magnitude;
        if (!_attacking)
        {
            if (releaseDist > _distToThrow && _myWeapon != null)
            {
                _weaponTrigger.UseMe(swipeDir);
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
        _myWeapon = Instantiate(_weaponPrefabs[(int) weaponType], transform.position + _spawnSpots[(int) weaponType], Quaternion.identity).GetComponent<Weapon>();
        _weaponTrigger = _myWeapon;
    }
}