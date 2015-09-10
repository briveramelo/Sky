using UnityEngine;
using System.Collections;

public class Basket : MonoBehaviour {
	
	public Balloon[] balloonScripts;
	public ScreenShake screenShakeScript;

	public CircleCollider2D[] balloonColliders;
	public GameObject[] balloons;
	public Animator[] balloonAnimators;
	public SpriteRenderer[] balloonSprites;

	public Rigidbody2D rigbod;
	public BoxCollider2D basketCollider;
	public Transform jaiTransform;

	public AudioSource popNoise; 

	public Vector3[] relativeBalloonPositions;

	public string[] balloonPrefabNames;

	public float[] distanceAway;
	public float dropForce;

	public int lowestRemainingBalloon;
	public int balloonCount;
	public int poppedBalloon;
	public int numToFill;

	public bool[] popped;
	public bool popping;

	// Use this for initialization
	void Awake () {
		dropForce = 100f;
		lowestRemainingBalloon = 0;
		balloonCount = 3;
		distanceAway = new float[3];
		screenShakeScript = GameObject.Find ("mainCam").GetComponent<ScreenShake> ();
		popNoise = GetComponent<AudioSource> ();
		rigbod = GetComponent<Rigidbody2D> ();
		jaiTransform = GameObject.Find ("Jai").transform;
		basketCollider = GameObject.Find ("Basket").GetComponent<BoxCollider2D> ();
		balloons = new GameObject[]{
			GameObject.Find("PinkBalloon"),
			GameObject.Find("TealBalloon"),
			GameObject.Find("GreyBalloon")
		};
		balloonPrefabNames = new string[]{
			"Prefabs/Gear/PinkBalloon",
			"Prefabs/Gear/TealBalloon",
			"Prefabs/Gear/GreyBalloon",
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

		relativeBalloonPositions = new Vector3[]{
			balloons[0].transform.position - jaiTransform.position,
			balloons[1].transform.position - jaiTransform.position,
			balloons[2].transform.position - jaiTransform.position
		};
		popped = new bool[]{
			false,
			false,
			false,
		}; 
	}

	public IEnumerator BeginPopping(int balloonNumber){
		if (!popping){
			popping = true;
			StartCoroutine(Invincibility());
			StartCoroutine (SlowTime(.5f,.75f));
			StartCoroutine(PopBalloon(balloonNumber));
			StartCoroutine(screenShakeScript.CameraShake());
		}
		yield return null;
	}
	
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
		popped [i] = true;
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

	public IEnumerator SpawnNewBalloon(){
		numToFill = -1;
		int q = 0;
		foreach (bool pop in popped){
			if (pop){
				numToFill = q;
				break;
			}
			q++;
		}
		if (numToFill!=-1){
			balloons[numToFill] = Instantiate (Resources.Load (balloonPrefabNames[numToFill]),relativeBalloonPositions[numToFill],Quaternion.identity) as GameObject;
			balloons[numToFill].transform.SetParent(transform);
			balloons[numToFill].transform.localScale = Vector3.one;
			balloons[numToFill].transform.position = jaiTransform.position + relativeBalloonPositions[numToFill];
			popped[numToFill] = false;
			balloonColliders[numToFill] = balloons[numToFill].GetComponent<CircleCollider2D>();
			balloonAnimators[numToFill] = balloons[numToFill].GetComponent<Animator>();
			balloonSprites[numToFill] = balloons[numToFill].GetComponent<SpriteRenderer>();
			balloonCount++;
		}
		yield return null;
	}

	public IEnumerator EndGame(){
		yield return new WaitForSeconds (1.5f);
		UnityEditor.EditorApplication.isPlaying = false;
	}
}
