using UnityEngine;
using UnityEngine.SceneManagement;

public enum Threat{
	BalloonPopped = 25,
    BalloonGained = -25,
	BatSurrounding =2,
    BatLeft = -2,

	BasketGrabbed = 15,
    BasketReleased = -15,
	BasketBumped =5,
    BasketStabilized = -5,

    Poop = 10,
    PoopCleaned = -10,
    FreeDuck = 2,
}

public enum BirdThreat {
    Spawn = 0,
    Damage = 1,
    Leave = 2
}

public interface IThreat {
    void RaiseThreat(Threat myThreat);
    void BirdThreat(ref BirdStats birdStats, BirdThreat myThreat);
}

public class EmotionalIntensity : MonoBehaviour, IThreat{

    #region Member Variables
    public static IThreat ThreatTracker;
    public static BirdType[] ThreateningBirds => _threateningBirds;

    private static BirdType[] _threateningBirds = {
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
    
    [SerializeField] private AnimationCurve _timeIntensity = new AnimationCurve();
    private static float _intensity;
    private float _repeatTime;

    public static float Intensity { 
        get => _intensity;
        private set => _intensity = Mathf.Clamp(value, 0, 1000);
    }
    #endregion
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        _intensity = 0;
        _timeIntensity = new AnimationCurve();
    }

    private void Awake() {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        ThreatTracker = this;
        Decay();
    }
    
    #if UNITY_EDITOR //visualize intensity as an animation curve over time
    private void Update() {
        if (SceneManager.GetActiveScene().name != Scenes.Menu) {
            _timeIntensity.AddKey(Time.time, Intensity);
        }
    }
    #endif

    void IThreat.BirdThreat(ref BirdStats birdStats, BirdThreat myThreat) {
        int threat=0;
        switch (myThreat) {
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

    void IThreat.RaiseThreat(Threat myThreat){
        Intensity += (int)myThreat;
    }
    
    private void Decay(){
		bool decay = Intensity > 0 && ScoreSheet.Reporter.GetCount(CounterType.Alive, true, BirdType.All)<5;
		if (decay){
            Intensity -= 3;
			_repeatTime=1f;
		}
		else{
			_repeatTime =3f;
		}
		Invoke ("Decay",_repeatTime);
	}
}
