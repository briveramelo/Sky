using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine.SceneManagement;

#region Interfaces

public interface ITallyable
{
    void TallyBirdCount(ref BirdStats birdStats, BirdCounterType type, int amount);
    void TallyScoreCount(ScoreCounterType type, int amount);
    void TallyPoints(ref BirdStats birdStats);
    void TallyThreat(int threatLevel);
    void TallyBirdThreat(ref BirdStats birdStats, BirdThreat myThreat);
    void TallyBalloonPoints(Vector2 balloonPosition);
}

public interface IResetable
{
    void ResetWaveCounters();
}

public interface IReportable
{
    int GetCount(BirdCounterType birdCounter, bool currentWave, BirdType birdType);
    int GetCounts(BirdCounterType birdCounter, bool currentWave, params BirdType[] birdTypes);
    int GetScore(ScoreCounterType scoreType, bool currentWave);
    void ReportScores();
    IEnumerator DisplayTotal();
}

public interface IStreakable
{
    void ReportHit(int spearNumber);
    int GetHitStreak();
}

#endregion

public enum BirdCounterType
{
    BirdsSpawned = 0,
    BirdsAlive = 1,
    BirdsKilled = 2,
}

public enum ScoreCounterType
{
    ScoreTotal = 3,
    ScoreStreak = 4,
    ScoreCombo = 5,
    SpearsThrown = 6,
}

public class ScoreSheet : Singleton<ScoreSheet>, ITallyable, IResetable, IReportable, IStreakable
{
    #region Variables
    public static ITallyable Tallier { get; private set; }
    public static IResetable Resetter { get; private set; }
    public static IReportable Reporter { get; private set; }
    public static IStreakable Streaker { get; private set; }

    [SerializeField] private Canvas _parentCanvas;
    [SerializeField] private GameObject _points;
    [SerializeField] private ScoreBoard _scoreBoard;

    protected override bool _destroyOnLoad => true;
    
    private Dictionary<BirdCounterType, Dictionary<BirdType, ICount>> _birdCounters;
    private Dictionary<ScoreCounterType, ICount> _scoreCounters;
    
    private float _startTime;
    private int _hitStreak;
    private int _tempStreak;
    private int _lastHitWeaponNumber;
    #endregion

    #region Setup
    protected override void Awake()
    {
        base.Awake();

        Tallier = this;
        Resetter = this;
        Reporter = this;
        Streaker = this;

        InitializeCounters();

        _startTime = Time.time;
    }

    private void InitializeCounters()
    {
        _birdCounters = new Dictionary<BirdCounterType, Dictionary<BirdType, ICount>>
        {
            {BirdCounterType.BirdsAlive, Enum.GetValues(typeof(BirdType)).Cast<BirdType>().ToDictionary(counter => counter, counter => (ICount) new BirdCounter(BirdCounterType.BirdsAlive))},
            {BirdCounterType.BirdsKilled, Enum.GetValues(typeof(BirdType)).Cast<BirdType>().ToDictionary(counter => counter, counter => (ICount) new BirdCounter(BirdCounterType.BirdsKilled))},
            {BirdCounterType.BirdsSpawned, Enum.GetValues(typeof(BirdType)).Cast<BirdType>().ToDictionary(counter => counter, counter => (ICount) new BirdCounter(BirdCounterType.BirdsSpawned))},
        };

        _scoreCounters = new Dictionary<ScoreCounterType, ICount>
        {
            {ScoreCounterType.ScoreCombo, new ScoreCounter(ScoreCounterType.ScoreCombo)},
            {ScoreCounterType.ScoreStreak, new ScoreCounter(ScoreCounterType.ScoreStreak)},
            {ScoreCounterType.ScoreTotal, new ScoreCounter(ScoreCounterType.ScoreTotal)},
            {ScoreCounterType.SpearsThrown, new ScoreCounter(ScoreCounterType.SpearsThrown)},
        };
    }
    #endregion
    

    #region IStreakable
    private void ResetHitStreak()
    {
        _hitStreak = 0;
        _tempStreak = 0;
        _lastHitWeaponNumber = 0;
    }

    void IStreakable.ReportHit(int newHitWeaponNumber)
    {
        var weaponNumberDif = newHitWeaponNumber - _lastHitWeaponNumber;

        if (weaponNumberDif == 0 || weaponNumberDif == 1)
        {
            //continue the streeeeeaaaakkkk!
            _hitStreak++;
            if (weaponNumberDif == 1)
            {
                _tempStreak = 1;
            }
        }
        else if (weaponNumberDif > 1)
        {
            //c-c-c-combo breaker
            _tempStreak = _hitStreak;
            _hitStreak = 1;
        }
        else if (weaponNumberDif < 0)
        {
            //Combo RESTORATION!
            _hitStreak = _tempStreak + 1;
            //rectify points
        }

        _lastHitWeaponNumber = newHitWeaponNumber;
    }

    int IStreakable.GetHitStreak()
    {
        return _hitStreak;
    }
    #endregion

    #region IResetable
    void IResetable.ResetWaveCounters()
    {
        foreach (var birdCounter in _birdCounters)
        {
            var birdTypesCounters = birdCounter.Value; 
            foreach (var birdTypeCounter in birdTypesCounters)
            {
                birdTypeCounter.Value.ResetCurrent();
            }
        }

        foreach (var scoreCounter in _scoreCounters)
        {
            scoreCounter.Value.ResetCurrent();
        }
    }
    #endregion

