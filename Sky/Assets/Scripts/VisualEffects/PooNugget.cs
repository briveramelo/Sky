using UnityEngine;
using GenericFunctions;

public class PooNugget : MonoBehaviour {

	[SerializeField] private Rigidbody2D softbody;
	[SerializeField] private GameObject pooSplat;

	void Awake(){
		Destroy (gameObject, 3f);
	}

	public void InitializePooNugget(Vector2 vel){
		softbody.velocity = vel;
	}
		
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.layer == Constants.faceLayer){
			SplatterPoo();
		}
	}

	void SplatterPoo(){
		Instantiate (pooSplat, Vector3.zero, Quaternion.identity);
		Destroy(gameObject);
	}
}