using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    InputManager inputManager;
    [HideInInspector] public bool allowEnable = false;

    private void Awake()
    {
        inputManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
    }

    private void Update()
    {
        if (allowEnable && inputManager.interacting)
            TriggerDialogue();
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
