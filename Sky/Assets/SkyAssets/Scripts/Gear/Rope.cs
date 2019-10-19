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
        var fastEnough = Mathf.Abs(transform.position.x - _lastXPosition) > _speedThreshold;
        _ropeAnimator.SetInteger("AnimState", fastEnough ? (int) RopeAnimState.Waving : (int) RopeAnimState.Idle);
        _lastXPosition = transform.position.x;
    }
}