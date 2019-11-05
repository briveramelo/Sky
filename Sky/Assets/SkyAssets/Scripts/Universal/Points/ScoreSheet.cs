using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using GenericFunctions;
using UnityEngine.SceneManagement;

#region Interfaces

public interface ITallyable
{
    void TallyBirth(ref BirdStats birdStats);
    void TallyDeath(ref BirdStats birdStats);
    void TallyKill(ref BirdStats birdStats);
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
    int GetCount(CounterType counter, bool currentWave, BirdType birdType);
    int GetCounts(CounterType counter, bool currentWave, params BirdType[] birdTypes);
    int GetScore(ScoreType scoreType, bool currentWave, BirdType birdType);
    void ReportScores();
    IEnumerator DisplayTotal();
}

public interface IStreakable
{
    void ReportHit(int spearNumber);
    int GetHitStreak();
}

#endregion

public enum CounterType
{
    Spawned = 0,
    Alive = 1,
    Scored = 2,
    Killed = 3,
}

public enum ScoreType
{
    Total = 0,
    Streak = 1,
    Combo = 2
}

public class ScoreSheet : Singleton<ScoreSheet>, ITallyable, IResetable, IReportable, IStreakable
{
    #region BirdCounters

    private class Counter
    {
        protected CounterType CounterType;
        protected Dictionary<BirdType, int> CurrentCount;
        protected Dictionary<BirdType, int> CummulativeCount;

        public Counter(CounterType counterType)
        {
            CounterType = counterType;
            CurrentCount = GetEmptyDictionary();
            CummulativeCount = GetEmptyDictionary();
        }

        private Dictionary<BirdType, int> GetEmptyDictionary()
        {
            return new Dictionary<BirdType, int>
            {
                {BirdType.Albatross, 0},
                {BirdType.Bat, 0},
                {BirdType.Crow, 0},
                {BirdType.Duck, 0},
                {BirdType.Eagle, 0},
                {BirdType.Pelican, 0},
                {BirdType.Pigeon, 0},
                {BirdType.Seagull, 0},
                {BirdType.Shoebill, 0},
                {BirdType.Tentacles, 0},
                {BirdType.BabyCrow, 0},
                {BirdType.DuckLeader, 0},
                {BirdType.BirdOfParadise, 0},
                {BirdType.All, 0}
            };
        }

        public void SetCount(BirdType birdType, int change)
        {
            CurrentCount[birdType] += change;
            CurrentCount[BirdType.All] += change;
            CummulativeCount[birdType] += change;
            CummulativeCount[BirdType.All] += change;
        }

        public int GetCount(BirdType birdType, bool currentWave)
        {
            return currentWave ? CurrentCount[birdType] : CummulativeCount[birdType];
        }

        public void ResetCurrentCount()
        {
            CurrentCount = GetEmptyDictionary();
        }
    }

    private class BirdCounter : Counter
    {
        public BirdCounter(CounterType counterType) : base(counterType)
        {
        }

        public void SetCount(BirdType birdType, bool increase)
        {
            var change = increase ? 1 : -1;
            base.SetCount(birdType, change);
        }
    }

    private class PointCounter : Counter
    {
        public PointCounter(CounterType counterType) : base(counterType)
        {
        }
    }

    #endregion

    public static ITallyable Tallier { get; private set; }
    public static IResetable Resetter { get; private set; }
    public static IReportable Reporter { get; private set; }
    public static IStreakable Streaker { get; private set; }

    [SerializeField] private GameObject _points;
    [SerializeField] private ScoreBoard _scoreBoard;

    private static int _hitStreak;
    private static int _tempStreak;
    private static int _lastHitWeaponNumber;
    private static Dictionary<CounterType, Counter> _allCounters;
    private static Dictionary<ScoreType, PointCounter> _scoreCounters;

    private const bool _increase = true;
    private const bool _decrease = false;
    
    private float _startTime;

    protected override bool _destroyOnLoad => true;

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnLevelFinishedLoading;

        Tallier = this;
        Resetter = this;
        Reporter = this;
        Streaker = this;

        InitializeCounters();

