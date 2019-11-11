using UnityEngine;
using UnityEngine.SceneManagement;

public static class Threat
{
    public const int BalloonPopped = 25;
    public const int BalloonGained = -25;
    public const int BatSurrounding = 2;
    public const int BatLeft = -2;

    public const int BasketGrabbed = 15;
    public const int BasketReleased = -15;
    public const int BasketBumped = 5;
    public const int BasketStabilized = -5;

    public const int Poop = 10;
    public const int PoopCleaned = -10;
    public const int FreeDuck = 2;
}

public enum BirdThreat
{
    Spawn = 0,
    Damage = 1,
    Leave = 2
}

public interface IThreat
{
    void RaiseThreat(int myThreat);
    void BirdThreat(ref BirdStats birdStats, BirdThreat myThreat);
}

public class EmotionalIntensity : MonoBehaviour, IThreat
{
    #region Member Variables

    public static IThreat ThreatTracker;
    public static BirdType[] ThreateningBirds => _threateningBirds;

    [SerializeField] private AnimationCurve _timeIntensity = new AnimationCurve();
    private static BirdType[] _threateningBirds =
    {
        BirdType.Pigeon,
        BirdType.Duck,
        BirdType.DuckLeader,
        BirdType.Albatross,
        //BirdType.BabyCrow,
        BirdType.Crow,
        //BirdType.Seagull,
        BirdType.Tentacles,
        BirdType.Pelican,
        BirdType.Shoebill,
        BirdType.Bat,
        BirdType.Eagle,
        BirdType.BirdOfParadise
    };

    private static float _intensity;
    private float _repeatTime;

    public static float Intensity
    {
        get => _intensity;
        private set => _intensity = Mathf.Clamp(value, 0, 1000);
    }

    #endregion
    private void Awake()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        ThreatTracker = this;
        Decay();
    }
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
    
    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        _intensity = 0;
        _timeIntensity = new AnimationCurve();
    }

#if UNITY_EDITOR //visualize intensity as an animation curve over time
    private void Update()
    {
        if (SceneManager.GetActiveScene().name != Scenes.Menu)
        {
            //_timeIntensity.AddKey(Time.time, Intensity);
        }
    }
#endif

    void IThreat.BirdThreat(ref BirdStats birdStats, BirdThreat myThreat)
    {
        var threat = 0;
        switch (myThreat)
        {
            case BirdThreat.Spawn:
                threat = birdStats.TotalThreatValue;
                break;
            case BirdThreat.Damage:
                threat = -birdStats.ThreatRemoved;
                break;
            case BirdThreat.Leave:
                threat = -birdStats.TotalThreatValue;
                break;
        }

        Intensity += threat;
    }

    void IThreat.RaiseThreat(int threatLevel)
    {
        Intensity += threatLevel;
    }

    private void Decay()
    {
        var decay = Intensity > 0 && ScoreSheet.Reporter.GetCount(CounterType.Alive, true, BirdType.All) < 5;
        if (decay)
        {
            Intensity -= 3;
            _repeatTime = 1f;
        }
        else
        {
            _repeatTime = 3f;
        }

        Invoke(nameof(Decay), _repeatTime);
    }
}