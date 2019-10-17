﻿using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Lightning : Weapon {

    [SerializeField] private LineRenderer myLineRenderer;

    private Vector2 strikeDir;
    protected override int weaponNumber => timesUsed;
    protected override Vector2 MyVelocity => strikeDir.normalized * 2f;

    protected override void DeliverDamage(Collider2D col) {
        base.DeliverDamage(col);
        // code here

        Bool.Toggle(collisionDisabled=>Physics2D.IgnoreCollision(attackCollider, col, !collisionDisabled), 1.5f);
    }


    private void Awake() {
        transform.parent = Constants.jaiTransform.parent;
    }
    protected override void UseMe(Vector2 swipeDir) {
        base.UseMe(swipeDir);
        strikeDir = swipeDir;
        StartCoroutine(Strike(swipeDir));
    }

    private IEnumerator Strike(Vector2 swipeDir) {
        birdsHit = 0;
        myLineRenderer.enabled = true;
        attackCollider.enabled = true;        
        transform.rotation = Quaternion.Euler(0f,0f,ConvertAnglesAndVectors.ConvertVector2SpearAngle(swipeDir));
        yield return new WaitForSeconds(.25f);
        attackCollider.enabled = false;
        myLineRenderer.enabled = false;
    }
}
