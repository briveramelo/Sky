using UnityEngine;
using GenericFunctions;

// The Duck receives communication from the DuckLeader through this interface
public interface ILeaderToDuck
{
    void Scatter();
    int FormationIndex { get; set; }
}

public enum DuckDirection
{
    UpRight = 0,
    UpLeft = 1,
    DownRight = 2,
    DownLeft = 3
}

public interface IDirectable
{
    void SetDuckDirection(DuckDirection scatterDirection);
}

public class Duck : Bird, ILeaderToDuck, IDirectable
{
    // The Duck will follow his/her DuckLeader, until the DuckLeader dies. 
    // Then, the Duck will aimlessly bounce around the screen until killed
    [SerializeField] private LeadDuck _leaderScript;
    [SerializeField] private AnimationCurve _newFormationTransition;
    [SerializeField] private float _newFormationTransitionDuration;
    protected override BirdType MyBirdType => BirdType.Duck;
    
    private const float _bounceSpeed = 2.5f / 4f;
    
    private IDuckToLeader _leader;
    private Transform _myFormationTransform;

    private Vector3 _newFormationStartLocalPosition;
    private float _newFormationStartTime;
    private int _formationIndex;
    private bool _bouncing;

    private Vector2[] _scatterDir =
    {
        new Vector2(1, 1).normalized,
        new Vector2(-1, 1).normalized,
        new Vector2(1, -1).normalized,
        new Vector2(-1, -1).normalized,
        new Vector2(1, 1).normalized,
        new Vector2(-1, 1).normalized
    };

    private Vector2 CurrentVelocity
    {
        get => _rigbod.velocity;
        set
        {
            _rigbod.velocity = value;
            transform.FaceForward(_rigbod.velocity.x < 0);
        }
    }
    
    void IDirectable.SetDuckDirection(DuckDirection scatterDirection)
    {
        CurrentVelocity = _scatterDir[(int) scatterDirection] * _bounceSpeed;
    }
    void ILeaderToDuck.Scatter()
    {
        ScoreSheet.Tallier.TallyThreat(Threat.FreeDuck);
        Scatter();
    }
    int ILeaderToDuck.FormationIndex
    {
        get => _formationIndex;
        set
        {
            _formationIndex = value;
            _myFormationTransform = _leader.GetFormationTransform(_formationIndex);
            _newFormationStartTime = Time.time;
            transform.SetParent(_myFormationTransform);
            _newFormationStartLocalPosition = transform.localPosition;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        if (_leaderScript)
        {
            _leader = _leaderScript;
        }
        else
        {
            Scatter();
        }
    }

    private void Update()
    {
        if (_bouncing)
        {
            BounceOnTheWalls();
        }
        else
        {
            StayInFormation();
        }
    }

    // This function restricts a duck's movement to the confines of the screen- an omage to DuckHunt
    // Although it seems that flipping the sign of the duck's x or y velocity when the duck's position exceed the WorldSize
    // would eliminate extra lines of code, this is not the case.
    // In that scenario, the duck often travels far beyond the WorldDimension, reversing direction and velocity,
    // only to be trapped flipping its velocity each frame for some time. 
    // This solution eliminates that concern.
    private void BounceOnTheWalls()
    {
        var pos = transform.position;
        var worldEdge = ScreenSpace.WorldEdge;
        var overX = pos.x > worldEdge.x;
        var underX = pos.x < -worldEdge.x;
        var overY = pos.y > worldEdge.y;
        var underY = pos.y < worldEdge.y;
        if (overX || underX || overY || underY)
        {
            CurrentVelocity = new Vector2(
                                  underX ? 1 : overX ? -1 : Mathf.Sign(_rigbod.velocity.x),
                                  underY ? 1 : overY ? -1 : Mathf.Sign(_rigbod.velocity.y)).normalized * _bounceSpeed;
        }
    }

    private void StayInFormation()
    {
        var secondsSinceNewFormationSet = Time.time - _newFormationStartTime;
        var normalizedTime = secondsSinceNewFormationSet / _newFormationTransitionDuration;
        if (normalizedTime > 1f)
        {
            transform.localPosition = Vector2.zero;
            return;
        }

        var targetProgress = _newFormationTransition.Evaluate(normalizedTime);
        var targetLocalPosition = (1 - targetProgress) * _newFormationStartLocalPosition;
        transform.localPosition = targetLocalPosition;
    }

    private void Scatter()
    {
        CurrentVelocity = _scatterDir[_formationIndex] * _bounceSpeed;
        BirdStats.ModifyForEvent(3);
        _bouncing = true;
    }

    protected override void OnDeath()
    {
        _leader?.OrganizeDucks(this);
        base.OnDeath();
    }
}