using UnityEngine;
using System.Collections;
using GenericFunctions;

public class PooSlide : MonoBehaviour {

	[SerializeField] GameObject maskCamera;
	[SerializeField] SpriteRenderer mySpriteRenderer;
	[SerializeField] Animator pooAnimator;
    [SerializeField] Material[] pooMaterials;

	[SerializeField] Sprite[] lastPooSprites;

	void Awake(){
        mySpriteRenderer.material = pooMaterials[Constants.TargetPooInt];
        StartCoroutine (AnimateSplat (Constants.TargetPooInt));
		StartCoroutine (SlideDown ());
		Destroy (transform.parent.gameObject,15f);
        ScoreSheet.Tallier.TallyThreat(Threat.Poop);

        Constants.TargetPooInt++;
        Seagull.LogPooCam(true);
	}

	IEnumerator AnimateSplat(int pooCount){
		while (pooAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime<1f){
			yield return null;
		}
		Destroy (pooAnimator);
		int i = ((pooCount+2) % 2) == 0 ? 0 :1;
		mySpriteRenderer.sprite = lastPooSprites[1];
	}

    float slideSpeed = .08f;
	IEnumerator SlideDown(){
		while (true){
			transform.position += Vector3.down * slideSpeed;
            yield return new WaitForSeconds(Random.Range(0.3f, 0.6f));
		}
	}

	void OnDestroy(){
		StopAllCoroutines ();
        Seagull.LogPooCam(false);
        ScoreSheet.Tallier.TallyThreat(Threat.PoopCleaned);
    }
}
