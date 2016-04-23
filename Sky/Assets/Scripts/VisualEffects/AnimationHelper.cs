using UnityEngine;
using System.Collections;

public class AnimationHelper : MonoBehaviour {

	[SerializeField] Animator myAnimator;

    void SetAnimState(int i) {
        myAnimator.SetInteger("AnimState", i);
    }
}
