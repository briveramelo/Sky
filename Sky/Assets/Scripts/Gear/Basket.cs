using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GenericFunctions;
using System.Linq;
using UnityEngine.SceneManagement;

public interface ITentacleToBasket {
	void KnockDown(float downForce);
	void LoseAllBalloons();
	void AttachToTentacles(Transform tentaclesTransform);
	void DetachFromTentacles();
}
public interface IBalloonToBasket {
	void ReportPoppedBalloon(IBasketToBalloon poppedBalloon);
}
public class Basket : MonoBehaviour, IBalloonToBasket, ITentacleToBasket {

	public static Basket Instance;
	public static IBalloonToBasket BalloonToBasket;
	public static ITentacleToBasket TentacleToBasket;

	[SerializeField] Rigidbody2D rigbod;
	[SerializeField] Transform balloonCenter;
	[SerializeField] Transform basketCenter;
	[SerializeField] Balloon[] balloonScripts; private List<IBasketToBalloon> balloons;
	[SerializeField] BoxCollider2D basketCollider;
    [SerializeField] Collider2D[] boundingColliders;
    [SerializeField] GameObject balloonReplacement;
    [SerializeField] List<SpriteRenderer> mySprites;
    [SerializeField] AudioClip invincible, ready, rebirth;

    [SerializeField] BasketEngine basketEngine;

	Vector2[] relativeBalloonPositions;
    int continuesRemaining =1;
    const float invincibleTime = 1.5f;

	void Awake () {
		Instance = this;
		BalloonToBasket = (IBalloonToBasket)this;
		TentacleToBasket = (ITentacleToBasket)this;
		Constants.balloonCenter = balloonCenter;
		Constants.basketTransform = basketCenter;
		balloons = new List<IBasketToBalloon>((IBasketToBalloon[])balloonScripts);
		relativeBalloonPositions = new Vector2[3];
        for (int i=0; i<balloons.Count; i++){
			balloons[i].BalloonNumber = i;
			relativeBalloonPositions[i] = balloonScripts[i].transform.position - Constants.jaiTransform.position;
		}
	}

	#region IBalloonToBasket
	void IBalloonToBasket.ReportPoppedBalloon(IBasketToBalloon poppedBalloon){
		balloons.Remove(poppedBalloon);
        ScoreSheet.Tallier.TallyThreat(Threat.BalloonPopped);
		GrantBalloonInvincibility();
        PlayRecoverySounds();
		if (balloons.Count<1){
			StartCoroutine (FallToDeath());
		}
	}
	#endregion

	void GrantBalloonInvincibility(){
		for (int i=0; i<balloons.Count; i++){
			StartCoroutine (balloons[i].BecomeInvincible());
		}
        StartCoroutine(FlashColor(invincibleTime));
	}

    void PlayRecoverySounds() {
        AudioManager.PlayAudio(invincible);
        if (balloons.Count>=1) {
            AudioManager.PlayReadyDelayed(invincible.length);
        }
    }

    IEnumerator FlashColor(float invincibleTime) {
        bool isVisible = false;
        Color invisible = Color.clear;
        Color visible = Color.white;
        float timePassed = 0f;
        float invisibleTime = 0.1f;
        float visibleTime = 0.2f;
        while (timePassed<invincibleTime) {
            mySprites.ForEach(sprite => sprite.color = isVisible ? visible : invisible);
            float timeToWait = isVisible ? visibleTime : invisibleTime;
            yield return new WaitForSeconds(timeToWait);
            timePassed += timeToWait;
            isVisible = !isVisible;
        }
        mySprites.ForEach(sprite => sprite.color = visible);
        
    }

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.layer == Constants.balloonFloatingLayer){
			if (balloons.Count<3){
				CollectNewBalloon(col.gameObject.GetComponent<IBasketToBalloon>());
			}
		}
	}

	void CollectNewBalloon(IBasketToBalloon newBalloon){
        List<int> balloonNumbers = new List<int>(new int[] { 0, 1, 2 });
        balloons.ForEach(balloon => balloonNumbers.Remove(balloon.BalloonNumber));
        int newBalloonNumber= balloonNumbers[0];

		newBalloon.AttachToBasket(relativeBalloonPositions[newBalloonNumber]);
		newBalloon.BalloonNumber = newBalloonNumber;
		balloons.Add(newBalloon);
        ScoreSheet.Tallier.TallyThreat(Threat.BalloonGained);
    }

	#region ITentacleToBasket
	void ITentacleToBasket.KnockDown(float downForce){
		rigbod.AddForce(Vector2.down * downForce);
	}

	void ITentacleToBasket.LoseAllBalloons(){
		rigbod.velocity = Vector2.zero;
		for (int i=0; i<balloons.Count; i++){
			balloons[i].DetachFromBasket();
		}
		balloons.Clear();
		StartCoroutine(FallToDeath());
	}

	void ITentacleToBasket.AttachToTentacles(Transform tentaclesTransform){
		rigbod.velocity = Vector2.zero;
		rigbod.isKinematic = true;
		basketCollider.enabled = false;
        ScoreSheet.Tallier.TallyThreat(Threat.BasketGrabbed);
        transform.parent = tentaclesTransform;
	}

	void ITentacleToBasket.DetachFromTentacles(){
        transform.parent = null;
		transform.localScale = Vector3.one;
		basketCollider.enabled = true;
		rigbod.isKinematic = false;
        ScoreSheet.Tallier.TallyThreat(Threat.BasketReleased);
    }
	#endregion

    IEnumerator FinishPlay() {
		ScoreSheet.Reporter.ReportScores ();
        yield return StartCoroutine (ScoreSheet.Reporter.DisplayTotal());
		SceneManager.LoadScene((int)Scenes.Menu);
    }

    IEnumerator FallToDeath(){
        rigbod.gravityScale = 1;
        ((IDie)basketEngine).Die();
        boundingColliders.ToList().ForEach(col => col.enabled = false);
        yield return new WaitForSeconds (invincibleTime);
        if (continuesRemaining>0) {
            continuesRemaining--;
            FindObjectOfType<Continuer>().DisplayContinueMenu(true);
        }
        else {
            StartCoroutine(FinishPlay());
        }
        yield return null;
	}

    public void ComeBackToLife() {
        rigbod.gravityScale = 0;
        ((IDie)basketEngine).Rebirth();
        PlayRebirthSounds();
        transform.position = Vector3.zero;
        rigbod.velocity = Vector2.zero;
        boundingColliders.ToList().ForEach(col => col.enabled = true);
        IBasketToBalloon newBalloon;
        for (int i=0; i<3; i++) {
            newBalloon = (Instantiate(balloonReplacement, Vector3.zero, Quaternion.identity) as GameObject).GetComponent<IBasketToBalloon>();
            CollectNewBalloon(newBalloon);
        }
        GrantBalloonInvincibility();
    }

    void PlayRebirthSounds() {
        AudioManager.PlayAudio(rebirth);
    }
}