using UnityEngine;
using System.Collections;
using GenericFunctions;

public class PooSlide : MonoBehaviour {

	[SerializeField] private GameObject maskCamera;
	[SerializeField] private Rigidbody2D rigbod;
	[SerializeField] private SpriteRenderer mySpriteRenderer;
	[SerializeField] private Animator pooAnimator;

	[SerializeField] private Sprite[] lastPooSprites;

	private float slideSpeed = .72f;

	void Awake(){
		StartCoroutine (AnimateSplat (Constants.TargetPooInt));
		StartCoroutine (SlideDown ());
		Destroy (transform.parent.gameObject,15f);

		Constants.TargetPooInt++;
		Constants.poosOnJaisFace++;
	}

	IEnumerator AnimateSplat(int pooCount){
		while (pooAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime<1f){
			yield return null;
		}
		Destroy (pooAnimator);
		//int i = ((pooCount+2) % 2) == 0 ? 0 :1;
		mySpriteRenderer.sprite = lastPooSprites[1];
	}

	IEnumerator SlideDown(){
		while (true){
			rigbod.velocity = Vector2.down * slideSpeed;
			yield return null;
		}
	}

	void OnDestroy(){
		StopAllCoroutines ();
		Constants.poosOnJaisFace--;
	}
}
