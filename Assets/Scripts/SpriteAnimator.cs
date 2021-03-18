using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    // Animation States
    string currentState; // Current animation state
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }



    public void ChangeAnimationState(string newState)
    {

        //stop the same animation from interrupting itself
        if (currentState == newState) return;
        
        // Play the animation
        animator.Play(newState);

        // Reassign the current state
        currentState = newState;
    }
}
