using UnityEngine;
using GenericFunctions;

public class PooNugget : MonoBehaviour {

	[SerializeField] private Rigidbody2D _softbody;
	[SerializeField] private GameObject _pooSplat;

	private void Awake(){
		Destroy (gameObject, 3f);
	}

	public void InitializePooNugget(Vector2 vel){
        _softbody.velocity = new Vector2(0.5f * vel.x, 0f);
        transform.FaceForward(vel.x > 0);
	}

	private void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.layer == Constants.FaceLayer){
			SplatterPoo();
		}
	}

	private void SplatterPoo(){
		Instantiate (_pooSplat, Vector3.zero, Quaternion.identity);
		Destroy(gameObject);
	}
}