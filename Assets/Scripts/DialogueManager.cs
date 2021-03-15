using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public Queue<string> sentences; // A queue will go through each item one by one.

    [SerializeField] Text nameText;
    [SerializeField] Text dialogueText;

    [SerializeField] GameObject dialogueBox;
    
    InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        inputManager = GetComponent<InputManager>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        Time.timeScale = 0.0f;

        dialogueBox.SetActive(true);
        dialogueBox.GetComponent<Animator>().Play("Dialogue_Start");
        inputManager.ChangeGameState(3);
        nameText.text = dialogue.name; // Set the name text to the name of the character

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence); // Add sentences to queue
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence() // Display next sentence
    {
        if (sentences.Count == 0) // if there are no sentences left
        {
            EndDialogue(); // End conversation
            return; // return out of function
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence; // Set the dialogue text to the current sentence
    }

    void EndDialogue()
    {
        Time.timeScale = 1;
        inputManager.EndOfDialogue();
        dialogueBox.GetComponent<Animator>().Play("Dialogue_End");
    }

}
