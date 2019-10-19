using GenericFunctions;
using UnityEngine;

public class AnimationHelper : MonoBehaviour
{
    [SerializeField] private Animator _myAnimator;

    //called from the animation keyframe events 
    public void SetAnimState(int animState)
    {
        _myAnimator.SetInteger(Constants.AnimState, animState);
    }
}