    #region IReportable
    int IReportable.GetCount(BirdCounterType birdCounter, bool isCurrentWave, BirdType birdType)
    {
        return _birdCounters[birdCounter][birdType].GetCount(isCurrentWave);
    }

    int IReportable.GetCounts(BirdCounterType birdCounter, bool isCurrentWave, params BirdType[] birdTypes)
    {
        var total = 0;
        foreach (var type in birdTypes)
        {
            total += _birdCounters[birdCounter][type].GetCount(isCurrentWave);
        }

        return total;
    }

    int IReportable.GetScore(ScoreCounterType scoreType, bool isCurrentWave)
    {
        return _scoreCounters[scoreType].GetCount(isCurrentWave);
    }

    void IReportable.ReportScores()
    {
        if (WaveManager.CurrentWave == WaveName.Endless)
        {
            var duration = Time.time - _startTime;
            var myEndlessScore = new EndlessScore(_scoreCounters[ScoreCounterType.ScoreTotal].GetCount(false), duration);
            FindObjectOfType<SaveLoadData>().PromptSave(myEndlessScore);
        }
        else
        {
            var myStoryScore = new StoryScore(_scoreCounters[ScoreCounterType.ScoreTotal].GetCount(false), WaveManager.CurrentWave);
            FindObjectOfType<SaveLoadData>().PromptSave(myStoryScore);
        }
    }

    IEnumerator IReportable.DisplayTotal()
    {
        yield return StartCoroutine(FindObjectOfType<WaveUi>().DisplayPoints(false));
    }
    #endregion

    #region ITallyable
    void ITallyable.TallyBirdCount(ref BirdStats birdStats, BirdCounterType type, int amount)
    {
        _birdCounters[type][birdStats.MyBirdType].Add(amount);
    }

    void ITallyable.TallyScoreCount(ScoreCounterType type, int amount)
    {
        _scoreCounters[type].Add(amount);
    }

    void ITallyable.TallyPoints(ref BirdStats birdStats)
    {
        _scoreCounters[ScoreCounterType.ScoreTotal].Add(birdStats.PointsToAdd);
        _scoreCounters[ScoreCounterType.ScoreStreak].Add(birdStats.StreakPoints);
        _scoreCounters[ScoreCounterType.ScoreCombo].Add(birdStats.ComboPoints);

        DisplayPoints(birdStats.BirdPosition, birdStats.PointsToAdd);
    }

    void ITallyable.TallyBalloonPoints(Vector2 balloonPosition)
    {
        const int balloonPoints = 1000;
        _scoreCounters[ScoreCounterType.ScoreTotal].Add(balloonPoints);
        
        DisplayPoints(balloonPosition, balloonPoints);
    }

    private void DisplayPoints(Vector2 worldPosition, int pointsToAdd)
    {
        var worldSize = ScreenSpace.WorldEdge;
        var xClamp = worldSize.x - 0.1f;
        var yClamp = worldSize.y - 0.1f;
        var worldPositionClamped = new Vector2(Mathf.Clamp(worldPosition.x, -xClamp, xClamp), Mathf.Clamp(worldPosition.y, -yClamp, yClamp));
        var spawnPosition = worldPositionClamped.WorldPositionToCanvasPosition(_parentCanvas);

        var pointsInstance = Instantiate(_points, transform);
        pointsInstance.transform.position = spawnPosition;
        pointsInstance.GetComponent<IPointsDisplayable>().DisplayPoints(pointsToAdd);
        ((IPointsDisplayable) _scoreBoard).DisplayPoints(_scoreCounters[ScoreCounterType.ScoreTotal].GetCount(false));
    }

    void ITallyable.TallyThreat(int threatLevel)
    {
        EmotionalIntensity.ThreatTracker.RaiseThreat(threatLevel);
    }

    void ITallyable.TallyBirdThreat(ref BirdStats birdStats, BirdThreat myThreat)
    {
        if (EmotionalIntensity.ThreateningBirds.Contains(birdStats.MyBirdType))
        {
            EmotionalIntensity.ThreatTracker.BirdThreat(ref birdStats, myThreat);
        }
    }
    #endregion
    
    
    #region Counters
    private interface ICount
    {
        void Add(int amount);
        void Reset();
        void ResetCurrent();
        int GetCount(bool isCurrentWave);
    }

    private abstract class Counter<T> : ICount where T : Enum
    {
        private T _counterType;
        private int _currentCount;
        private int _cummulativeCount;

        protected Counter(T counterType)
        {
            _counterType = counterType;
        }

        public void Add(int amount)
        {
            _currentCount += amount;
            _cummulativeCount += amount;
        }

        public void Reset()
        {
            _currentCount = 0;
            _cummulativeCount = 0;
        }

        public void ResetCurrent()
        {
            _currentCount = 0;
        }

        public int GetCount(bool isCurrentWave) => isCurrentWave ? _currentCount : _cummulativeCount;
    }

    private class BirdCounter : Counter<BirdCounterType>
    {
        public BirdCounter(BirdCounterType counterType) : base(counterType)
        {
        }
    }

    private class ScoreCounter : Counter<ScoreCounterType>
    {
        public ScoreCounter(ScoreCounterType counterType) : base(counterType)
        {
        }
    }
    #endregion
}