using UnityEngine;
using System.Collections;
using GenericFunctions;

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

	public int balloonCount;
	public int poppedBalloon;
	public int numToFill;

	public bool[] popped;
	public bool popping;

	// Use this for initialization
	void Awake () {
		dropForce = 100f;
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
			StartCoroutine(PopBalloon(balloonNumber));
			StartCoroutine(Invincibility());
			StartCoroutine (SlowTime(.5f,.75f));
			StartCoroutine(screenShakeScript.CameraShake());
		}
		yield return null;
	}

	public IEnumerator PopBalloon(int i){
		popNoise.Play ();
		balloonAnimators[i].SetInteger("AnimState",1);
		rigbod.AddForce (Vector2.down * dropForce);
		Destroy (balloons[i].transform.GetChild(0).gameObject);
		popped [i] = true;
		balloonCount--;
		Destroy (balloons[i],Constants.time2Destroy);
		yield return null;
	}


	public IEnumerator Invincibility(){
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
		popping = false;
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

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.layer == 18){
			StartCoroutine (CollectNewBalloon(col.gameObject));
		}
	}

	public IEnumerator SpawnNewBalloon(){
		GameObject newBal = Instantiate (Resources.Load (balloonPrefabNames[Random.Range (0,3)]),new Vector3 (Random.Range(-6f,6f),-8f,0f),Quaternion.identity) as GameObject;
		yield return null;
	}

	public IEnumerator CollectNewBalloon(GameObject newBalloon){
		int balloonNumber = 0;
		bool collect = false;
		for (int i =0;i<popped.Length;i++){
			if (popped[i]){
				balloonNumber = i;
				collect = true;
				break;
			}
		}
		
		if (collect){
			balloons[balloonNumber] = newBalloon;
			balloonColliders[balloonNumber] = balloons[balloonNumber].GetComponent<CircleCollider2D>();
			balloonAnimators[balloonNumber] = balloons[balloonNumber].GetComponent<Animator>();
			balloonSprites[balloonNumber] = balloons[balloonNumber].GetComponent<SpriteRenderer>();
			balloonScripts[balloonNumber] = balloons[balloonNumber].GetComponent<Balloon>();
			balloonScripts[balloonNumber].moving = false;
			balloonScripts[balloonNumber].balloonNumber = balloonNumber;

			balloons[balloonNumber].transform.SetParent(transform);
			balloons[balloonNumber].transform.localScale = Vector3.one;
			balloons[balloonNumber].transform.position = jaiTransform.position + relativeBalloonPositions[balloonNumber];
			popped[balloonNumber] = false;
			balloonCount++;
			balloons[balloonNumber].layer = 17;
		}
		yield return null;
	}

	public IEnumerator EndGame(){
		yield return new WaitForSeconds (1.5f);
		UnityEditor.EditorApplication.isPlaying = false;
	}
}
