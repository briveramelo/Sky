using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Lightning : Weapon
{
    [SerializeField] private LineRenderer _myLineRenderer;

    private Vector2 _strikeDir;
    protected override int WeaponNumber => TimesUsed;
    protected override Vector2 MyVelocity => _strikeDir.normalized * 2f;

    protected override void DeliverDamage(Collider2D col)
    {
        base.DeliverDamage(col);
        // code here

        Bool.Toggle(collisionDisabled => Physics2D.IgnoreCollision(_attackCollider, col, !collisionDisabled), 1.5f);
    }

    protected override void UseMe(Vector2 swipeDir)
    {
        base.UseMe(swipeDir);
        _strikeDir = swipeDir;
        StartCoroutine(Strike(swipeDir));
    }

    private IEnumerator Strike(Vector2 swipeDir)
    {
        BirdsHit = 0;
        _myLineRenderer.enabled = true;
        _attackCollider.enabled = true;
        transform.rotation = Quaternion.Euler(0f, 0f, ConvertAnglesAndVectors.ConvertVector2SpearAngle(swipeDir));
        yield return new WaitForSeconds(.25f);
        _attackCollider.enabled = false;
        _myLineRenderer.enabled = false;
    }
}