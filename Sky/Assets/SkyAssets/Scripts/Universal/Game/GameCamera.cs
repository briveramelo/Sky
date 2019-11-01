using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameCamera : Singleton<GameCamera>
{
    [SerializeField] private PixelPerfectCamera _pixelCam;
    
    private bool _shaking;
    private Vector3 _startSpot;

    protected override void Awake()
    {
        base.Awake();
        _startSpot = transform.position;
        _pixelCam.cameraZoom = GetCamZoom();
    }

    private int GetCamZoom()
    {
        float diagonalPixels = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height);
        return Mathf.CeilToInt(diagonalPixels / 400f);
    }

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