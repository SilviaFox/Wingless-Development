using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Queue<string> names; // A queue will go through each item one by one.
    public Queue<string> sentences;
    public Queue<DialogueTalkSprites> talkSprites;

    [SerializeField] Image leftTalkSprite;
    [SerializeField] Image rightTalkSprite;

    [SerializeField] Text nameText;
    [SerializeField] Text dialogueText;
    [SerializeField] float timeForNextChar = 0.01f; // Time before next character is displayed
    [SerializeField] float charTimeMultiplier = 2; // If the A button is pressed, char time is divided by this number
    float currentCharTime;

    bool finishedSentence = false; // When the sentence is finished, this is true.

    [SerializeField] GameObject dialogueBox, nextButton;
    [SerializeField] ObjectAudioManager audioManager;

    InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        names = new Queue<string>();
        sentences = new Queue<string>();
        talkSprites = new Queue<DialogueTalkSprites>();

        inputManager = GetComponent<InputManager>();
        currentCharTime = timeForNextChar;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        Time.timeScale = 0.0f;

        dialogueBox.SetActive(true);
        dialogueBox.GetComponent<Animator>().Play("Dialogue_Start");
        // nameText.text = dialogue.name; // Set the name text to the name of the character

        names.Clear();
        sentences.Clear();
        talkSprites.Clear();

        int loops = 0;

        foreach (string sentence in dialogue.sentences)
        {
            names.Enqueue(dialogue.name[loops]);
            talkSprites.Enqueue(dialogue.talkSprites[loops]);
            sentences.Enqueue(sentence); // Add sentences to queue
            loops ++;
        }

        DisplayNextSentence();
    }

    public void OnButtonPressed()
    {
        if (!finishedSentence)
            currentCharTime /= charTimeMultiplier;
        else
            DisplayNextSentence();
    }

    public void OnButtonReleased()
    {
        currentCharTime = timeForNextChar;
    }

    public void DisplayNextSentence() // Display next sentence
    {
        finishedSentence = false; // since a new sentence is displayed, set finished to false
        nextButton.SetActive(false);
        if (sentences.Count == 0) // if there are no sentences left
        {
            EndDialogue(); // End conversation
            return; // return out of function
        }

        string name = names.Dequeue();
        string sentence = sentences.Dequeue();
        Sprite newLeftTalkSprite = talkSprites.Peek().leftSprite;
        Sprite newRightTalkSprite = talkSprites.Dequeue().rightSprite;
        
        StopAllCoroutines();
        StartCoroutine(TypeSentence(name, sentence, newLeftTalkSprite, newRightTalkSprite));
    }

    void EndDialogue()
    {
        Time.timeScale = 1;
        inputManager.EndOfDialogue();
        dialogueBox.GetComponent<Animator>().Play("Dialogue_End");
    }

    IEnumerator TypeSentence (string name, string sentence, Sprite left, Sprite right) {

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

        nextButton.SetActive(true);
        finishedSentence = true;
    }

}
