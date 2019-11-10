using GenericFunctions;
using UnityEngine;

public class BirdPoints : PointDisplay
{
    private const float _moveSpeed = 300f;

    private void Awake()
    {
        Destroy(gameObject, 1f);
    }

    private void Update()
    {
        transform.position += Constants.SpeedMultiplier * _moveSpeed * Time.deltaTime * Vector3.up;
    }

    protected override void DisplayPoints(int points)
    {
        _myText.text = $"+{points}";
    }
}