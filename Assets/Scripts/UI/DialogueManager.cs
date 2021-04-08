using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    public Queue<string> names; // A queue will go through each item one by one.
    public Queue<string> sentences;
    public Queue<DialogueTalkSprites> talkSprites;
    public Queue<bool> enabledButtons;
    public Queue<int> amountOfButtons;

    [SerializeField] Image leftTalkSprite, rightTalkSprite;
    

    [SerializeField] Text nameText, dialogueText;
    [SerializeField] float timeForNextChar = 0.01f; // Time before next character is displayed
    [SerializeField] float charTimeMultiplier = 2; // If the A button is pressed, char time is divided by this number
    float currentCharTime;

    bool finishedSentence = false; // When the sentence is finished, this is true.
    public bool inDialogue;

    [SerializeField] GameObject dialogueBox, nextButton, buttonHolder;
    [SerializeField] GameObject[] selectionButtons;
    [SerializeField] ObjectAudioManager audioManager;

    InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        names = new Queue<string>();
        sentences = new Queue<string>();
        talkSprites = new Queue<DialogueTalkSprites>();
        enabledButtons = new Queue<bool>();
        amountOfButtons = new Queue<int>();

        inputManager = GetComponent<InputManager>();
        currentCharTime = timeForNextChar;
    }

    public void StartDialogue(Dialogue[] dialogue)
    {
        inDialogue = true; // User is in dialogue
        inputManager.DisableDialogueButtonSelection(); // Disable button input
        Time.timeScale = 0.0f; // Freeze game

        dialogueBox.SetActive(true);
        dialogueBox.GetComponent<Animator>().Play("Dialogue_Start");

        // Clear Queues
        names.Clear();
        sentences.Clear();
        talkSprites.Clear();
        enabledButtons.Clear();
        amountOfButtons.Clear();


        foreach (Dialogue thing in dialogue)
        {
            // Add dialogue to queue 
            names.Enqueue(thing.name);
            talkSprites.Enqueue(thing.talkSprites);
            sentences.Enqueue(thing.sentence); 
            enabledButtons.Enqueue(thing.allowButtons);
            amountOfButtons.Enqueue(thing.amountOfButtons);
        }

        DisplayNextSentence();
    }

    public void OnButtonPressed()
    {
        // speed up char speed if sentence isn't finished
        if (!finishedSentence)
            currentCharTime /= charTimeMultiplier;
        else
            DisplayNextSentence();
    }

    public void OnButtonReleased()
    {
        // Time for next character is set to normal
        currentCharTime = timeForNextChar;
    }

    public void DisplayNextSentence() // Display next sentence
    {
        buttonHolder.SetActive(false);
        // Disable all the buttons
        foreach (var item in selectionButtons)
        {
            item.SetActive(false);
        }

        finishedSentence = false; // since a new sentence is displayed, set finished to false
        nextButton.SetActive(false);
        if (sentences.Count == 0) // if there are no sentences left
        {
            EndDialogue(); // End conversation
            return; // return out of function
        }

        // Dequeue all of the information for the sentence
        string name = names.Dequeue();
        string sentence = sentences.Dequeue();
        Sprite newLeftTalkSprite = talkSprites.Peek().leftSprite;
        Sprite newRightTalkSprite = talkSprites.Dequeue().rightSprite;
        bool enableButton = enabledButtons.Dequeue();
        int buttons = amountOfButtons.Dequeue();
        
        StopAllCoroutines();
        StartCoroutine(TypeSentence(name, sentence, newLeftTalkSprite, newRightTalkSprite, enableButton, buttons));
    }

    void EndDialogue()
    {
        Time.timeScale = 1;
        inputManager.EndOfDialogue();
        dialogueBox.GetComponent<Animator>().Play("Dialogue_End");
        inDialogue = false;
    }

    IEnumerator TypeSentence (string name, string sentence, Sprite left, Sprite right, bool enableButtons, int buttons) {

        nameText.text = name;
        leftTalkSprite.sprite = left;
        rightTalkSprite.sprite = right;
        dialogueText.text = ""; // Set the dialogue text to the current sentence

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            audioManager.Play(name); // Play the current talk sound
            yield return new WaitForSecondsRealtime(currentCharTime);
        }

        if (!enableButtons)
            nextButton.SetActive(true);
        else
            EnableDialogueButtons(buttons);

        finishedSentence = true;
    }

    void EnableDialogueButtons(int buttons) {

        buttonHolder.SetActive(true);

        for (int i = 0; i < buttons; i++)
        {
            selectionButtons[i].SetActive(true);
        }
        EventSystem.current.SetSelectedGameObject(selectionButtons[0]);
        inputManager.EnableDialogueButtonSelection();
    }


}
