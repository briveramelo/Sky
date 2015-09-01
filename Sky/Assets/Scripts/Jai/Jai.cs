using UnityEngine;
using System.Collections;

public class Jai : MonoBehaviour {

	public bool throwing;
	public bool stabbing;

	public int throws;
	public int stabs;

	public float throwForce;
	public float throwSpearTime;

	public string ballString;

	public Animator jaiAnimator;
	public GameObject ball;

	void Start(){
		throwing = false;
		throwForce = 1000f;
		ballString = "Prefabs/Equipment/Ball";
		throwSpearTime = 0.5f;
		jaiAnimator = GetComponent<Animator> ();
	}

	public IEnumerator ThrowSpear(Vector2 throwDir){
		if (!throwing && !stabbing){
			throwing = true;
			if (throwDir.x>0){
				transform.localScale = new Vector3 (-1,1,1);
			}
			else{
				transform.localScale = new Vector3 (1,1,1);
			}
			jaiAnimator.SetInteger("AnimState",1);
			yield return new WaitForSeconds (throwSpearTime);
			jaiAnimator.SetInteger("AnimState",0);

			ball = Instantiate (Resources.Load (ballString), transform.position, Quaternion.identity) as GameObject;
			ball.GetComponent<Rigidbody2D>().AddForce (throwDir * throwForce);
			throws++;
		}
		throwing = false;
		yield return null;
	}

	public IEnumerator StabSpear(Vector2 throwDir){
		if (!throwing && !stabbing) {
			stabbing = true;
			stabs++;
		}
		stabbing = false;
		yield return null;
	}

}
