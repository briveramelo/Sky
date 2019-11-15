using System;
using UnityEngine;
using GenericFunctions;

public class Albatross : Bird
{
    protected override BirdType MyBirdType => BirdType.Albatross;
    
    private const float _moveSpeed = 1.065f;

    [SerializeField] private float _dist;
    private const float _fixedSpeedDistWorldUnits = 0.1f;
    private bool _shouldWaitToTurn;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, _fixedSpeedDistWorldUnits);
    }

    private void Update()
    {
        Vector2 moveDir = EasyAccess.BalloonCenter.position - transform.position;
        if (Vector2.Distance(moveDir, Vector2.zero) > _fixedSpeedDistWorldUnits)
        {
            //fixed speed approach toward balloons
            _rigbod.velocity = Constants.SpeedMultiplier * _moveSpeed * moveDir.normalized;
            transform.FaceForward(_rigbod.velocity.x < 0);
        }
        else
        {
            //decayed speed approach toward balloons when close
            _rigbod.velocity = Constants.SpeedMultiplier * Vector2.Lerp(_rigbod.velocity, Vector2.zero, 0.1f);
            if (!_shouldWaitToTurn)
            {
                //prevent jittery, rapid alternation of direction
                StartCoroutine(Bool.Toggle(theBool => _shouldWaitToTurn = !theBool, .75f));
                transform.FaceForward(Bool.TossCoin());
            }
        }
    }

    protected override int TakeDamage(ref WeaponStats weaponStats)
    {
        var hitHeight = _birdCollider.bounds.ClosestPoint(weaponStats.WeaponCollider.transform.position).y;
        const int damageToTake = 4;
        int damageDealt;
        if (weaponStats.Velocity.y > 0 && hitHeight < transform.position.y)
        {
            //kill albatross with a tactical shot to the underbelly
            damageDealt = BirdStats.Health >= damageToTake ? damageToTake : BirdStats.Health;
        }
        else
        {
            damageDealt = weaponStats.Damage;
        }

        BirdStats.Health -= damageDealt;
        Instantiate(_guts, transform.position, Quaternion.identity).GetComponent<IBleedable>().GenerateGuts(ref BirdStats, weaponStats.Velocity);
        return damageDealt;
    }
}