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
    void RaiseThreat(Threat MyThreat);
    void BirdThreat(ref BirdStats birdStats, BirdThreat MyThreat);
}

public class EmotionalIntensity : MonoBehaviour, IThreat{

    public static IThreat ThreatTracker;
    int level;
    #region ThreateningBirds
    static BirdType[] threateningBirds = new BirdType[]{
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
    public static BirdType[] ThreateningBirds { get { return threateningBirds; } }
    #endregion
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        intensity = 0;
        timeIntensity = new AnimationCurve();
        editorIntensity = timeIntensity;
        this.level = level;
    }

    void Awake() {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        ThreatTracker = this;
        editorIntensity = timeIntensity;
        Decay();
    }
    static float intensity;
    public static float Intensity { get { return intensity; }
        private set {
            intensity = Mathf.Clamp(value, 0, 1000);
        }
    }


    static AnimationCurve timeIntensity = new AnimationCurve();
    public AnimationCurve editorIntensity;
    public static AnimationCurve TimeIntensity { get { return timeIntensity; } }

    void Update() {
        if (SceneManager.GetActiveScene().name != Scenes.Menu) {
            timeIntensity.AddKey(Time.time, Intensity);
        }
    }

    void IThreat.BirdThreat(ref BirdStats birdStats, BirdThreat MyThreat) {
        int threat=0;
        switch (MyThreat) {
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

    void IThreat.RaiseThreat(Threat MyThreat){
        Intensity += (int)MyThreat;
    }

	float repeatTime;
	void Decay(){
		bool decay = Intensity > 0 && ScoreSheet.Reporter.GetCount(CounterType.Alive, true, BirdType.All)<5;
		if (decay){
            Intensity -= 3;
			repeatTime=1f;
		}
		else{
			repeatTime =3f;
		}
		Invoke ("Decay",repeatTime);
	}
}
