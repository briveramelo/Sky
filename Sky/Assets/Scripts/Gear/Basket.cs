using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GenericFunctions;
using System.Linq;

public class Basket : MonoBehaviour, IBalloonToBasket, ITentacleToBasket {

	public static Basket Instance;
	public static IBalloonToBasket BalloonToBasket;
	public static ITentacleToBasket TentacleToBasket;

	[SerializeField] private Rigidbody2D rigbod;
	[SerializeField] private Transform balloonCenter;
	[SerializeField] private List<Balloon> balloonScripts; private List<IBasketToBalloon> basketToBalloons;
	[SerializeField] private BoxCollider2D[] basketColliders;

	private Vector3[] relativeBalloonPositions;
	
	private float dropForce = 100f;

	// Use this for initialization
	void Awake () {
		Instance = this;
		BalloonToBasket = (IBalloonToBasket)this;
		TentacleToBasket = (ITentacleToBasket)this;

		Constants.balloonCenter = balloonCenter;
		basketToBalloons = new List<IBasketToBalloon>();
		for (int i=0; i<balloonScripts.Count; i++){
			basketToBalloons.Add((IBasketToBalloon)balloonScripts[i]);
			basketToBalloons[i].BalloonNumber = i;
		}

		Constants.basketTransform = transform;
		relativeBalloonPositions = new Vector3[]{
			balloonScripts[0].transform.position - Constants.jaiTransform.position,
			balloonScripts[1].transform.position - Constants.jaiTransform.position,
			balloonScripts[2].transform.position - Constants.jaiTransform.position
		};
	}

	#region IBalloonToBasket
	void IBalloonToBasket.ReportPoppedBalloon(IBasketToBalloon IpoppedBalloon){
		if (basketToBalloons.Remove(IpoppedBalloon)){
			GrantBalloonInvincibility();
			if (basketToBalloons.Count<1){
				StartCoroutine (EndItAll());
			}
		}
	}
	#endregion

	void GrantBalloonInvincibility(){
		for (int i=0; i<basketToBalloons.Count; i++){
			StartCoroutine (basketToBalloons[i].BecomeInvincible());
		}
	}

	IEnumerator EndItAll(){
		rigbod.gravityScale = 1;
		for (int i=0; i<basketColliders.Length; i++){
			basketColliders[i].enabled = false;
		}
		yield return new WaitForSeconds (2.5f);
		foreach (Rigidbody2D rigger in FindObjectsOfType<Rigidbody2D>()){
			rigger.isKinematic = true;
		}
		SaveLoadData.Instance.PromptSave ();
//		while (SaveLoadData.Instance.prompting){
//			yield return null;
//		}
		yield return new WaitForSeconds (1f);
		UnityEditor.EditorApplication.isPlaying = false;
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.layer == Constants.balloonFloatingLayer){
			if (basketToBalloons.Count<3){
				CollectNewBalloon(col.gameObject.GetComponent<IBasketToBalloon>());
			}
		}
	}

	void CollectNewBalloon(IBasketToBalloon basketToBalloon){
		int newBalloonNumber=0;
		for (int i=0; i<balloonScripts.Count; i++){
			if (!basketToBalloons.Any(balloon => balloon.BalloonNumber==i)){
				newBalloonNumber=i;
			}
		}
		basketToBalloons.Add(basketToBalloon);
		basketToBalloon.BalloonNumber = newBalloonNumber;
		basketToBalloon.AttachToBasket(relativeBalloonPositions[newBalloonNumber]);
	}

	#region ITentacleToBasket
	void ITentacleToBasket.KnockDown(float downForce){
		rigbod.AddForce(Vector2.down * downForce);
	}

	void ITentacleToBasket.LoseAllBalloons(){
		rigbod.velocity = Vector2.zero;
		for (int i=0; i<basketToBalloons.Count; i++){
			basketToBalloons[i].DetachFromBasket();
		}
		basketToBalloons.Clear();
		StartCoroutine(EndItAll());
	}

	void ITentacleToBasket.AttachToTentacles(Transform tentaclesTransform){
		rigbod.velocity = Vector2.zero;
		rigbod.isKinematic = true;
		for (int i=0; i<basketColliders.Length; i++){
			basketColliders[i].enabled = false;
		}
		transform.parent = tentaclesTransform;
	}

	void ITentacleToBasket.DetachFromTentacles(){
		transform.parent = null;
		transform.localScale = Vector3.one;
		for (int i=0; i<basketColliders.Length; i++){
			basketColliders[i].enabled = true;
		}
		rigbod.isKinematic = false;
	}

	Collider2D[] ITentacleToBasket.BasketColliders{get{return basketColliders;}}
	#endregion
}