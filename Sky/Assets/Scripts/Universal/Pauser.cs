using UnityEngine;
using GenericFunctions;

public class Pauser : MonoBehaviour, IBegin {

	float lastTimeScale;
	static bool paused = false; public static bool Paused { get { return paused; } }
	public static readonly Vector2 pauseSpot = new Vector2 (Constants.WorldDimensions.x * (4f/5f),Constants.WorldDimensions.y * (4f/5f));
	public static readonly float pauseRadius = 0.5f;
	[SerializeField] AudioSource pauseSounds;
	[SerializeField] AudioClip pause;
    [SerializeField] AudioClip unPause;

	void Awake(){
		transform.position = pauseSpot;
	}

	void IBegin.OnTouchBegin(int fingerID){
		float distFromStick = Vector2.Distance(InputManager.touchSpot,pauseSpot);
		if (distFromStick < pauseRadius){
			paused = !paused;
			if (paused){
				lastTimeScale = Time.timeScale;
				pauseSounds.PlayOneShot(pause);
			}
			else{
				pauseSounds.PlayOneShot(unPause);
			}
			Time.timeScale = paused ? 0f : lastTimeScale;
		}
	}
}
