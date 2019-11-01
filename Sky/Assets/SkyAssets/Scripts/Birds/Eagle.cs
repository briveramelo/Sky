using UnityEngine;
using System.Collections;
using PixelArtRotation;
using GenericFunctions;

public class Eagle : Bird
{
    [SerializeField] private EagleFriends _eagleFriends;
    [SerializeField] private PixelRotation _pixelRotationScript;
    
    public override BirdType MyBirdType => BirdType.Eagle;
    private ITriggerSpawnable _myEagleFriends;
    private Vector3 _attackDir;

    private Vector2[] _startPos;
    private Vector2[] _moveDir;

    protected override void Awake()
    {
        base.Awake();
        _myEagleFriends = _eagleFriends;
        _startPos = new[]
        {
            new Vector2(-Constants.ScreenSizeWorldUnits.x, -Constants.ScreenSizeWorldUnits.y) * 1.2f,
            new Vector2(Constants.ScreenSizeWorldUnits.x, -Constants.ScreenSizeWorldUnits.y) * 1.2f
        };
        _moveDir = new[]
        {
            Constants.ScreenSizePixels.normalized,
            new Vector2(-Constants.ScreenSizePixels.x, Constants.ScreenSizePixels.y).normalized,
        };
        StartCoroutine(InitiateAttack(1f));
    }

    private IEnumerator InitiateAttack(float waitTime)
    {
        yield return null;
        _myEagleFriends.TriggerSpawnEvent();
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(SweepUp(true));
    }

    private IEnumerator SweepUp(bool first)
    {
        var moveSpeed = 5f;
        transform.FaceForward(first);
        transform.position = _startPos[first ? 0 : 1];
        _rigbod.velocity = Constants.SpeedMultiplier * _moveDir[first ? 0 : 1] * moveSpeed;
        _pixelRotationScript.Angle = ConvertAnglesAndVectors.ConvertVector2IntAngle(_moveDir[first ? 0 : 1]);
        yield return new WaitForSeconds(first ? 4f : 6f);
        if (first)
        {
            StartCoroutine(SweepUp(false));
        }
        else
        {
            _rigbod.velocity = Vector2.zero;
            Strike();
        }
    }

    private void Strike()
    {
        var xStartPoint = 20f;
        while (Mathf.Abs(xStartPoint) > Constants.ScreenSizeWorldUnits.x)
        {
            xStartPoint = Constants.BalloonCenter.position.x + Random.Range(-Constants.ScreenSizeWorldUnits.x, Constants.ScreenSizeWorldUnits.x) * .15f;
        }

        var strikeSpeed = 9f;
        Vector3 newPosition = new Vector2(xStartPoint, Constants.ScreenSizeWorldUnits.y * 1.2f);
        _attackDir = (Constants.BalloonCenter.position - newPosition).normalized;
        transform.position = newPosition;
        _rigbod.velocity = Constants.SpeedMultiplier * strikeSpeed * _attackDir;
        transform.FaceForward(_attackDir.x > 0);
        _pixelRotationScript.Angle = ConvertAnglesAndVectors.ConvertVector2IntAngle(_attackDir);

        StartCoroutine(InitiateAttack(5f));
    }
}