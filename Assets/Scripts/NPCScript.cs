using UnityEngine;

public class NPCScript : MonoBehaviour
{
    SpriteAnimator spriteAnimator;
    DialogueTrigger dialogueTrigger;

    private void Awake()
    {
        spriteAnimator = GetComponent<SpriteAnimator>();
        dialogueTrigger = GetComponent<DialogueTrigger>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spriteAnimator.ChangeAnimationState("NPC_ResistanceSoldierTalkRadius");
            dialogueTrigger.allowEnable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spriteAnimator.ChangeAnimationState("NPC_ResistanceSoldierIdle");
            dialogueTrigger.allowEnable = false;
        }
    }
}
