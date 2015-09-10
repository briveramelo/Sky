using UnityEngine;
using System.Collections;

public class Jai : MonoBehaviour {
	
	public Spear spearScript;

	public Animator jaiAnimator;

	public Vector3 fixVector;

	public float throwForce; //Force with which Jai throws the spear
	public float throwSpearTime; //time between initiating the throw and when the spear leaves his hand

	public int throws; //counts number of throws he's done
	public int animInt;

	void Awake(){
		throwForce = 1500f;
		throwSpearTime = 0.5f;
		fixVector = new Vector3 (0f, .02f);
		jaiAnimator = GetComponent<Animator> ();
	}

	public IEnumerator ThrowSpear(Vector2 throwDir){
		if (!spearScript.throwing){
			if (throwDir.y>.2f){
				animInt = 2; //highThrow
			}
			else{
				animInt = 1;// low throw
			}

			StartCoroutine (spearScript.FlyFree(throwDir, throwForce,animInt));

			if ((throwDir.x>0 && animInt==1) || (throwDir.x<0 && animInt == 2)){
				transform.localScale = new Vector3 (-1,1,1); 
				spearScript.transform.localScale = new Vector3 (-1,1,1);
			}
			else{
				transform.localScale = new Vector3 (1,1,1);
			}

			jaiAnimator.SetInteger("AnimState",animInt);
			yield return new WaitForSeconds (throwSpearTime);
			jaiAnimator.SetInteger("AnimState",0);
			//transform.position += fixVector;
			throws++;
		}
		yield return null;
	}

}
