using UnityEngine;

public class InputManager : MonoBehaviour
{
    GameManager gameManager;
    DialogueManager dialogueManager;

    [SerializeField] int gameState = 1; // State the player is in.
    // 1 - Player can be controlled
    // 2 - In Pause Menu
    // 3 - In Dialogue Menu

    [HideInInspector] public float inputX; // Directional input for the x axis
    [HideInInspector] public bool dashPressed; // Has dash been pressed
    [HideInInspector] public bool dashReleased; // Has dash been released
    [HideInInspector] public bool jumpPressed; // Has jump been pressed
    [HideInInspector] public bool jumpHeld; // Is jump being held
    [HideInInspector] public bool shootPressed; // Has shoot been pressed
    [HideInInspector] public bool shootHeld; // Is shoot being held
    [HideInInspector] public bool shootReleased; // Has shoot been released
    [HideInInspector] public bool attackPressed; // Has attack been pressed

    [HideInInspector] public bool interacting; // Directional input for the y axis
    bool allowInteract;

    [HideInInspector] public float pauseInput;
    bool allowPauseInput = true;
    [HideInInspector] public bool pauseSelect;

    [HideInInspector] public bool goToNextDialogue;

    
    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        dialogueManager = FindObjectOfType<DialogueManager>();
    }
    // Update is called once per frame
    void Update()
    {
        switch(gameState){
            
            case 1:
                GetPlayerInput();
            break;

            case 2:
                GetMenuInput();
            break;
            
            case 3:
                GetDialogueInput();
            break;
        }
    }

    void GetPlayerInput()
    {
        //Moving
        inputX = Input.GetAxisRaw("Horizontal");
        // Dashing
        dashPressed = Input.GetButtonDown("Dash");
        dashReleased = Input.GetButtonUp("Dash");
        // Jumping
        jumpPressed = Input.GetButtonDown("Jump");
        jumpHeld = Input.GetButton("Jump");
        // Shooting
        shootPressed = Input.GetButtonDown("Shoot");
        shootHeld = Input.GetButton("Shoot");
        shootReleased = Input.GetButtonUp("Shoot");
        //Attacking
        attackPressed = Input.GetButtonDown("Attack");
        // Pause
        if (Input.GetButtonDown("Pause"))
        {
            gameState = 2; // In Menu
            gameManager.Pause(); // Pause the game
        }
        // Interaction
        if (Input.GetAxisRaw("Vertical") > 0 && !allowInteract)
            interacting = true;
        else
            interacting = false;
    }

    void GetMenuInput()
    {
        if (!allowPauseInput)
            pauseInput = 0;

        shootHeld = Input.GetButton("Shoot"); // to fix a bug with releasing the charge button while paused.

        if (Input.GetButtonDown("Pause"))
        {
            gameState = 1;
            gameManager.Unpause();
        }

        if (Input.GetAxisRaw("Vertical") != 0 && allowPauseInput)
        {
            pauseInput = Input.GetAxisRaw("Vertical"); // Get the direction pressed as a float
            allowPauseInput = false;
        }
        else if (Input.GetAxisRaw("Vertical") == 0 && !allowPauseInput)
            allowPauseInput = true;
        

        pauseSelect = Input.GetButtonDown("Jump"); // Select item from pause menu

        if (pauseSelect)
            gameState = 1;
    }

    void GetDialogueInput()
    {
        interacting = false;
           
        if (Input.GetButtonDown("Jump"))
            dialogueManager.DisplayNextSentence();
                       
    }

    public void ChangeGameState(int newGameState)
    {
        gameState = newGameState;
    }
}
