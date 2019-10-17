using UnityEngine;
using System.Collections;
using GenericFunctions;

public class PooSlide : MonoBehaviour {

	[SerializeField] private GameObject maskCamera;
	[SerializeField] private SpriteRenderer mySpriteRenderer;
	[SerializeField] private Animator pooAnimator;
    [SerializeField] private Material[] pooMaterials;

	[SerializeField] private Sprite[] lastPooSprites;

	private void Awake(){
        mySpriteRenderer.material = pooMaterials[Constants.TargetPooInt];
        StartCoroutine (AnimateSplat (Constants.TargetPooInt));
		StartCoroutine (SlideDown ());
		Destroy (transform.parent.gameObject,15f);
        ScoreSheet.Tallier.TallyThreat(Threat.Poop);

        Constants.TargetPooInt++;
        Seagull.LogPooCam(true);
	}

	private IEnumerator AnimateSplat(int pooCount){
		while (pooAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime<1f){
			yield return null;
		}
		Destroy (pooAnimator);
		int i = ((pooCount+2) % 2) == 0 ? 0 :1;
		mySpriteRenderer.sprite = lastPooSprites[1];
	}

	private float slideSpeed = .08f;

	private IEnumerator SlideDown(){
		while (true){
			transform.position += Vector3.down * slideSpeed;
            yield return new WaitForSeconds(Random.Range(0.3f, 0.6f));
		}
	}

	private void OnDestroy(){
		StopAllCoroutines ();
        Seagull.LogPooCam(false);
        ScoreSheet.Tallier.TallyThreat(Threat.PoopCleaned);
    }
}
