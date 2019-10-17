using UnityEngine;

public class AnimationHelper : MonoBehaviour {

	[SerializeField] private Animator myAnimator;

    private void SetAnimState(TextAnimState MyAnim) {
        myAnimator.SetInteger("AnimState", (int)MyAnim);
    }
}
