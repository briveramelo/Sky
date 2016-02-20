using UnityEngine;

public interface ICrowToMurder {
	void SendNextCrow();
	void ReportCrowDown(IMurderToCrow crowDown);
	int Cycle{get;}
}
