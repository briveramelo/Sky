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
        _crowPositions = new[]
        {
            new Vector2(0f, Constants.ScreenSizeWorldUnits.y * 1.4f),
            new Vector2(Constants.ScreenSizeWorldUnits.x * 1.08f, Constants.ScreenSizeWorldUnits.y * 1.2f),
            new Vector2(Constants.ScreenSizeWorldUnits.x * 1.08f, -Constants.ScreenSizeWorldUnits.y * 1.2f),
            new Vector2(0f, -Constants.ScreenSizeWorldUnits.y * 1.4f),
            new Vector2(-Constants.ScreenSizeWorldUnits.x * 1.08f, -Constants.ScreenSizeWorldUnits.y * 1.2f),
            new Vector2(-Constants.ScreenSizeWorldUnits.x * 1.08f, Constants.ScreenSizeWorldUnits.y * 1.2f)
        };
        
        _crowsAlive = new List<IMurderToCrow>(_crows);
        _crowsToSwoop = new List<IMurderToCrow>(_crowsAlive);
        _availableCrowPositions = new List<Vector2>(_crowPositions);
        _availableCrowPositions.Shuffle();
    }

    private void InitializeCrows()
    {
        for (var i = 0; i < _crowsAlive.Count; i++)
        {
            var crow = _crowsAlive[i];
            crow.InitializeCrow(i==5);
        }

        _me.SendNextCrow();
    }

    #region ICrowToMurder Interface

    void ICrowToMurder.SendNextCrow()
    {
        if (_crowsToSwoop.Count > 0)
        {
            var luckyCrow = Random.Range(0, _crowsToSwoop.Count - 1);
            _crowsToSwoop[luckyCrow].TakeFlight(_availableCrowPositions[luckyCrow]);
            _crowsToSwoop.Remove(_crowsToSwoop[luckyCrow]);
            _availableCrowPositions.Remove(_availableCrowPositions[luckyCrow]);
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
            StopAllCoroutines();
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