        _startTime = Time.time;
    }

    private void InitializeCounters()
    {
        _allCounters = new Dictionary<CounterType, Counter>();
        for (var i = 0; i < Enum.GetNames(typeof(CounterType)).Length; i++)
        {
            if ((CounterType) i == CounterType.Scored)
            {
                _allCounters.Add((CounterType) i, new PointCounter(CounterType.Scored));
            }
            else
            {
                _allCounters.Add((CounterType) i, new BirdCounter((CounterType) i));
            }
        }

        _scoreCounters = new Dictionary<ScoreType, PointCounter>();
        for (var i = 0; i < Enum.GetNames(typeof(ScoreType)).Length; i++)
        {
            _scoreCounters.Add((ScoreType) i, new PointCounter(CounterType.Scored));
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == Scenes.Story || scene.name == Scenes.Endless)
        {
            ResetHitStreak();
        }
    }

    #region IStreakable

    private static void ResetHitStreak()
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
        for (var i = 0; i < _allCounters.Count; i++)
        {
            _allCounters[(CounterType) i].ResetCurrentCount();
        }

        for (var i = 0; i < _scoreCounters.Count; i++)
        {
            _scoreCounters[(ScoreType) i].ResetCurrentCount();
        }
    }

    #endregion

    #region IReportable

    int IReportable.GetCount(CounterType counter, bool currentWave, BirdType birdType)
    {
        return _allCounters[counter].GetCount(birdType, currentWave);
    }

    int IReportable.GetCounts(CounterType counter, bool currentWave, params BirdType[] birdTypes)
    {
        var total = 0;
        for (var i = 0; i < birdTypes.Length; i++)
        {
            total += _allCounters[counter].GetCount(birdTypes[i], currentWave);
        }

        return total;
    }

    int IReportable.GetScore(ScoreType scoreType, bool currentWave, BirdType birdType)
    {
        return _scoreCounters[scoreType].GetCount(birdType, currentWave);
    }

    void IReportable.ReportScores()
    {
        if (WaveManager.CurrentWave == WaveName.Endless)
        {
            var duration = Time.time - _startTime;
            var myEndlessScore = new EndlessScore(_scoreCounters[ScoreType.Total].GetCount(BirdType.All, false), duration);
            FindObjectOfType<SaveLoadData>().PromptSave(myEndlessScore);
        }
        else
        {
            var myStoryScore = new StoryScore(_scoreCounters[ScoreType.Total].GetCount(BirdType.All, false), WaveManager.CurrentWave);
            FindObjectOfType<SaveLoadData>().PromptSave(myStoryScore);
        }
    }

    IEnumerator IReportable.DisplayTotal()
    {
        yield return StartCoroutine(FindObjectOfType<WaveUi>().DisplayPoints(false));
    }

    #endregion

    #region ITallyable

    void ITallyable.TallyBirth(ref BirdStats birdStats)
    {
        ((BirdCounter) _allCounters[CounterType.Spawned]).SetCount(birdStats.MyBirdType, _increase);
        ((BirdCounter) _allCounters[CounterType.Alive]).SetCount(birdStats.MyBirdType, _increase);
    }

    void ITallyable.TallyDeath(ref BirdStats birdStats)
    {
        ((BirdCounter) _allCounters[CounterType.Alive]).SetCount(birdStats.MyBirdType, _decrease);
    }

    void ITallyable.TallyKill(ref BirdStats birdStats)
    {
        ((BirdCounter) _allCounters[CounterType.Killed]).SetCount(birdStats.MyBirdType, _increase);
    }

    void ITallyable.TallyPoints(ref BirdStats birdStats)
    {
        ((PointCounter) _allCounters[CounterType.Scored]).SetCount(birdStats.MyBirdType, birdStats.PointsToAdd);
        _scoreCounters[ScoreType.Total].SetCount(birdStats.MyBirdType, birdStats.PointsToAdd);
        _scoreCounters[ScoreType.Streak].SetCount(birdStats.MyBirdType, birdStats.StreakPoints);
        _scoreCounters[ScoreType.Combo].SetCount(birdStats.MyBirdType, birdStats.ComboPoints);

        DisplayPoints(birdStats.BirdPosition, birdStats.PointsToAdd);
    }

    void ITallyable.TallyBalloonPoints(Vector2 balloonPosition)
    {
        var balloonPoints = 1000;
        ((PointCounter) _allCounters[CounterType.Scored]).SetCount(BirdType.BabyCrow, balloonPoints);
        _scoreCounters[ScoreType.Total].SetCount(BirdType.BabyCrow, balloonPoints);

        DisplayPoints(balloonPosition, balloonPoints);
    }

    private void DisplayPoints(Vector2 position, int pointsToAdd)
    {
        var xClamp = ScreenSpace.ScreenSizePixels.x * .9f;
        var yClamp = ScreenSpace.ScreenSizePixels.y * .9f;
        var spawnPosition = new Vector2(Mathf.Clamp(position.x, -xClamp, xClamp), Mathf.Clamp(position.y, -yClamp, yClamp));

        Instantiate(_points, spawnPosition, Quaternion.identity, ScoreSheet.Instance.transform).GetComponent<IDisplayable>().DisplayPoints(pointsToAdd);
        ((IDisplayable) _scoreBoard).DisplayPoints(((PointCounter) _allCounters[CounterType.Scored]).GetCount(BirdType.All, false));
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
}