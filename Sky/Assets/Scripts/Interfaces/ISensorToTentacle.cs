using UnityEngine;
using System.Collections;

public interface ISensorToTentacle {

	IEnumerator GoForTheKill();
	IEnumerator ResetPosition(bool defeated);
}
