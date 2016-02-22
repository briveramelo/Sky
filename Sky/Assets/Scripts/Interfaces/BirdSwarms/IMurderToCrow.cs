using UnityEngine;

public interface IMurderToCrow{
	void InitializeCrow(int crowNum, Vector2 crowPosition);
	void TakeFlight();
	bool ReadyToFly{get;}
}
