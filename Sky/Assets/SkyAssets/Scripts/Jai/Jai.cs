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

public class Jai : MonoBehaviour, IBegin, IEnd, IFreezable
{
    [SerializeField] private GameObject[] _weaponPrefabs = new GameObject[3];
    [SerializeField] private Animator _jaiAnimator;

    private const float _distToThrow = .03f;

    private Weapon _myWeapon;
    private IUsable _weaponTrigger;
    private WeaponType _myWeaponType;
    private IJaiId _inputManager;


    private Vector2 _startingTouchPoint;
    private bool _attacking, _stabbing, _beingHeld;

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
        _inputManager = FindObjectOfType<InputManager>().GetComponent<IJaiId>();
    }

    private Vector3[] spawnSpots = new Vector3[]
    {
        new Vector3(0.14f, 0.12f, 0f),
        Vector3.zero,
        Vector3.zero
    };

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

    void IBegin.OnTouchBegin(int fingerId)
    {
        if (!Pauser.Paused)
        {
            if (!_beingHeld)
            {
                var distFromStick = Vector2.Distance(InputManager.TouchSpot, Joyfulstick.StartingJoystickSpot);
                var distFromPause = Vector2.Distance(InputManager.TouchSpot, Pauser.PauseSpot);
                if (distFromStick > Joyfulstick.JoystickMaxStartDist && distFromPause > Pauser.PauseRadius)
                {
                    if (Input.touchCount < 3)
                    {
                        _startingTouchPoint = InputManager.TouchSpot;
                        _inputManager.SetJaiId(fingerId);
                    }
                }
            }
            else
            {
                if (!_stabbing)
                {
                    StartCoroutine(StabTheBeast());
                }
            }
        }
    }

    void IEnd.OnTouchEnd()
    {
        if (!Pauser.Paused)
        {
            var swipeDir = InputManager.TouchSpot - _startingTouchPoint;
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

    private enum Throw
    {
        Idle = 0,
        Down = 1,
        Up = 2,
    }

    private IEnumerator AnimateThrowSpear(Vector2 throwDir)
    {
        var throwState = throwDir.y <= .2f ? Throw.Down : Throw.Up;
        transform.FaceForward(throwDir.x > 0);

        _jaiAnimator.SetInteger("AnimState", (int) throwState);
        yield return new WaitForSeconds(Constants.Time2ThrowSpear);
        _jaiAnimator.SetInteger("AnimState", (int) Throw.Idle);
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
        //jaiAnimator.SetInteger("AnimState",5);
        Tentacles.StabbableTentacle.GetStabbed(); //stab the tentacle!
        yield return new WaitForSeconds(.1f);
        _stabbing = false;
        //jaiAnimator.SetInteger("AnimState",0);
    }

    private IEnumerator PullOutNewSpear(float time2Wait)
    {
        yield return new WaitForSeconds(time2Wait);
        GenerateNewWeapon(WeaponType.Spear);
    }

    private void GenerateNewWeapon(WeaponType weaponType)
    {
        _myWeapon = Instantiate(_weaponPrefabs[(int) weaponType], transform.position + spawnSpots[(int) weaponType], Quaternion.identity).GetComponent<Weapon>();
        _weaponTrigger = _myWeapon;
    }
}