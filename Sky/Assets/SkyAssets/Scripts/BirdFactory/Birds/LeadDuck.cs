using System;
using UnityEngine;
using System.Collections.Generic;
using GenericFunctions;

public interface IDuckToLeader
{
    void OrganizeDucks(ILeaderToDuck deadDuck);
    Transform GetFormationTransform(int formationIndex);
}
// The DuckLeader communicates with Ducks through this interface

public class LeadDuck : LinearBird, IDuckToLeader
{
    // The DuckLeader ensures all ducks follow as closely behind in an evenly distributed Flying V Formation
    // The DuckLeader will fly linearly across the screen
    protected override BirdType MyBirdType => BirdType.DuckLeader;
    protected override float MoveSpeed => 2.5f;
    
    [SerializeField] private Duck[] _duckScripts;
    [SerializeField] private Transform[] _formationTransforms;
    
    private List<ILeaderToDuck> _ducks;

    protected override void Start()
    {
        base.Start();
        _ducks = new List<ILeaderToDuck>(_duckScripts);
        var goLeft = transform.position.x > 0;
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
        var sign = goLeft ? 1 : -1;
        const float separationDistance = 0.15f;
        var cosmeticOffset = new Vector2(0f, -0.04f);

        for (var i = 0; i < _formationTransforms.Length; i++)
        {
            var indexMultiplier = Mathf.Floor(f: (float) i / 2) + 1;
            var normalizedLocalPos = i % 2 != 0 ? topSide : bottomSide;
            var targetLocalPos = sign * separationDistance * indexMultiplier * normalizedLocalPos;
            _formationTransforms[i].localPosition = targetLocalPos + cosmeticOffset;
            _duckScripts[i].transform.SetParent(_formationTransforms[i]);
            _duckScripts[i].transform.localPosition = Vector2.zero;
            _ducks[i].FormationIndex = i;
        }
    }

    #region IDuckToLeader

    Transform IDuckToLeader.GetFormationTransform(int formationIndex)
    {
        if (_formationTransforms != null && _formationTransforms.Length > formationIndex)
        {
            return _formationTransforms[formationIndex];
        }

        return null;
    }

    // Organizes the ducks into a "FlyingV" pattern
    void IDuckToLeader.OrganizeDucks(ILeaderToDuck deadDuck)
    {
        _ducks.Remove(deadDuck);
        var deadNumber = deadDuck.FormationIndex;
        bool isDeadOnTop = deadNumber % 2 == 0;
        var topCount = 0;
        var bottomCount = 0;

        for (var i = 0; i < _ducks.Count; i++)
        {
            var formIndex = _ducks[i].FormationIndex;
            bool isTop = formIndex % 2 == 0;
            topCount += isTop ? 1 : 0;
            bottomCount += !isTop ? 1 : 0;

            //slide ducks which are further back on the same side up two slots
            if (formIndex > deadNumber && isTop == isDeadOnTop)
            {
                _ducks[i].FormationIndex -= 2;
            }
        }

        //always retain a max difference of 1 per side.
        //shift ducks from side excess to the deficient side
        if (bottomCount > topCount + 1 && isDeadOnTop)
        {
            var highestOdd = bottomCount * 2 - 1;
            _ducks.Find(duck => duck.FormationIndex == highestOdd).FormationIndex -= 3;
        }
        else if (topCount > bottomCount + 1 && !isDeadOnTop)
        {
            var highestEven = (topCount - 1) * 2;
            _ducks.Find(duck => duck.FormationIndex == highestEven).FormationIndex -= 1;
        }
    }

    #endregion

    protected override void OnDeath()
    {
        BreakTheV();
        base.OnDeath();
    }

    private void BreakTheV()
    {
        transform.DetachChildren();
        for (var i = 0; i < _formationTransforms.Length; i++)
        {
            _formationTransforms[i].DetachChildren();
            Destroy(_formationTransforms[i].gameObject);
        }
        _ducks.ForEach(duck => duck.Scatter());
    }
}