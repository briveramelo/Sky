using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Bat : Bird
{
    // Simulate a bat's erratic flight pattern
    // Set to approach and orbit the player and his vulnerable balloons

    // Intentional following-lag is introduced to allow for collision between bat / balloon
    // From a game design standpoint, this forces the player to slow movement to avoid this dangerous collision

    #region Initialize Variables
    protected override BirdType MyBirdType => BirdType.Bat;
    
    private Vector2[] _targetPositions;
    private Vector2 _ellipseTrace;

    private float[] _orbitalRadii = {1.25f/4, 1.5f/4, 1.75f/4};

    // Overwrite the first few frames of the bat's positional-targeting array with the last few
    // Set the _targetIndex "frameDelay" #frames before the realTime Index
    // In effect, create a following delay to allow for bat/balloon collision
    private const int _frameDelay = 2;
    private const int _positionWindowLength = 15;
    private const float _approachSpeed = 2.8f/3f;
    private float _orbitSpeedMultiplier = 2.16f;
    private const float _orbitDistance = 1.5f/4;
    private const float _stepDistance = 0.4f/4f;

    private float _curvature;
    private float _ellipseTilt;
    private float _speedPhaseShift;
    private float _ellipseAng;

    private int _realTimeIndex;
    private int _targetIndex;
    private int _xRadiusIndex;
    private int _yRadiusIndex;

    private bool _clockwise;
    private bool _orbiting;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        _targetPositions = new Vector2[_positionWindowLength];
        StartCoroutine(Approach());
    }

    // Flap straight towards the first point on the elliptical perimeter
    // Start _orbiting once close enough
    private IEnumerator Approach()
    {
        while (true)
        {
            UpdatePositionIndices();
            Vector2 batPos = transform.position;
            _targetPositions[_realTimeIndex] = (Vector2) Constants.BalloonCenter.position - _orbitDistance * _rigbod.velocity.normalized;
            var dist2Target = Vector2.Distance(_targetPositions[_targetIndex], batPos);

            // Once close enough, stop approaching and start _orbiting
            if (dist2Target < _orbitDistance)
            {
                StartCoroutine(Orbit());
                yield break;
            }

            var moveDir = (_targetPositions[_targetIndex] - batPos).normalized;
            _rigbod.velocity = _approachSpeed * moveDir;
            transform.FaceForward(transform.position.x > Constants.BalloonCenter.position.x);
            yield return null;
        }
    }

    private void UpdatePositionIndices()
    {
        _realTimeIndex++;
        _targetIndex++;
        if (_realTimeIndex >= _positionWindowLength)
        {
            ResetTargetPositionWindow();
        }
    }

    private void ResetTargetPositionWindow()
    {
        for (var i = 0; i < _frameDelay; i++)
        {
            _targetPositions[i] = _targetPositions[_positionWindowLength - _frameDelay - 1 + i];
        }

        _realTimeIndex = _frameDelay;
        _targetIndex = 0;
    }

    // Move in an ellipse
    // Set speed proportional to the _curvature of the point on the ellipse
    private IEnumerator Orbit()
    {
        ShuffleOrbitalPhase();
        _orbiting = true;
        ScoreSheet.Tallier.TallyThreat(Threat.BatSurrounding);
        _ellipseAng = ConvertAnglesAndVectors.ConvertVector2FloatAngle(-_rigbod.velocity);
        _targetPositions[_realTimeIndex] = FindEllipsePosition();

        while (true)
        {
            Vector2 batPos = transform.position;
            var dist2Target = Vector2.Distance(_targetPositions[_targetIndex], batPos);
            if (dist2Target < _stepDistance)
            {
                UpdatePositionIndices();
                _ellipseAng = FindTargetAngle();
                _targetPositions[_realTimeIndex] = FindEllipsePosition();
            }

            var moveDir = (_targetPositions[_targetIndex] - batPos).normalized;
            _curvature = _orbitalRadii[_xRadiusIndex] * _orbitalRadii[_yRadiusIndex] /
                         Mathf.Pow(_orbitalRadii[_xRadiusIndex] * _orbitalRadii[_xRadiusIndex] *
                                   Mathf.Sin((_ellipseAng + _speedPhaseShift) * Mathf.Deg2Rad) * Mathf.Sin((_ellipseAng + _speedPhaseShift) * Mathf.Deg2Rad) +
                                   _orbitalRadii[_yRadiusIndex] * _orbitalRadii[_yRadiusIndex] *
                                   Mathf.Cos(_ellipseAng * Mathf.Deg2Rad) * Mathf.Cos(_ellipseAng * Mathf.Deg2Rad), 1.5f);
            var orbitSpeed = _orbitSpeedMultiplier / _curvature;

            _rigbod.velocity = orbitSpeed * moveDir;
            transform.FaceForward(transform.position.x > Constants.BalloonCenter.position.x);
            yield return null;
        }
    }

    // Set properties of the ellipse
    // Randomize these properties periodically for erratic flight
    private void ShuffleOrbitalPhase()
    {
        _ellipseTilt = Random.Range(-30f, 30f);
        _speedPhaseShift = _ellipseTilt * 2f / 3f;
        _xRadiusIndex = Random.Range(0, 3);
        _yRadiusIndex = _xRadiusIndex == 1 ? 1 + (int) Mathf.Sign(Random.insideUnitCircle.x) : 1;
        _clockwise = Bool.TossCoin();
        Invoke(nameof(ShuffleOrbitalPhase), Random.Range(2f, 4f));
    }

    // Help define the elliptical pattern
    private float FindTargetAngle()
    {
        const float decay = 0.0167f * 4f;
        var targetAng = _clockwise ? _ellipseAng + 90f : _ellipseAng - 90f;
        return Mathf.LerpAngle(_ellipseAng, targetAng, decay);
    }

    // Set an elliptical pattern around the balloons
    // Update position indices & ensure the Bat is facing the player
    private Vector2 FindEllipsePosition()
    {
        _ellipseTrace = new Vector2(_orbitalRadii[_xRadiusIndex] * Mathf.Cos((_ellipseAng + _ellipseTilt) * Mathf.Deg2Rad),
            _orbitalRadii[_yRadiusIndex] * Mathf.Sin(_ellipseAng * Mathf.Deg2Rad));
        return (Vector2) Constants.BalloonCenter.position + _ellipseTrace;
    }

    protected override void OnDeath()
    {
        if (_orbiting)
        {
            ScoreSheet.Tallier.TallyThreat(Threat.BatLeft);
        }

        base.OnDeath();
    }
}