using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] private Animator _ropeAnimator;

    private float _lastXPosition;
    private const float _speedThreshold = 0.019f;

    private enum RopeAnimState
    {
        Idle = 0,
        Waving = 1
    }

    private void Update()
    {
        var pos = transform.position;
        var fastEnough = Mathf.Abs(pos.x - _lastXPosition) > _speedThreshold;
        _ropeAnimator.SetInteger(0, fastEnough ? (int) RopeAnimState.Waving : (int) RopeAnimState.Idle);
        _lastXPosition = pos.x;
    }
}