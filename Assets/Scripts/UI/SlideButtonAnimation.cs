using UnityEngine;
using UnityEngine.EventSystems;

public class SlideButtonAnimation : MonoBehaviour
{
     // Animations for button states
    static string selectAnimation = "MainMenu_SlideOut", deselectAnimation = "MainMenu_SlideIn", defaultState = "MainMenu_Idle";

    Animator animator;
    AnimatorClipInfo[] currentClip;
    SpriteAnimator spriteAnimator; // Sprite animator will animate the button

    private void Start() // Get components
    {
        animator = GetComponent<Animator>();
        spriteAnimator = GetComponent<SpriteAnimator>();
    }

    private void Update()
    {
        currentClip = animator.GetCurrentAnimatorClipInfo(0); // Get info about the current clip

        if(EventSystem.current.currentSelectedGameObject == this.gameObject) // if this object is currently selected
            spriteAnimator.ChangeAnimationState(selectAnimation, 0);
        else if (currentClip[0].clip.name != defaultState) // If the current clip is not the default state
            spriteAnimator.ChangeAnimationState(deselectAnimation, 0);
        else
        {
            spriteAnimator.ChangeAnimationState(defaultState, 0);
        }

    }
}
