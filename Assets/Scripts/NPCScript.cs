using UnityEngine;

public class NPCScript : MonoBehaviour
{
    SpriteAnimator spriteAnimator;
    DialogueTrigger dialogueTrigger;
    InputManager inputManager;

    private void Awake()
    {
        spriteAnimator = GetComponent<SpriteAnimator>();
        dialogueTrigger = GetComponent<DialogueTrigger>();
        inputManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spriteAnimator.ChangeAnimationState("NPC_ResistanceSoldierTalkRadius");
            dialogueTrigger.allowEnable = true;
            inputManager.dialogueTrigger = dialogueTrigger;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spriteAnimator.ChangeAnimationState("NPC_ResistanceSoldierIdle");
            dialogueTrigger.allowEnable = false;
            inputManager.dialogueTrigger = null;
        }
    }
}
