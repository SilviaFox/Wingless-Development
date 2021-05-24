using System.Collections;
using UnityEngine;

public class BreakableLight : MonoBehaviour
{
    [SerializeField] float animationTime;
    SpriteAnimator animator;

    void Start()
    {
        animator = GetComponent<SpriteAnimator>();
    }

    public void OnHit()
    {
        StartCoroutine(AnimateLight(animationTime));
    }

    public IEnumerator AnimateLight(float time)
    {
        float timeToBeat = Time.time + time;
        animator.ChangeAnimationState("BreakableLight_Damage", 0);

        while (timeToBeat > Time.time)
        {
            yield return new WaitForEndOfFrame();
        }

        animator.ChangeAnimationState("BreakableLight_Regen", 0);   
    }

}
