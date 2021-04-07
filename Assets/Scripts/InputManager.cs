using UnityEngine;

public class InputManager : MonoBehaviour
{
    GameManager gameManager;
    public InputMaster controls;

    GameObject player;
    PlayerController playerController;
    Shooting shootingScript;
    MeleeSystem meleeSystem;
    DialogueManager dialogueManager;
    [HideInInspector] public DialogueTrigger dialogueTrigger;

    [HideInInspector] public float inputX; // Directional input for the x axis
    [HideInInspector] public bool dashReleased; // Has dash been released
    [HideInInspector] public bool jumpPressed; // Has jump been pressed
    [HideInInspector] public bool jumpHeld; // Is jump being held
    [HideInInspector] public bool shootHeld; // Is shoot being held

    [HideInInspector] public bool interacting; // Directional input for the y axis
    bool allowInteract;

    [HideInInspector] public float pauseInput;

    [HideInInspector] public bool goToNextDialogue;

    #region Enable Or Disable Input

        public void DisableAll() { // Disable all of the Action Maps
            controls.Disable();
        }

        private void OnEnable() { // On enable, reset the Player Controls
            controls.Player.Disable();
            controls.Player.Enable();
        }

        private void Pause() { // Pause the game, disable player controls, enable pause controls
            Debug.Log("Game is Paused");
            controls.Player.Disable();
            controls.Pause.Enable();
            gameManager.Pause();
        }
        

        public void Unpause() { // Unpause the game, enable player controls, disable pause controls
            controls.Pause.Disable();
            controls.Player.Enable();
        }

        public void EscUnpause() { // Unpause using the escape key
            controls.Pause.Disable();
            controls.Player.Enable();
            gameManager.Unpause();
        }

        private void Dialogue() { // Dialogue input
            controls.Player.Disable();
            controls.Dialogue.Enable();
        }

        public void EndOfDialogue() { // When dialogue has ended
            controls.Dialogue.Disable();
            controls.Player.Enable();
        }


    #endregion


    private void Awake()
    {
        // Get components
        gameManager = GetComponent<GameManager>();
        
        // Player Components
        player = GameObject.FindGameObjectWithTag("Player");
        shootingScript = player.GetComponent<Shooting>();
        playerController = player.GetComponent<PlayerController>();
        meleeSystem = player.GetComponent<MeleeSystem>();

        // Dialogue manager
        dialogueManager = GetComponent<DialogueManager>();
        
        controls = new InputMaster();
                

            #region Gameplay Input
                // Jumping
                controls.Player.Jump.started += ctx => {playerController.RecieveJumpInput(); jumpHeld = true;};
                controls.Player.Jump.canceled += ctx => jumpHeld = false;
                // Shooting
                controls.Player.Fire.started += ctx => {shootingScript.InitialShot(); shootHeld = true;};
                controls.Player.Fire.canceled += ctx => {shootingScript.ReleaseShot(); shootHeld = false;};
                // Attacking
                controls.Player.Attack.started += ctx => meleeSystem.Attack();
                // Dashing
                controls.Player.Dash.started += ctx => {
                    playerController.InitializeDash();
                    dashReleased = false;
                };
                controls.Player.Dash.canceled += ctx => dashReleased = true;
                // Interaction
                controls.Player.Interact.started += ctx => CheckForItem();

                // Pause Menu
                controls.Player.Pause.started += ctx => Pause();
                controls.Pause.Unpause.started += ctx => EscUnpause();

                // Dialogue
                controls.Dialogue.Next.started += ctx => dialogueManager.OnButtonPressed();
                controls.Dialogue.Next.canceled += ctx => dialogueManager.OnButtonReleased();

            #endregion
        

        
    }
    // Update is called once per frame
    void Update()
    {
        inputX = controls.Player.Move.ReadValue<Vector2>().x; // Get movement input

        if (shootHeld)
            shootingScript.HoldShot();
        
    }

    void CheckForItem() {
        // Check if there is a dialogue trigger
        if (dialogueTrigger != null) {
            dialogueTrigger.TriggerDialogue(); // Trigger dialogue
            Dialogue();
        }
    }
}
