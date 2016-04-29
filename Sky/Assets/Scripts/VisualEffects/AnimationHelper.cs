using UnityEngine;
using System.Collections;

public class AnimationHelper : MonoBehaviour {

	[SerializeField] Animator myAnimator;

    void SetAnimState(TextAnimState MyAnim) {
        myAnimator.SetInteger("AnimState", (int)MyAnim);
    }
}
