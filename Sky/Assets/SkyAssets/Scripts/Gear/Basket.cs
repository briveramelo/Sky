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

	[SerializeField] private Rigidbody2D rigbod;
	[SerializeField] private Transform balloonCenter;
	[SerializeField] private Transform basketCenter;
	[SerializeField] private Balloon[] balloonScripts; private List<IBasketToBalloon> balloons;
	[SerializeField] private BoxCollider2D basketCollider;
    [SerializeField] private Collider2D[] boundingColliders;
    [SerializeField] private GameObject balloonReplacement;
    [SerializeField] private List<SpriteRenderer> mySprites;
    [SerializeField] private AudioClip invincible, ready, rebirth;

    [SerializeField] private BasketEngine basketEngine;

    private Vector2[] relativeBalloonPositions;
    private int continuesRemaining =1;
    private const float invincibleTime = 1.5f;

    private void Awake () {
		Instance = this;
		BalloonToBasket = this;
		TentacleToBasket = this;
		Constants.balloonCenter = balloonCenter;
		Constants.basketTransform = basketCenter;
		balloons = new List<IBasketToBalloon>(balloonScripts);
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

	private void GrantBalloonInvincibility(){
		for (int i=0; i<balloons.Count; i++){
			StartCoroutine (balloons[i].BecomeInvincible());
		}
        StartCoroutine(FlashColor(invincibleTime));
	}

	private void PlayRecoverySounds() {
        AudioManager.PlayAudio(invincible);
        if (balloons.Count>=1) {
            AudioManager.PlayReadyDelayed(invincible.length);
        }
    }

	private IEnumerator FlashColor(float invincibleTime) {
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

	private void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.layer == Constants.balloonFloatingLayer){
			if (balloons.Count<3){
				CollectNewBalloon(col.gameObject.GetComponent<IBasketToBalloon>());
			}
		}
	}

	private void CollectNewBalloon(IBasketToBalloon newBalloon){
        List<int> balloonNumbers = new List<int>(new[] { 0, 1, 2 });
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

	private IEnumerator FinishPlay() {
		ScoreSheet.Reporter.ReportScores ();
        yield return StartCoroutine (ScoreSheet.Reporter.DisplayTotal());
		SceneManager.LoadScene(Scenes.Menu);
    }

	private IEnumerator FallToDeath(){
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
            newBalloon = Instantiate(balloonReplacement, Vector3.zero, Quaternion.identity).GetComponent<IBasketToBalloon>();
            CollectNewBalloon(newBalloon);
        }
        GrantBalloonInvincibility();
    }

    private void PlayRebirthSounds() {
        AudioManager.PlayAudio(rebirth);
    }
}