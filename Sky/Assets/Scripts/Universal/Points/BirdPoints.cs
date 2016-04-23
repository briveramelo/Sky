using UnityEngine;

public class BirdPoints : PointDisplay {

	const float moveSpeed = 0.017f;
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
