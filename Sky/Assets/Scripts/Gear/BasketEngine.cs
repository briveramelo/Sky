using UnityEngine;
using GenericFunctions;
using System.Linq;

public interface IBumpable{
	void Bump(Vector2 bumpDir);
}

public interface IDie {
    void Die();
    void Rebirth();
}

public class BasketEngine : MonoBehaviour, IBumpable, IHold, IDie {

	[SerializeField] Rigidbody2D basketBody;
	const float moveSpeed = 2.7f;
	bool movingEnabled =true;

	void IHold.OnTouchHeld(){
		if (movingEnabled){
			Vector2 moveDir = Vector2.ClampMagnitude(InputManager.touchSpot - Joyfulstick.startingJoystickSpot,Joyfulstick.joystickMaxMoveDistance);
			basketBody.velocity = moveDir * moveSpeed;
		}
	}

	void IBumpable.Bump(Vector2 bumpDir){
		StopAllCoroutines();
		basketBody.velocity = bumpDir;
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
