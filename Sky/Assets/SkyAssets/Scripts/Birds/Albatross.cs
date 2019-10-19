using UnityEngine;
using GenericFunctions;

public class Albatross : Bird {
    private bool _shouldWaitToTurn;
    private const float _moveSpeed = 1.065f;

    private void Update () {
        Vector2 moveDir = Constants.BalloonCenter.position - transform.position;
        if (Vector2.Distance(moveDir, Vector2.zero)>0.1f) {
            _rigbod.velocity = (moveDir).normalized * _moveSpeed;
            transform.FaceForward(_rigbod.velocity.x<0);
        }
        else {
            _rigbod.velocity = Vector2.Lerp(_rigbod.velocity, Vector2.zero, 0.1f);
            if (!_shouldWaitToTurn) {
                StartCoroutine(Bool.Toggle(theBool => _shouldWaitToTurn = !theBool, .75f));
                transform.FaceForward(Bool.TossCoin());
            }
        }
	}
		
	protected override int TakeDamage (ref WeaponStats weaponStats){
		float hitHeight = _birdCollider.bounds.ClosestPoint(weaponStats.WeaponCollider.transform.position).y;
        int damageToTake = 3;
        int damageDealt;
        if (weaponStats.Velocity.y > 0 && hitHeight < transform.position.y){ //kill albatross with a tactical shot to the underbelly
            damageDealt = BirdStats.Health >= damageToTake ? damageToTake : BirdStats.Health;
        }
        else {
            damageDealt = weaponStats.Damage;
        }
        BirdStats.Health -= damageDealt;
        Instantiate(_guts, transform.position, Quaternion.identity).GetComponent<IBleedable>().GenerateGuts(ref BirdStats, weaponStats.Velocity);
        Debug.Log(damageDealt);
        return damageDealt;
	}
}