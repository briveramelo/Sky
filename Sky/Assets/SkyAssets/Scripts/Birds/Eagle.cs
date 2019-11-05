using UnityEngine;
using System.Collections;
using PixelArtRotation;
using GenericFunctions;

public class Eagle : Bird
{
    [SerializeField] private EagleFriends _eagleFriends;
    [SerializeField] private PixelRotation _pixelRotationScript;
    
    protected override BirdType MyBirdType => BirdType.Eagle;
    private ITriggerSpawnable _myEagleFriends;
    private Vector3 _attackDir;

    private Vector2[] _startPos;
    private Vector2[] _moveDir;

    protected override void Awake()
    {
        base.Awake();
        _myEagleFriends = _eagleFriends;
        var screenSizeWorldUnits = ScreenSpace.WorldEdge;
        var xEdge = screenSizeWorldUnits.x;
        var yEdge = screenSizeWorldUnits.y;

        var normalizedSize = screenSizeWorldUnits.normalized;
        _startPos = new[]
        {
            new Vector2(-xEdge, -yEdge) - normalizedSize * 0.2f,
            new Vector2(xEdge, -yEdge) + new Vector2(normalizedSize.x, - normalizedSize.y) * 0.2f
        };
        _moveDir = new[]
        {
            normalizedSize,
            new Vector2(-normalizedSize.x, normalizedSize.y),
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
        transform.FaceForward(first);
        transform.position = _startPos[first ? 0 : 1];
        
        const float sweepSpeed = 5f/4;
        _rigbod.velocity = sweepSpeed * _moveDir[first ? 0 : 1];
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
        var xStartPoint = Mathf.Infinity;
        var screenSize = ScreenSpace.WorldEdge;
        while (Mathf.Abs(xStartPoint) > screenSize.x)
        {
            xStartPoint = Constants.BalloonCenter.position.x + Random.Range(-screenSize.x, screenSize.x) * .15f;
        }

        Vector3 newPosition = new Vector2(xStartPoint, screenSize.y + 0.2f);
        _attackDir = (Constants.BalloonCenter.position - newPosition).normalized;
        transform.position = newPosition;
        const float strikeSpeed = 9f/4;
        _rigbod.velocity = strikeSpeed * _attackDir;
        transform.FaceForward(_attackDir.x > 0);
        _pixelRotationScript.Angle = ConvertAnglesAndVectors.ConvertVector2IntAngle(_attackDir);

        StartCoroutine(InitiateAttack(5f));
    }
}