using UnityEngine;
using System.Collections;

public class BalloonBasket : MonoBehaviour {
	
	public float maxBalloonSpeed;
	public Rigidbody2D rigbod;
	public BoxCollider2D basketCollider;

	public ScreenShake screenShakeScript;
	public SpriteRenderer[] balloonSprites;
	public int balloonCount;
	public float[] distanceAway;
	public int poppedBalloon;
	public bool[] popped;
	public int lowestRemainingBalloon;
	public CircleCollider2D[] balloonColliders;
	public GameObject[] balloons;
	public Balloon[] balloonScripts;
	public Animator[] balloonAnimators;
	public AudioSource popNoise; 
	public bool popping;
	public float dropForce;
	
	// Use this for initialization
	void Awake () {
		dropForce = 100f;
		maxBalloonSpeed = 2f;
		lowestRemainingBalloon = 0;
		balloonCount = 3;
		distanceAway = new float[3];
		screenShakeScript = GameObject.Find ("mainCam").GetComponent<ScreenShake> ();
		popNoise = GetComponent<AudioSource> ();
		rigbod = GetComponent<Rigidbody2D> ();
		basketCollider = GameObject.Find ("Basket").GetComponent<BoxCollider2D> ();
		balloons = new GameObject[]{
			GameObject.Find("PinkBalloon"),
			GameObject.Find("TealBalloon"),
			GameObject.Find("GreyBalloon")
		};
		
		balloonSprites = new SpriteRenderer[]{
			balloons[0].GetComponent<SpriteRenderer>(),
			balloons[1].GetComponent<SpriteRenderer>(),
			balloons[2].GetComponent<SpriteRenderer>()
		};

		balloonScripts = new Balloon[]{
			balloons[0].GetComponent<Balloon>(),
			balloons[1].GetComponent<Balloon>(),
			balloons[2].GetComponent<Balloon>()
		};
		
		balloonColliders = new CircleCollider2D[]{
			balloons[0].GetComponent<CircleCollider2D>(),
			balloons[1].GetComponent<CircleCollider2D>(),
			balloons[2].GetComponent<CircleCollider2D>()
		};
		
		balloonAnimators = new Animator[]{
			balloons [0].GetComponent<Animator> (),
			balloons [1].GetComponent<Animator> (),
			balloons [2].GetComponent<Animator> ()
		};
		popped = new bool[]{
			false,
			false,
			false,
		}; 
		//Physics2D.IgnoreLayerCollision (14, 13); //ignore basket and spear collision
		//Physics2D.IgnoreLayerCollision (16, 16); //ignore birds hitting birds
	}

	public IEnumerator BeginPopping(int balloonNumber){
		if (!popping){
			popping = true;
			/*int j= 0;
			foreach (GameObject balloon in balloons){
				if (balloon){
					distanceAway[j] = Vector3.Distance(balloons[j].transform.position,col.transform.position);
				}
				else{
					distanceAway[j] = 10f;
				}
				j++;
			}*/
			
			
			StartCoroutine(Invincibility());
			StartCoroutine (SlowTime(.5f,.75f));
			StartCoroutine(PopBalloon(balloonNumber));
			StartCoroutine(screenShakeScript.CameraShake());
		}
		yield return null;
	}
	
	/*public int LowestIndex(float[] distances){
		if (popped[0]){
			lowestRemainingBalloon = 1;
			if (popped[1]){
				lowestRemainingBalloon = 2;
			}
		}
		
		float lowestVal = distances[lowestRemainingBalloon];
		int lowestIndex = 0;
		int index = 0;
		foreach (float distance in distances) {
			if (distance <= lowestVal && !popped[index]) {
				lowestVal = distance;
				lowestIndex = index;
			}
			index++;
		}
		popped [lowestIndex] = true;
		
		
		return lowestIndex;
	}*/
	
	public IEnumerator Invincibility(){
		balloonCount--;
		if (balloonCount<1){
			rigbod.gravityScale = 1;
			foreach (CircleCollider2D cirCol in balloonColliders){
				if (cirCol){
					cirCol.enabled = false;
				}
			}
			basketCollider.enabled = false;
			StartCoroutine (EndGame());
		}
		Physics2D.IgnoreLayerCollision (16, 17, true);//ignore birds and balloons for a second;
		yield return new WaitForSeconds(1.5f);
		Physics2D.IgnoreLayerCollision (16, 17, false); //resume
		foreach (CircleCollider2D cirCol in balloonColliders){
			if (cirCol){
				cirCol.enabled = true;
			}
		}
	}
	
	public IEnumerator PopBalloon(int i){
		popping = true;
		popNoise.Play ();
		balloonAnimators[i].SetInteger("AnimState",1);
		rigbod.AddForce (Vector2.down * dropForce);
		Destroy (balloons[i].transform.GetChild(0).gameObject);
		while (popNoise.isPlaying){
			yield return null;
		}
		Destroy (balloons[i]);
		popping = false;
		foreach (Balloon balloonScript in balloonScripts){
			if (balloonScript){
				balloonScript.popping = false;
			}
		}
		yield return null;
	}
	
	public IEnumerator SlowTime(float slowDuration, float timeScale){
		StartCoroutine (WaitForRealSeconds (slowDuration));
		Time.timeScale = timeScale;
		yield return null;
	}
	
	public IEnumerator WaitForRealSeconds(float slowDuration){
		float startTime = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup - startTime < slowDuration){
			yield return null;
		}
		Time.timeScale = 1f;
		yield return null;
	}
	
	public IEnumerator EndGame(){
		yield return new WaitForSeconds (1.5f);
		UnityEditor.EditorApplication.isPlaying = false;
	}
}
