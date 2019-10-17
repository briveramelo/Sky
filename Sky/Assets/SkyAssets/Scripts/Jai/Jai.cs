using UnityEngine;
using System.Collections;
using GenericFunctions;

public enum WeaponType {
    Spear =0,
    Lightning=1,
    Flail=2,
    None=3
}

public class Jai : MonoBehaviour, IBegin, IEnd, IFreezable {

	[SerializeField] private GameObject[] weaponPrefabs = new GameObject[3];
	[SerializeField] private Animator jaiAnimator;
	private Weapon myWeapon;
	private IUsable weaponTrigger;
	private WeaponType MyWeaponType;
	private IJaiID inputManager;

	private const float distToThrow = .03f;
	private bool attacking, stabbing, beingHeld;
	bool IFreezable.IsFrozen{get => beingHeld;
		set => beingHeld = value;
	}

	private Vector2 startingTouchPoint;

	private void Awake(){
		Constants.jaiTransform = transform;
		inputManager = FindObjectOfType<InputManager>().GetComponent<IJaiID>();
	}

	private Vector3[] spawnSpots = new Vector3[] {
        new Vector3 (0.14f, 0.12f,0f),
        Vector3.zero,
        Vector3.zero
    };
    public IEnumerator CollectNewWeapon(ICollectable collectableWeapon) {
        MyWeaponType = collectableWeapon.GetCollected();
        GenerateNewWeapon(MyWeaponType);

        switch (MyWeaponType) {
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

    private IEnumerator AnimateCollectSpear() {
        Debug.Log("Collected A Spear!");
        yield return null;
    }

    private IEnumerator AnimateCollectLightning() {
        Debug.Log("Collected Lightning!");
        yield return null;
    }

    private IEnumerator AnimateCollectFlail() {
        Debug.Log("Collected A Flail!");
        yield return null;
    }

	void IBegin.OnTouchBegin(int fingerID){
        if (!Pauser.Paused) {
            if (!beingHeld){
			    float distFromStick = Vector2.Distance(InputManager.touchSpot,Joyfulstick.startingJoystickSpot);
			    float distFromPause = Vector2.Distance(InputManager.touchSpot,Pauser.PauseSpot);
			    if (distFromStick>Joyfulstick.joystickMaxStartDist && distFromPause > Pauser.pauseRadius){
				    if (Input.touchCount<3){
					    startingTouchPoint = InputManager.touchSpot;
					    inputManager.SetJaiID(fingerID);
				    }
			    }
		    }
		    else{
			    if (!stabbing){
				    StartCoroutine(StabTheBeast());
			    }
		    }
        }
	}

	void IEnd.OnTouchEnd(){
        if (!Pauser.Paused) {
            Vector2 swipeDir = InputManager.touchSpot- startingTouchPoint;
            float releaseDist = swipeDir.magnitude;
		    if (!attacking){
			    if ( releaseDist > distToThrow && myWeapon!=null){
                    weaponTrigger.UseMe(swipeDir);
                    StartCoroutine (AnimateUseWeapon(swipeDir));
			    }
		    }
        }
	}

	private IEnumerator AnimateUseWeapon(Vector2 attackDir) {
        attacking = true;
        switch (MyWeaponType) {
            case WeaponType.None:
                break;
            case WeaponType.Spear:
				StartCoroutine(PullOutNewSpear(Constants.time2ThrowSpear));
                yield return StartCoroutine(AnimateThrowSpear(attackDir));
                break;
            case WeaponType.Lightning:
                yield return StartCoroutine(AnimateCastLightning(attackDir));
                break;
            case WeaponType.Flail:
                yield return StartCoroutine(AnimateSwingFlail(attackDir));
                break;
        }
        attacking = false;
    }

	private enum Throw{
		Idle=0,
		Down=1,
		Up=2,
	}

	private IEnumerator AnimateThrowSpear(Vector2 throwDir){
		Throw ThrowState = throwDir.y<=.2f ? Throw.Down : Throw.Up;
        transform.FaceForward(throwDir.x > 0);

		jaiAnimator.SetInteger("AnimState",(int)ThrowState);
		yield return new WaitForSeconds (Constants.time2ThrowSpear);
		jaiAnimator.SetInteger("AnimState",(int)Throw.Idle);
	}

	private IEnumerator AnimateCastLightning(Vector2 swipeDir) {
        Debug.Log("Animating Lightning Strike!");
        yield return new WaitForSeconds(Constants.time2StrikeLightning);
    }

	private IEnumerator AnimateSwingFlail(Vector2 swipeDir) {
        Debug.Log("Swing Mace for now");
        yield return null;
    }


	private IEnumerator StabTheBeast(){
        stabbing = true;
		//jaiAnimator.SetInteger("AnimState",5);
		Tentacles.StabbableTentacle.GetStabbed(); //stab the tentacle!
		yield return new WaitForSeconds (.1f);
		stabbing = false;
		//jaiAnimator.SetInteger("AnimState",0);
	}

	private IEnumerator PullOutNewSpear(float time2Wait){
		yield return new WaitForSeconds (time2Wait);
        GenerateNewWeapon(WeaponType.Spear);
	}

	private void GenerateNewWeapon(WeaponType weaponType) {
        myWeapon = Instantiate (weaponPrefabs[(int)weaponType], transform.position + spawnSpots[(int)weaponType], Quaternion.identity).GetComponent<Weapon>();
        weaponTrigger = myWeapon;
    }
}