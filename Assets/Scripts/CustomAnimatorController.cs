using UnityEngine;

public class CustomAnimatorController : MonoBehaviour
{
    public Animator animator;

    public void PlayAnimation(AnimationClip animationClip)
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        animator.StopPlayback();
        animator.Play(animationClip.name.ToString());
    }
}
