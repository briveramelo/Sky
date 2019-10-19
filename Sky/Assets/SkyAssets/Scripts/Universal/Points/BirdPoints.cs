using UnityEngine;

public class BirdPoints : PointDisplay
{
    private const float _moveSpeed = .5f;

    protected override void DisplayPoints(int points)
    {
        _myText.text = "+" + points.ToString();
    }

    private void Awake()
    {
        transform.SetParent(ScoreSheet.Instance.transform);
        Destroy(gameObject, 1f);
    }

    private void Update()
    {
        transform.position += Vector3.up * _moveSpeed * Time.deltaTime;
    }
}