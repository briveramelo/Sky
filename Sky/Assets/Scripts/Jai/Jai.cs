using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Jai : MonoBehaviour {
	
	public Spear spearScript;

	public Animator jaiAnimator;

	public Vector3 fixVector;

	public float throwForce; //Force with which Jai throws the spear

	public int throws; //counts number of throws he's done
	public int animInt;
	public int highLow;

	public bool throwing;

	void Awake(){
		throwForce = 1750f;
		fixVector = new Vector3 (0f, .02f);
		jaiAnimator = GetComponent<Animator> ();
	}

	public IEnumerator ThrowSpear(Vector2 throwDir){
		if (!throwing){
			throwing = true;
			if (throwDir.y<=.2f && throwDir.x<=0){
				animInt = 1; //downLeft
				highLow = 1;
			}
			else if (throwDir.y<=.2f && throwDir.x>0){
				animInt = 2; //downRight
				highLow = 1;
			}
			else if (throwDir.y>.2f && throwDir.x<=0){
				animInt = 3; //upLeft
				highLow = 2;
			}
			else if (throwDir.y>.2f && throwDir.x>0){
				animInt = 4; //upRight
				highLow = 2;
			}

			StartCoroutine (spearScript.FlyFree(throwDir, throwForce, highLow));

			jaiAnimator.SetInteger("AnimState",animInt);
			yield return new WaitForSeconds (Constants.timeToThrow);
			throwing = false;
			jaiAnimator.SetInteger("AnimState",0);
			throws++;
		}
		yield return null;
	}

}
