using UnityEngine;
using System.Collections.Generic;
using GenericFunctions;

public interface IDuckToLeader
{
    void OrganizeDucks(ILeaderToDuck deadDuck);
    Transform[] FormationTransforms { get; }
}
// The DuckLeader communicates with Ducks through this interface

public class LeadDuck : Bird, IDuckToLeader
{
    // The DuckLeader ensures all ducks follow as closely behind in an evenly distributed Flying V Formation
    // The DuckLeader will fly linearly across the screen

    [SerializeField] private Duck[] _duckScripts;
    [SerializeField] private Transform[] _formationTransforms;
    public override BirdType MyBirdType => BirdType.DuckLeader;
    private List<ILeaderToDuck> ducks;

    protected override void Awake()
    {
        base.Awake();
        ducks = new List<ILeaderToDuck>(_duckScripts);
        var goLeft = transform.position.x > 0;
        transform.FaceForward(goLeft);
        _rigbod.velocity = Constants.SpeedMultiplier * 2.5f * new Vector2(goLeft ? -1 : 1, 0);
        SetDuckFormation(goLeft);
    }

    // Set ducks into the "Flying V" Formation, like so:
    //	  [4]
    //	  	[2]
    //	  	  [0]
    //  		[leader]  --->
    // 	  	  [1]
    //		[3]
    //	  [5]

    private void SetDuckFormation(bool goLeft)
    {
        var topSide = ConvertAnglesAndVectors.ConvertAngleToVector2(goLeft ? 30 : 150);
        var bottomSide = ConvertAnglesAndVectors.ConvertAngleToVector2(goLeft ? -30 : 210);

        var separationDistance = 0.15f;
        for (var i = 0; i < _formationTransforms.Length; i++)
        {
            _formationTransforms[i].localPosition = (goLeft ? 1 : -1) * (separationDistance) * (Mathf.Floor(f: i / 2) + 1) * (i % 2 == 0 ? topSide : bottomSide);
            ducks[i].FormationIndex = i;
        }
    }

    #region IDuckToLeader

    Transform[] IDuckToLeader.FormationTransforms => _formationTransforms;

    // Review the above "Flying V" Formation and the attached video "FlyingV_United" to see this logic in action
    void IDuckToLeader.OrganizeDucks(ILeaderToDuck deadDuck)
    {
        var deadNumber = deadDuck.FormationIndex;
        var topCount = 0;
        var bottomCount = 0;

        for (var i = 0; i < ducks.Count; i++)
        {
            topCount += ducks[i].FormationIndex % 2 == 0 ? 1 : 0; //birds on top use even indices
            bottomCount += ducks[i].FormationIndex % 2 != 0 ? 1 : 0; //		bottom use odd indices

            if (ducks[i].FormationIndex > deadNumber && ducks[i].FormationIndex % 2 == deadNumber % 2)
            {
                ducks[i].FormationIndex -= 2;
            }
        }

        if (topCount < bottomCount && deadNumber % 2 == 0)
        {
            var highestOdd = bottomCount * 2 - 1;
            ducks.Find(duck => duck.FormationIndex == highestOdd).FormationIndex -= 3;
        }
        else if (bottomCount < topCount && deadNumber % 2 != 0)
        {
            var highestEven = (topCount - 1) * 2;
            ducks.Find(duck => duck.FormationIndex == highestEven).FormationIndex -= 1;
        }

        ducks.Remove(deadDuck);
    }

    #endregion

    // Remember that final action some "Bird"s need to perform?
    // The DuckLeader Breaks the Flying V Formation and tells each Duck to Scatter when he/she dies
    protected override void DieUniquely()
    {
        BreakTheV();
        base.DieUniquely();
    }

    private void BreakTheV()
    {
        transform.DetachChildren();
        ducks.ForEach(duck => duck.Scatter());
        for (var i = 0; i < _formationTransforms.Length; i++)
        {
            Destroy(_formationTransforms[i].gameObject);
        }
    }
}