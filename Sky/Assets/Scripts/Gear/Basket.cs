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
	[SerializeField] private List<Balloon> balloonScripts; private List<IBasketToBalloon> balloons;
	[SerializeField] private BoxCollider2D basketCollider;

	private Vector2[] relativeBalloonPositions;

	void Awake () {
		Instance = this;
		BalloonToBasket = (IBalloonToBasket)this;
		TentacleToBasket = (ITentacleToBasket)this;
		Constants.balloonCenter = balloonCenter;
		Constants.basketTransform = basketCenter;
		balloons = new List<IBasketToBalloon>();
		relativeBalloonPositions = new Vector2[3];
        for (int i=0; i<balloonScripts.Count; i++){
			balloons.Add((IBasketToBalloon)balloonScripts[i]);
			balloons[i].BalloonNumber = i;
			relativeBalloonPositions[i] = balloonScripts[i].transform.position - Constants.jaiTransform.position;
		}

	}

	#region IBalloonToBasket
	void IBalloonToBasket.ReportPoppedBalloon(IBasketToBalloon poppedBalloon){
		balloons.Remove(poppedBalloon);
        ScoreSheet.Tallier.TallyThreat(Threat.BalloonPopped);
		GrantBalloonInvincibility();
		if (balloons.Count<1){
			StartCoroutine (EndItAll());
		}
	}
	#endregion

	void GrantBalloonInvincibility(){
		for (int i=0; i<balloons.Count; i++){
			StartCoroutine (balloons[i].BecomeInvincible());
		}
	}

	IEnumerator EndItAll(){
		rigbod.gravityScale = 1;
		basketCollider.enabled = false;
		SaveLoadData.Instance.PromptSave ();
		yield return new WaitForSeconds (1f);
		SceneManager.LoadScene((int)Scenes.Menu);
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.layer == Constants.balloonFloatingLayer){
			if (balloons.Count<3){
				CollectNewBalloon(col.gameObject.GetComponent<IBasketToBalloon>());
			}
		}
	}

	void CollectNewBalloon(IBasketToBalloon newBalloon){
		int newBalloonNumber=0;
		for (int i=0; i<balloonScripts.Count; i++){
			if (!balloons.Any(balloon => balloon.BalloonNumber==i)){
				newBalloonNumber=i;
				break;
			}
		}
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
		StartCoroutine(EndItAll());
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
}