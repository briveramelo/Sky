using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Pauser : MonoBehaviour, IBegin {

	float lastTimeScale;
	bool paused = false;
	public static readonly Vector2 pauseSpot = new Vector2 (Constants.WorldDimensions.x * (4f/5f),Constants.WorldDimensions.y * (4f/5f));
	public static readonly float pauseRadius = 0.5f;
	[SerializeField] AudioSource pause;
	[SerializeField] AudioSource unPause;

	void Awake(){
		transform.position = pauseSpot;
	}

	void IBegin.OnTouchBegin(int fingerID){
		float distFromStick = Vector2.Distance(InputManager.touchSpot,pauseSpot);
		if (distFromStick < pauseRadius){
			paused = !paused;
			if (paused){
				lastTimeScale = Time.timeScale;
				pause.Play();
			}
			else{
				unPause.Play();
			}
			Time.timeScale = paused ? 0f : lastTimeScale;
		}
	}
}
