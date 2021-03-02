using UnityEngine;

public class PlayerJumpCalculator : MonoBehaviour
{

    // Variables

    [Header("Jump Scales")]
    [SerializeField] float defaultGravityScale = 1f;
    [SerializeField] float jumpFallMultiplier = 5f; // Lowers the amount of time the jump has to stop
    [SerializeField] float jumpReleaseMultiplier = 2f; // Multiplies fall speed when jump buttons is released

    [Header("Wall Jump Variables")]
    [SerializeField] float wallJumpGravity = 1;
    [SerializeField] float wallCheckDistance = 0.5f;
    [SerializeField] float wallJumpCheckOffset = 0.5f;
    [SerializeField] float wallAttatchForce = 1f;
    [SerializeField] LayerMask mask;

    bool initialWallCheck = true;
    Rigidbody2D rb2d;
    PlayerController playerController; // player controller
    RaycastHit2D rightWallDetection;
    RaycastHit2D leftWallDetection;

    // Awake is called before starting
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        // Raycasts to check for walls
        rightWallDetection = Physics2D.Raycast(transform.position + new Vector3(0.0f, -wallJumpCheckOffset), Vector2.right, wallCheckDistance, mask); // Detects walls to the right of the player
        leftWallDetection = Physics2D.Raycast(transform.position + new Vector3(0.0f, -wallJumpCheckOffset), Vector2.left, wallCheckDistance, mask); // Detects walls to the right of the player
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Wall Jump detection
        if (leftWallDetection && Input.GetAxisRaw("Horizontal") < 0 && !playerController.isGrounded)
        {
            ApplyWallJumpPhysics();
        }
        else if (rightWallDetection && Input.GetAxisRaw("Horizontal") > 0 && !playerController.isGrounded)
        {
            ApplyWallJumpPhysics();
        }
        else
        {
            playerController.isOnWall = false;
            
            if (rb2d.velocity.y < 0) // Jump physics
            {
                rb2d.gravityScale = jumpFallMultiplier;
            }
            else if (rb2d.velocity.y > 0 && !Input.GetButton("Jump") && !playerController.isGrounded)
            {
                rb2d.gravityScale = jumpReleaseMultiplier;
            }
            else {
                rb2d.gravityScale = defaultGravityScale;
            }
        }
    }

    void ApplyWallJumpPhysics()
    {
        // reset upward velocity when attatched to a wall
        if (initialWallCheck)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, -wallAttatchForce);
            initialWallCheck = false;
        }

        playerController.isOnWall = true;
        rb2d.gravityScale = wallJumpGravity;
    }

}
