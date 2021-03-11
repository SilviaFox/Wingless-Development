using UnityEngine;

public class InputManager : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] int gameState = 1; // State the player is in.
    // 1 - Player can be controlled
    // 2 - In Pause Menu

    [HideInInspector] public float inputX; // Directional input for the x axis
    [HideInInspector] public bool dashPressed; // Has dash been pressed
    [HideInInspector] public bool dashReleased; // Has dash been released
    [HideInInspector] public bool jumpPressed; // Has jump been pressed
    [HideInInspector] public bool jumpHeld; // Is jump being held
    [HideInInspector] public bool shootPressed; // Has shoot been pressed
    [HideInInspector] public bool shootHeld; // Is shoot being held
    [HideInInspector] public bool shootReleased; // Has shoot been released
    [HideInInspector] public bool attackPressed; // Has attack been pressed

    [HideInInspector] public float pauseInput;
    [HideInInspector] public bool pauseSelect;

    
    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
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

    }

    void GetMenuInput()
    {
        shootHeld = Input.GetButton("Shoot"); // to fix a bug with releasing the charge button while paused.

        if (Input.GetButtonDown("Pause"))
        {
            gameState = 1;
            gameManager.Unpause();
        }

        if (Input.GetButtonDown("Vertical")) // On the initial press of up or down
        {
            pauseInput = Input.GetAxisRaw("Vertical"); // Get the direction pressed as a float
        }
        else
        {
            pauseInput = 0; // Reset after button isn't down
        }

        pauseSelect = Input.GetButtonDown("Jump");

        if (pauseSelect)
            gameState = 1;
    }
}
