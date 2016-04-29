using UnityEngine;
using System.Collections;
using GenericFunctions;
using System.Collections.Generic;

public class Lightning : Weapon {

    [SerializeField] LineRenderer myLineRenderer;

    Vector2 strikeDir;
    protected override int weaponNumber {get {return timesUsed; } }
    protected override Vector2 MyVelocity {get { return strikeDir * 2f; } }

    protected override void DeliverDamage(Collider2D col) {
        base.DeliverDamage(col);
        // code here

        Bool.Toggle(collisionDisabled=>Physics2D.IgnoreCollision(attackCollider, col, !collisionDisabled), 1.5f);
    }

    bool isUpgraded = true;
    Dictionary<StrikeDir, Vector3> StrikeDirections = new Dictionary<StrikeDir, Vector3>() {
        {StrikeDir.Up ,     Vector3.up},
        {StrikeDir.Down ,   Vector3.down},
        {StrikeDir.Left ,   Vector3.left},
        {StrikeDir.Right ,  Vector3.right},
    };
    enum StrikeDir {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,
    }
    protected override void UseMe(PointVector2 spotSwipe) {
        base.UseMe(spotSwipe);

        StrikeDir MyStrikeDir = GetStrikeDir(spotSwipe);
        strikeDir = StrikeDirections[MyStrikeDir];
        bool isVertical = strikeDir.x == 0;
        float spawnFloat = isVertical ? (spotSwipe.origin.x + spotSwipe.destination.x ) / 2f : (spotSwipe.origin.y + spotSwipe.destination.y ) / 2f;
        StartCoroutine(Strike(MyStrikeDir, spawnFloat));
    }

    #region GetStrikeDir
    StrikeDir GetStrikeDir(PointVector2 spotSwipe) {
        if (!isUpgraded || Mathf.Abs(spotSwipe.vector.y) > Mathf.Abs(spotSwipe.vector.x)) {
            if (Mathf.Sign(spotSwipe.vector.y)>0) {
                return StrikeDir.Up;
            }
            else {
                return StrikeDir.Down;
            }
        }
        else {
            if (Mathf.Sign(spotSwipe.vector.x)>0) {
                return StrikeDir.Right;
            }
            else {
                return StrikeDir.Left;
            }
        }
    }
    #endregion

    IEnumerator Strike(StrikeDir MyStrikeDir, float spawnFloat) {
        birdsHit = 0;
        myLineRenderer.enabled = true;
        attackCollider.enabled = true;
        Vector3 strikeDir = StrikeDirections[MyStrikeDir];
        bool isVertical = strikeDir.x == 0;
        bool movesPositive = strikeDir.x > 0 || strikeDir.y > 0;
        int side = movesPositive ? 1 : -1;
        if (isVertical) {
            transform.position = new Vector3(spawnFloat, 0f, 0f);
            transform.rotation = Quaternion.identity;
        }
        else {
            transform.position = new Vector3(0f, spawnFloat, 0f);
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }

        yield return new WaitForSeconds(.25f);
        attackCollider.enabled = false;
        myLineRenderer.enabled = false;
    }
}
