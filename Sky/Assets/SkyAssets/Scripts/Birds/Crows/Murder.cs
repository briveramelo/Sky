using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GenericFunctions;

public interface ICrowToMurder
{
    void SendNextCrow();
    void ReportCrowDown(IMurderToCrow crowDown);
    int Cycle { get; }
}

public class Murder : MonoBehaviour, ICrowToMurder
{
    [SerializeField] private Crow[] _crows;
    
    private Vector2[] _crowPositions;
    private List<Vector2> _availableCrowPositions;
    private List<IMurderToCrow> _crowsAlive, _crowsToSwoop;
    private ICrowToMurder _me;
    
    private int _maxCycles = 10;
    private int _cycle = 1;

    private void Start()
    {
        _me = this;
        InitializeCrowPositions();
        InitializeCrows();
    }

    private void InitializeCrowPositions()
    {
        var xPos = ScreenSpace.WorldEdge.x + 1f;
        var yPos = ScreenSpace.WorldEdge.y + 1.6f;
        _crowPositions = new[]
        {
            new Vector2(0f, yPos),
            new Vector2(xPos, yPos),
            new Vector2(xPos, -yPos),
            new Vector2(0f, -yPos),
            new Vector2(-xPos, -yPos),
            new Vector2(-xPos, yPos)
        };
        
        _crowsAlive = new List<IMurderToCrow>(_crows);
        _crowsToSwoop = new List<IMurderToCrow>(_crowsAlive);
        _availableCrowPositions = new List<Vector2>(_crowPositions);
        _availableCrowPositions.Shuffle();
    }

    private void InitializeCrows()
    {
        var killerCrowIndex = Random.Range(0, _crowsAlive.Count);
        for (var i = 0; i < _crowsAlive.Count; i++)
        {
            var crow = _crowsAlive[i];
            crow.InitializeCrow(i == killerCrowIndex);
        }

        _me.SendNextCrow();
    }

    #region ICrowToMurder Interface

    void ICrowToMurder.SendNextCrow()
    {
        if (_crowsToSwoop.Count > 0)
        {
            var luckyCrowIndex = Random.Range(0, _crowsToSwoop.Count - 1);
            var luckyCrow = _crowsToSwoop[luckyCrowIndex];
            var availableCrowPos = _availableCrowPositions[luckyCrowIndex];
            luckyCrow.TakeFlight(availableCrowPos);
            
            _crowsToSwoop.Remove(luckyCrow);
            _availableCrowPositions.Remove(availableCrowPos);
        }
        else if (_crowsAlive.Count > 0)
        {
            StartCoroutine(ResetTheCycle());
        }
    }

    void ICrowToMurder.ReportCrowDown(IMurderToCrow crowDown)
    {
        _crowsAlive.Remove(crowDown);
        _crowsToSwoop.Remove(crowDown);
        if (_crowsAlive.Count == 0)
        {
            Destroy(gameObject);
        }
    }

    int ICrowToMurder.Cycle => _cycle;

    #endregion

    private IEnumerator ResetTheCycle()
    {
        while (!_crowsAlive.Any(crow => crow.ReadyToFly))
        {
            yield return null;
        }

        _crowsToSwoop = new List<IMurderToCrow>(_crowsAlive);
        _availableCrowPositions = new List<Vector2>(_crowPositions);
        _availableCrowPositions.Shuffle();
        yield return new WaitForSeconds(3f);
        _cycle++;
        if (_cycle >= _maxCycles)
        {
            Destroy(gameObject);
        }

        _me.SendNextCrow();
    }
}