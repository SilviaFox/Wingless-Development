using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] int gameState = 1; // State the player is in.
    // 1 - Player can be controlled
    // 2 - In Pause Menu
    // 3 - In Dialogue Menu
    InputMaster controls;

    GameObject player;
    PlayerController playerController;
    Shooting shootingScript;
    MeleeSystem meleeSystem;
    DialogueManager dialogueManager;
    [HideInInspector] public DialogueTrigger dialogueTrigger;

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

    #region Enable Or Disable Input
        private void OnEnable() {
            controls.Player.Disable();
            controls.Player.Enable();
        }

        private void Pause() {
            controls.Player.Disable();
            controls.Pause.Enable();
            gameManager.Pause();
        }

        private void Unpause() {
            controls.Pause.Disable();
            controls.Player.Enable();
            gameManager.Unpause();
        }

        private void PauseSelect() {
            controls.Pause.Disable();
            gameManager.PauseSelect();
        }

        private void Dialogue() {
            controls.Player.Disable();
            controls.Dialogue.Enable();
        }

        public void EndOfDialogue() {
            controls.Dialogue.Disable();
            controls.Player.Enable();
        }


    #endregion


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        shootingScript = player.GetComponent<Shooting>();
        playerController = player.GetComponent<PlayerController>();
        meleeSystem = player.GetComponent<MeleeSystem>();
        dialogueManager = GetComponent<DialogueManager>();

        gameManager = GetComponent<GameManager>();

        // New Input
        controls = new InputMaster();
        #region Gameplay Input
            // Jumping
            controls.Player.Jump.started += ctx => playerController.RecieveJumpInput();
            controls.Player.Jump.started += ctx => jumpHeld = true;
            controls.Player.Jump.canceled += ctx => jumpHeld = false;
            // Shooting
            controls.Player.Fire.started += ctx => shootingScript.InitialShot();
            controls.Player.Fire.started += ctx => shootHeld = true;
            controls.Player.Fire.canceled += ctx => shootingScript.ReleaseShot();
            controls.Player.Fire.canceled += ctx => shootHeld = false;
            // Attacking
            controls.Player.Attack.started += ctx => meleeSystem.Attack();
            // Dashing
            controls.Player.Dash.started += ctx => playerController.InitializeDash();
            controls.Player.Dash.started += ctx => dashReleased = false;
            controls.Player.Dash.canceled += ctx => dashReleased = true;
            // Interaction
            controls.Player.Interact.started += ctx => CheckForItem();

            // Pause Menu
            controls.Player.Pause.started += ctx => Pause();
            controls.Pause.Unpause.started += ctx => Unpause();
            controls.Pause.Select.started += ctx => PauseSelect();

            // Dialogue
            controls.Dialogue.Next.started += ctx => dialogueManager.DisplayNextSentence();

        #endregion

        
    }
    // Update is called once per frame
    void Update()
    {
        inputX = controls.Player.Move.ReadValue<Vector2>().x; // Get movement input

        if (shootHeld)
        {
            shootingScript.HoldShot();
        }


        if (!allowPauseInput) // if pause input is currently not allowed
            pauseInput = 0; // set it to 0

        if (controls.Pause.Scroll.ReadValue<Vector2>().y != 0 && allowPauseInput) { // if pause input is allowed and up or down is pressed
            pauseInput = controls.Pause.Scroll.ReadValue<Vector2>().y; // pause input = dpad/key input
            allowPauseInput = false; // do not allow more inputs until key/pad is released
        }
        else if (controls.Pause.Scroll.ReadValue<Vector2>().y == 0 && !allowPauseInput)
            allowPauseInput = true;
    }

    void CheckForItem() {
        // Check if there is a dialogue trigger
        if (dialogueTrigger != null) {
            dialogueTrigger.TriggerDialogue(); // Trigger dialogue
            Dialogue();
        }
    }

    public void ChangeGameState(int newGameState)
    {
        gameState = newGameState;
    }
}
