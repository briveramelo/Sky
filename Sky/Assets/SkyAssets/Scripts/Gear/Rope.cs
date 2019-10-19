using GenericFunctions;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] private Animator _ropeAnimator;

    private float _lastXPosition;
    private const float _speedThreshold = 0.019f;

    private static class RopeAnimState
    {
        public const int Idle = 0;
        public const int Waving = 1;
    }

    private void Update()
    {
        var pos = transform.position;
        var fastEnough = Mathf.Abs(pos.x - _lastXPosition) > _speedThreshold;
        _ropeAnimator.SetInteger(Constants.AnimState, fastEnough ? RopeAnimState.Waving : RopeAnimState.Idle);
        _lastXPosition = pos.x;
    }
}