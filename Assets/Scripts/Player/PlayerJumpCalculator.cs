using UnityEngine;

public class PlayerJumpCalculator : MonoBehaviour
{

    // Variables

    [Header("Jump Scales")]
    [SerializeField] float defaultGravityScale = 1f;
    [SerializeField] float jumpFallMultiplier = 5f; // Lowers the amount of time the jump has to stop
    [SerializeField] float jumpReleaseMultiplier = 2f; // Multiplies fall speed when jump buttons is released

    [Header("Wall Jump Variables")]
    [SerializeField] float wallJumpGravity = 1; // Gravity applied when the player is walljumping
    [SerializeField] float wallCheckDistance = 0.5f; // distance the wall detection raycast is sent
    [SerializeField] float wallJumpCheckOffset = 0.5f; // vertical offset of the raycast
    [SerializeField] float wallAttatchForce = 1f; // force applied to the player when they attatch to a wall
    [SerializeField] LayerMask mask; // mask for what the raycast can hit (ie, walls)

    bool initialWallCheck = true; // is this the initial check for a wall?
    Rigidbody2D rb2d;
    PlayerController playerController; // player controller
    InputManager inputManager;
    RaycastHit2D rightWallDetection;
    RaycastHit2D leftWallDetection;
    

    // Awake is called before starting
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>(); // Get Rigidbody component
        playerController = GetComponent<PlayerController>(); // Get Player Controller
        inputManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>(); // Get the input manager
    }

    private void Update()
    {
        // Raycasts to check for walls
        rightWallDetection = Physics2D.Raycast(transform.position + new Vector3(0.0f, wallJumpCheckOffset), Vector2.right, wallCheckDistance, mask); // Detects walls to the right of the player
        leftWallDetection = Physics2D.Raycast(transform.position + new Vector3(0.0f, wallJumpCheckOffset), Vector2.left, wallCheckDistance, mask); // Detects walls to the right of the player

        if (rightWallDetection || leftWallDetection)
            playerController.foundWall = true;
        else
            playerController.foundWall = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // If a wall has been found and player is moving in the direction of it while in the air
        if (leftWallDetection && inputManager.inputX < 0 && !playerController.isGrounded)
        {
            ApplyWallJumpPhysics();
        }
        else if (rightWallDetection && inputManager.inputX > 0 && !playerController.isGrounded)
        {
            ApplyWallJumpPhysics();
        }
        else
        {
            // Disable walljump related things
            playerController.isOnWall = false;
            
            if (rb2d.velocity.y < 0 && !playerController.isGrounded) // Jump physics
                rb2d.gravityScale = jumpFallMultiplier;

            else if (!playerController.isRebounding && (rb2d.velocity.y > 0 && !inputManager.jumpHeld && !playerController.isGrounded))
                // if player is rebounding, this will not be applied, as it will cut off the jump arc
                rb2d.gravityScale = jumpReleaseMultiplier;

            else
                rb2d.gravityScale = defaultGravityScale;
        }
    }

    void ApplyWallJumpPhysics()
    {
        
        if (initialWallCheck)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, -wallAttatchForce); // reset upward velocity when attatched to a wall
            initialWallCheck = false; // initial wall check is over
        }
        playerController.isOnWall = true; // Player is on wall
        rb2d.gravityScale = wallJumpGravity; // Apply wall jump gravity
    }

}
