using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Basket : MonoBehaviour {
	
	public Balloon[] balloonScripts;
	public ScreenShake screenShakeScript;
	public WorldEffects worldEffectsScript;
	public Joyfulstick joyfulstickScript;

	public CircleCollider2D[] balloonColliders;
	public GameObject[] balloons;
	public Animator[] balloonAnimators;
	public SpriteRenderer[] balloonSprites;

	public Rigidbody2D rigbod;
	public BoxCollider2D[] basketColliders;
	public Transform jaiTransform;

	public AudioSource popNoise; 

	public Vector3[] relativeBalloonPositions;
	
	public float dropForce;

	public int balloonCount;
	public int poppedBalloon;
	public int numToFill;
	public int x;

	public bool[] popped;
	public bool popping;

	// Use this for initialization
	void Awake () {
		dropForce = 100f;
		balloonCount = 3;
		screenShakeScript = GameObject.Find ("mainCam").GetComponent<ScreenShake> ();
		worldEffectsScript = GameObject.Find ("WorldBounds").GetComponent<WorldEffects> ();
		joyfulstickScript = GameObject.Find ("StickHole").GetComponent<Joyfulstick> ();
		popNoise = GetComponent<AudioSource> ();
		rigbod = GetComponent<Rigidbody2D> ();
		jaiTransform = GameObject.Find ("Jai").transform;
		basketColliders = GameObject.Find ("Basket").GetComponents<BoxCollider2D> ();
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
			StartCoroutine (PopBalloon(balloonNumber));
			StartCoroutine (Invincibility());
			StartCoroutine (CheckForEndTimes());
			StartCoroutine (worldEffectsScript.SlowTime(.5f,.75f));
			StartCoroutine (screenShakeScript.CameraShake());
		}
		yield return null;
	}

	public IEnumerator PopBalloon(int i){
		Handheld.Vibrate ();
		popNoise.Play ();
		balloonAnimators[i].SetInteger("AnimState",1);
		if (!joyfulstickScript.beingHeld){
			rigbod.AddForce (Vector2.down * dropForce);
		}
		Destroy (balloons[i].transform.GetChild(0).gameObject);
		popped [i] = true;
		balloonCount--;
		balloonAnimators [i] = null;
		balloonColliders [i] = null;
		balloonScripts [i] = null;
		balloonSprites [i] = null;
		Destroy (balloons[i],Constants.time2Destroy);
		yield return null;
	}
	
	public IEnumerator Invincibility(){
		int qf = 0;
		foreach (Balloon balloonScript in balloonScripts){
			if (!popped[qf]){
				balloonScript.balloonCollider.enabled = false;
				balloonScript.popping = true;
			}
			qf++;
		}
		Physics2D.IgnoreLayerCollision (Constants.birdLayer, Constants.balloonLayer, true);//ignore birds and balloons for a second;
		yield return new WaitForSeconds(1.5f);
		Physics2D.IgnoreLayerCollision (Constants.birdLayer, Constants.balloonLayer, false); //resume
		foreach (Balloon balloonScript in balloonScripts){
			if (balloonScript){
				balloonScript.balloonCollider.enabled = true;
				balloonScript.popping = false;
			}
		}
		popping = false;
	}

	public IEnumerator CheckForEndTimes(){
		if (balloonCount<1){
			rigbod.gravityScale = 1;
			basketColliders[0].enabled = false;
			basketColliders[1].enabled = false;
			yield return new WaitForSeconds (2.5f);
			foreach (Rigidbody2D rigger in FindObjectsOfType<Rigidbody2D>()){
				rigger.isKinematic = true;
			}
			StartCoroutine (SaveLoadData.dataStorage.PromptSave ());
//			while (SaveLoadData.dataStorage.prompting){
//				yield return null;
//			}
			yield return new WaitForSeconds (1f);
			UnityEditor.EditorApplication.isPlaying = false;
		}
		yield return null;
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.layer == Constants.balloonFloatingLayer){
			StartCoroutine (CollectNewBalloon(col.gameObject));
		}
	}

	public IEnumerator CollectNewBalloon(GameObject newBalloon){
		int balloonNumber = 0;
		bool collect = false;
		for (int fun =0;fun<popped.Length;fun++){
			if (popped[fun]){
				balloonNumber = fun;
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
			balloons[balloonNumber].transform.Face4ward(true);
			balloons[balloonNumber].transform.position = jaiTransform.position + relativeBalloonPositions[balloonNumber];
			popped[balloonNumber] = false;
			balloonCount++;
			balloons[balloonNumber].layer = Constants.balloonLayer;
		}
		yield return null;
	}


}
