using UnityEngine;
using System.Collections;

public interface IThrowable {
	IEnumerator FlyFree(Vector2 throwDir, float throwForce, int lowOrHighThrow);
}
