using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue[] dialogue;
    
    [HideInInspector] public bool allowEnable = false;

    public void TriggerDialogue()
    {
        if (allowEnable)
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
