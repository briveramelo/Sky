using UnityEngine;
using System.Collections;

public interface ITentacleToSensor {

	void ToggleSensor(bool active);
	bool JaiInRange{get;}
}
