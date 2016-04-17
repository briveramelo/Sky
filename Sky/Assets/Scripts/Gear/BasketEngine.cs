using UnityEngine;
using GenericFunctions;

public interface IBumpable{
	void Bump(Vector2 bumpDir);
}

public class BasketEngine : MonoBehaviour, IBumpable, IHold {

	[SerializeField] private Rigidbody2D basketBody;
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
        Invoke("StabilizeBumpThreat", 7f);
    }

    void StabilizeBumpThreat() {
        ScoreSheet.Tallier.TallyThreat(Threat.BasketStabilized);
    }
}
