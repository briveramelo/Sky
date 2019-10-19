using UnityEngine;
using GenericFunctions;

public interface ILeaderToDuck
{
    void Scatter();
    int FormationIndex { get; set; }
}
// The Duck receives communication from the DuckLeader through this interface

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

    public override BirdType MyBirdType => BirdType.Duck;
    private IDuckToLeader _leader;
    private Transform _myFormationTransform;
    private const float _moveSpeed = 2.5f;
    private const float _maxSpeed = 4f;
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
        set
        {
            _rigbod.velocity = value;
            transform.FaceForward(_rigbod.velocity.x < 0);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        if (transform.parent)
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
    // Although it seems that flipping the sign of the duck's x or y velocity when the duck's position exceed the WorldDimensions
    // would eliminate extra lines of code, this is not the case.
    // In that scenario, the duck often travels far beyond the WorldDimension, reversing direction and velocity,
    // only to be trapped flipping its velocity each frame for some time. 
    // This solution eliminates that concern.
    private void BounceOnTheWalls()
    {
        var pos = transform.position;
        var overX = pos.x > Constants.WorldDimensions.x;
        var underX = pos.x < -Constants.WorldDimensions.x;
        var overY = pos.y > Constants.WorldDimensions.y;
        var underY = pos.y < -Constants.WorldDimensions.y;
        if (overX || underX || overY || underY)
        {
            CurrentVelocity = new Vector2(
                                  underX ? 1 : overX ? -1 : Mathf.Sign(_rigbod.velocity.x),
                                  underY ? 1 : overY ? -1 : Mathf.Sign(_rigbod.velocity.y)).normalized * _moveSpeed;
        }
    }

    private void StayInFormation()
    {
        transform.position = Vector3.MoveTowards(transform.position, _myFormationTransform.position, _maxSpeed * Time.deltaTime);
    }

    #region IDirectable

    void IDirectable.SetDuckDirection(DuckDirection scatterDirection)
    {
        CurrentVelocity = _scatterDir[(int) scatterDirection] * _moveSpeed;
    }

    #endregion

    #region ILeaderToDuck

    void ILeaderToDuck.Scatter()
    {
        ScoreSheet.Tallier.TallyThreat(Threat.FreeDuck);
        Scatter();
    }

    private void Scatter()
    {
        CurrentVelocity = _scatterDir[_formationIndex] * _moveSpeed;
        BirdStats.ModifyForEvent(3);
        _bouncing = true;
    }

    int ILeaderToDuck.FormationIndex
    {
        get => _formationIndex;
        set
        {
            _formationIndex = value;
            _myFormationTransform = _leader.FormationTransforms[_formationIndex];
        }
    }

    #endregion

    // Remember that final action some "Bird"s need to perform?
    // When a Duck with a DuckLeader dies, he/she lets the DuckLeader know to reorganize the Flying V formation
    protected override void DieUniquely()
    {
        if (_leaderScript)
        {
            _leader.OrganizeDucks(this);
        }

        base.DieUniquely();
    }
}