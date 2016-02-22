using UnityEngine;
using System.Collections;

public interface IControllable {

	IEnumerator ThrowSpear(Vector2 throwDir);
	IEnumerator StabTheBeast();
}
