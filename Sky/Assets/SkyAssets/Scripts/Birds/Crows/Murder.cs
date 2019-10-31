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
    private List<IMurderToCrow> _crowsAlive, _crowsToSwoop;
    private List<Vector2> _availableCrowPositions;
    private ICrowToMurder _me;


    private Vector2[] _crowPositions;
    private int _maxCycles = 10;
    private int _cycle = 1;

    private void Awake()
    {
        _crowPositions = new[]
        {
            new Vector2(0f, Constants.WorldSize.y * 1.4f),
            new Vector2(Constants.WorldSize.x * 1.08f, Constants.WorldSize.y * 1.2f),
            new Vector2(Constants.WorldSize.x * 1.08f, -Constants.WorldSize.y * 1.2f),
            new Vector2(0f, -Constants.WorldSize.y * 1.4f),
            new Vector2(-Constants.WorldSize.x * 1.08f, -Constants.WorldSize.y * 1.2f),
            new Vector2(-Constants.WorldSize.x * 1.08f, Constants.WorldSize.y * 1.2f)
        };
        
        _crowsAlive = new List<IMurderToCrow>(_crows);
        _crowsToSwoop = new List<IMurderToCrow>(_crowsAlive);
        _availableCrowPositions = new List<Vector2>(_crowPositions);
        _availableCrowPositions.Shuffle();
        var i = 0;
        _crowsAlive.ForEach(crow =>
        {
            crow.InitializeCrow(i);
            i++;
        });

        _me = this;
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