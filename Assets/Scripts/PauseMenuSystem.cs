using UnityEngine;
using UnityEngine.UI;

public class PauseMenuSystem : MonoBehaviour
{
    [SerializeField] Text[] menuText;
    int currentButtonSelected;
    int previousSelected;
    InputManager inputManager;
    bool changeOnThisUpdate = false;
    int standardTextSize;
    [SerializeField] int highlightedTextSize = 11;

    GameManager gameManager;

    private void Awake()
    {
        standardTextSize = menuText[0].fontSize;
        inputManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnEnable()
    {
        currentButtonSelected = 0;
        ChangeToButton();

    }

    private void OnDisable()
    {
        for (int i = 0; i < menuText.Length; i++)
        {
            menuText[i].fontSize = standardTextSize;
        }
    }

    private void Update()
    {
        
        if (inputManager.pauseInput < 0) // If down is pressed
        {
            previousSelected = currentButtonSelected; // Get current selected button as previous
            currentButtonSelected ++; // increment
            changeOnThisUpdate = true; // Change selection on this update
        }
        else if (inputManager.pauseInput > 0) // If up is pressed
        {   
            previousSelected = currentButtonSelected;
            currentButtonSelected --; // Decrement
            changeOnThisUpdate = true;
        }

         if (currentButtonSelected > menuText.Length - 1) // If current selected button goes out of the index range
             currentButtonSelected = 0; // set it to 0

         else if (currentButtonSelected < 0) // If it goes under 0
            currentButtonSelected = menuText.Length - 1; // set it to the maximum length

        if (changeOnThisUpdate)
        {
            changeOnThisUpdate = false; // don't change again on this update
            ChangeToButton(); // Change to another button
        }

    }

    void ChangeToButton()
    {
        menuText[previousSelected].fontSize = standardTextSize; // Reset the size of the previous selected text
        menuText[currentButtonSelected].fontSize = highlightedTextSize; // Change the size of the current selected text
    }

    public void Select()
    {   
        switch(menuText[currentButtonSelected].gameObject.name)
        {
            case "Resume":
                gameManager.Unpause();
            break;
            case "Restart":
                gameManager.Restart();
                gameManager.Unpause();
            break;
        }
    }
}
