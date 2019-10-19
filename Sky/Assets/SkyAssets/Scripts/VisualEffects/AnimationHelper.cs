using UnityEngine;

public class AnimationHelper : MonoBehaviour
{
    [SerializeField] private Animator _myAnimator;

    private void SetAnimState(TextAnimState myAnim)
    {
        _myAnimator.SetInteger(0, (int) myAnim);
    }
}