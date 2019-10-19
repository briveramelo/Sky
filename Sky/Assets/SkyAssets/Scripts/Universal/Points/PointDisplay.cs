using UnityEngine;
using UnityEngine.UI;

public interface IDisplayable{
	void DisplayPoints(int points);
}
public abstract class PointDisplay : MonoBehaviour, IDisplayable {

	[SerializeField] protected Text _myText;
	void IDisplayable.DisplayPoints(int points){
		DisplayPoints(points);
	}

	protected abstract void DisplayPoints(int points);
}
