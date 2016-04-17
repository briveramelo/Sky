using UnityEngine;
using UnityEngine.UI;

public abstract class PointDisplay : MonoBehaviour, IDisplayable {

	[SerializeField] protected Text myText;
	void IDisplayable.DisplayPoints(int points){
		DisplayPoints(points);
	}

	protected abstract void DisplayPoints(int points);
}
