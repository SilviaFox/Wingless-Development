using UnityEngine;

public class PlayerJumpCalculator : MonoBehaviour
{

    // Variables

    [Header("Jump Scales")]
    [SerializeField] float defaultGravityScale = 1f;
    [SerializeField] float jumpFallMultiplier = 5f; // Lowers the amount of time the jump has to stop
    [SerializeField] float jumpReleaseMultiplier = 2f; // Multiplies fall speed when jump buttons is released

    PlayerController playerController; // player controller
    InputManager inputManager;
    

    // Awake is called before starting
    void Start()
    {
        playerController = PlayerController.current;
        inputManager = InputManager.instance; // Get the input manager
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // If a wall has been found and player is moving in the direction of it while in the air
        
        if (PlayerController.rb2d.velocity.y < 0 && !playerController.isGrounded) // Jump physics
            PlayerController.rb2d.gravityScale = jumpFallMultiplier;

        else if (!playerController.isRebounding && (PlayerController.rb2d.velocity.y > 0 && !inputManager.jumpHeld && !playerController.isGrounded))
            // if player is rebounding, this will not be applied, as it will cut off the jump arc
            PlayerController.rb2d.gravityScale = jumpReleaseMultiplier;

        else
            PlayerController.rb2d.gravityScale = defaultGravityScale;
    
    }

}
