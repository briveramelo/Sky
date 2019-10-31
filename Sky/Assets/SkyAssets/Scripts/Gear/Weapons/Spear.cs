using UnityEngine;
using System.Collections;
using PixelArtRotation;
using GenericFunctions;

public class Spear : Weapon
{
    [SerializeField] private PixelRotation _pixelRotationScript; //allows for pixel perfect sprite rotations
    [SerializeField] private PixelPerfectSprite _pixelPerfectSpriteScript;
    [SerializeField] private Transform _spearTipParentTransform;

    protected override int WeaponNumber => _spearNumber;
    protected override Vector2 MyVelocity => _rigbod.velocity;

    private Rigidbody2D _rigbod; //the spear's rigidbody, created only upon throwing
    private Vector2 _throwAdjustmentVector = new Vector2(0f, .085f);
    private int _spearNumber;

    private void Start()
    {
        _spearNumber = TimesUsed;
        SetSpearAngle(Vector2.up);
    }

    private void SetSpearAngle(Vector2 direction)
    {
        var theSetAngle = ConvertAnglesAndVectors.ConvertVector2SpearAngle(direction);
        _pixelRotationScript.Angle = theSetAngle;
        _spearTipParentTransform.rotation = Quaternion.Euler(0f, 0f, theSetAngle);
    }

    private IEnumerator TiltAround()
    {
        _pixelPerfectSpriteScript.enabled = true;
        yield return null;
        while (true)
        {
            SetSpearAngle(_rigbod.velocity);
            yield return null;
        }
    }

    protected override void UseMe(Vector2 swipeForce)
    {
        base.UseMe(swipeForce);
        var myTran = transform;
        myTran.SetParent(null);
        myTran.position = (Vector2) Constants.JaiTransform.position + _throwAdjustmentVector;
        
        //ensure scale does not flip depending on Jai's orientation upon throwing
        var scale = myTran.localScale;
        scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;
        
        SetSpearAngle(swipeForce);
        _attackCollider.enabled = true;
        _rigbod = gameObject.AddComponent<Rigidbody2D>();
        _rigbod.AddForce(swipeForce);
        StartCoroutine(TiltAround());
        Destroy(gameObject, 3f);
    }

    protected override void DeliverDamage(Collider2D col)
    {
        base.DeliverDamage(col);

        //var bird = col.GetComponent<Bird>();
        //Deliver damage and redirect the spear as a bounce
        //_rigbod.velocity = bird.MyBirdStats.Health>0 ? 
        //	Vector2.Reflect(MyWeaponStats.Velocity,(transform.position-col.bounds.ClosestPoint (transform.position))) * 0.2f :
        //	MyWeaponStats.Velocity * .8f;

        Physics2D.IgnoreCollision(_attackCollider, col);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}