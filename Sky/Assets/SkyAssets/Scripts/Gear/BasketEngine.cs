using UnityEngine;
using GenericFunctions;

public interface IBumpable{
	void Bump(Vector2 bumpDir);
}

public interface IDie {
    void Die();
    void Rebirth();
}

public class BasketEngine : MonoBehaviour, IBumpable, IHold, IEnd, IDie {

	[SerializeField] Rigidbody2D rigbod;
	const float moveSpeed = 2.7f;
	bool movingEnabled =true;

	void IHold.OnTouchHeld(){
		if (movingEnabled){
			Vector2 moveDir = Vector2.ClampMagnitude(InputManager.touchSpot - Joyfulstick.startingJoystickSpot,Joyfulstick.joystickMaxMoveDistance);
			rigbod.velocity = moveDir * moveSpeed;
		}
	}

    void IEnd.OnTouchEnd() {
        rigbod.velocity = Vector2.zero;
    }

	void IBumpable.Bump(Vector2 bumpDir){
		StopAllCoroutines();
		rigbod.velocity = bumpDir;
		StartCoroutine (Bool.Toggle(boolState=>movingEnabled=boolState,.5f));
        ScoreSheet.Tallier.TallyThreat(Threat.BasketBumped);
        Invoke("StabilizeBumpThreat", 2f);
    }

    void StabilizeBumpThreat() {
        ScoreSheet.Tallier.TallyThreat(Threat.BasketStabilized);
    }

    void IDie.Die() {
        movingEnabled = false;
    }
    void IDie.Rebirth() {
        movingEnabled = true;
    }
}
