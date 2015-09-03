using UnityEngine;
using System.Collections;

public class Jai : MonoBehaviour {
	
	public int throws; //counts number of throws he's done
	public int animInt;

	public float throwForce; //Force with which Jai throws the spear
	public float throwSpearTime; //time between initiating the throw and when the spear leaves his hand


	public Vector3 fixVector;
	public string ballString;

	public Animator jaiAnimator;

	public Spear spearScript;
	public GameObject ball;

	void Awake(){
		throwForce = 1000f;
		ballString = "Prefabs/Gear/Ball";
		throwSpearTime = 0.5f;
		fixVector = new Vector3 (0f, .02f);
		jaiAnimator = GetComponent<Animator> ();
	}

	public IEnumerator ThrowSpear(Vector2 throwDir){
		if (!spearScript.flying){
			StartCoroutine (spearScript.FlyFree(throwDir, throwForce));
			if (throwDir.x>0){
				transform.localScale = new Vector3 (-1,1,1);
			}
			else{
				transform.localScale = new Vector3 (1,1,1);
			}

			if (throwDir.y>.2f){
				animInt = 2; //highThrow
			}
			else{
				animInt = 1;// low throw
			}
			jaiAnimator.SetInteger("AnimState",animInt);
			yield return new WaitForSeconds (throwSpearTime);
			jaiAnimator.SetInteger("AnimState",0);
			transform.position += fixVector;
			//ball = Instantiate (Resources.Load (ballString), transform.position, Quaternion.identity) as GameObject;
			//ball.GetComponent<Rigidbody2D>().AddForce (throwDir * throwForce);
			throws++;
		}
		yield return null;
	}

}
