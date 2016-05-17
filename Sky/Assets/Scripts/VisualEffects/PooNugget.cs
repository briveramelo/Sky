using UnityEngine;
using GenericFunctions;

public class PooNugget : MonoBehaviour {

	[SerializeField] Rigidbody2D softbody;
	[SerializeField] GameObject pooSplat;

	void Awake(){
		Destroy (gameObject, 3f);
	}

	public void InitializePooNugget(Vector2 vel){
        softbody.velocity = new Vector2(0.5f * vel.x, 0f);
        transform.FaceForward(vel.x > 0);
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