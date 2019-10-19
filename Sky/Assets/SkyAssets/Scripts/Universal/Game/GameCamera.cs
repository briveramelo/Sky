using UnityEngine;
using System.Collections;

public class GameCamera : Singleton<GameCamera>
{
    private bool _shaking;
    private Vector3 _startSpot = Vector3.zero;

    public void ShakeTheCamera()
    {
        //todo: fix
        //StartCoroutine (TriggerShake());
    }

    private IEnumerator TriggerShake()
    {
        StartCoroutine(ShakeIt());
        yield return new WaitForSeconds(.1f);
        _shaking = false;
    }

    private IEnumerator ShakeIt()
    {
        _shaking = true;
        while (_shaking)
        {
            var shift = new Vector3(Random.insideUnitCircle.x * .2f, Random.insideUnitCircle.y * .2f, 0f);
            transform.position = _startSpot + shift;
            yield return null;
        }

        transform.position = _startSpot;
    }
}