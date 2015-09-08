using UnityEngine;
using System.Collections;

public class Crow : MonoBehaviour {

	public Rigidbody2D rigbod;
	public bool swooping;
	public float rotationSpeed;
	public float targetAngle;
	public Transform stick;
	public int crowNumber;
	public Murder murderScript;

	// Use this for initialization
	void Awake () {
		rigbod = GetComponent<Rigidbody2D> ();
		murderScript = GameObject.Find ("Murder").GetComponent<Murder>();
		rotationSpeed = 2f;
		stick = transform.parent;
	}
	
	void Update(){
		if (swooping){
			targetAngle += rotationSpeed;
			stick.rotation = Quaternion.Euler (0f, 0f, targetAngle);
		}
	}

	void OnDestroy(){
		murderScript.crowHasGone [crowNumber] = true;
	}
}
