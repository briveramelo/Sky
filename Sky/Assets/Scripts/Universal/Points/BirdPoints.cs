using UnityEngine;

public class BirdPoints : PointDisplay {

	[Range(0,1)] public float moveSpeed;
	protected override void DisplayPoints(int points){
		myText.text = "+" + points.ToString();
	}

	void Awake(){
		transform.SetParent(ScoreSheet.Instance.transform);
		Destroy(gameObject, 1f);
	}

	void Update(){
		transform.position += Vector3.up * moveSpeed;
	}
}
