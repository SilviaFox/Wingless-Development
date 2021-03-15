using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    #region Variables
    // Define components
    Rigidbody2D rb2d;
    SpriteRenderer playerSprite;
    SpriteAnimator spriteAnimator;
    BoxCollider2D playerCollider;
    ObjectAudioManager playerAudioManager; // Audio Manager for playing local player sounds
    DeathScript deathScript; // When health is 0, trigger this script
    PlayerHealth playerHealth; // for getting the amount of health the player has
    InputManager inputManager;

    //=================
    // Define Variables
    //=================

    Vector2 movement; // Movement for when player moves on the x axis
    float inputX; // Horizontal input
    
    // Variables for movement
    [Header("Movement Variables")]
    [SerializeField] float moveSpeed = 5f; // Player's speed while moving
    [SerializeField] float jumpForce = 5f; // Force of the player's jump
    [SerializeField] float wallJumpForce = 4f; // Force of a player's wall jump
    [SerializeField] float groundedForce = 5f; // Force applied constantly when on the ground to make sure the player stays on the ground
    [SerializeField] float slopeCounterForce = 2; // Applied multiple that is taken away from the current grounded force
    [SerializeField] float maxDownwardVelocity = 5; // Maximum downward speed the player can travel in the air
    [SerializeField] float airSmoothingAmount = 1.5f; // Instead of setting speed to 0, we divide it by a certain amount to give a smoother feel
    [SerializeField] float velocityThreshold = 0.5f; // Once x velocity is below this number it is set back to 0

    [SerializeField] float wallJumpMoveCooldown = 0.5f; // cooldown before player can move again after walljumping
    float nextWallJumpTime = 0;

    bool landed;
    bool jumpRequest; // when true, request a jump

    [Header("Physics Materials")]
    [SerializeField] PhysicsMaterial2D idleMaterial;
    [SerializeField] PhysicsMaterial2D movingMaterial;

    // Variables for Jumping and ground checking
    [Header("Grounded Check Variables")]
    [SerializeField] float groundedSkin = 0.05f;
    [SerializeField] LayerMask groundedMask;

    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isOnWall; // true if the player is able to wall jump
    float subtractFromBoxSize = 0.05f;

    Vector2 playerSize; // Size of the player collider
    Vector2 boxCenter; // Center of the player collider
    Vector2 colliderOffset; // offset of the player collider
    Vector2 boxSize; // size of the ground detection box
    
    // Variables for dashing
    [Header("Dash Variables")]
    [SerializeField] float dashRate; // Directly affects nextDashTime
    [SerializeField] float dashSpeed; // Speed of the dash
    [SerializeField] float dashLength; // Directly affects currentDashTime
    [SerializeField] float dashSmoothingTime = 0.5f;
    [SerializeField] float dashEndMinspeed = 4;
    float newDashSmoothingTime = 0;
    float decreasingDashSpeed;

    Ghost dashGhosts;
    AudioSource dashSound;
    int dashStage = 1; // Stage of the dash, 1 = Initial Dash, 2 = Allow dash jump
    float nextDashTime; // Time before next dash
    float currentDashLength; // Time until dash ends
    bool isDashing = false;  // Is player dashing?
    bool allowDashSound = false; // allows dash sound to play
    bool dashHasEnded = true;

    [Header("Damage Variables")]
    // Taking damage or getting hurt
    [SerializeField] float HurtLength;
    [SerializeField] float knockbackSpeed;

    [HideInInspector] public bool isHurt; // While in the damaged state, this is true

    [HideInInspector] public bool isDead; // Player is dead when this is 0
    float endHurtTime = 0;
    float hurtDirection;

    [Header("Landing Particles")]
    // Create Landing Particles
    [SerializeField] GameObject landingParticles;
    [SerializeField] float landingParticleOffset = -0.5f;

    [HideInInspector] public bool isShooting = false;
    [HideInInspector] public bool isFacingLeft = false;

    // Attacking variables
    [Header("Attack Variables")]
    [SerializeField] BoxCollider2D attackCollider;
    [SerializeField] float airReboundStrength = 10f;

    [HideInInspector] public bool isAttacking = false;
    float attackEndTime;
    Vector2 attackColliderOffset;
    Vector2 leftAttackColliderOffset;
    [HideInInspector] public Vector2 attackForce;
    [HideInInspector] public float attackDamage;
    [HideInInspector] public bool attackingInAir = false;
    [HideInInspector] public bool isRebounding;
    bool groundedAttack;

    // Animation States

    // Standard States
    const string PLAYER_IDLE = "Player_Idle";
    const string PLAYER_RUN = "Player_Run";
    const string PLAYER_JUMP = "Player_Jump";
    const string PLAYER_FALL = "Player_Fall";
    const string PLAYER_HURT = "Player_Hurt";

    // Shooting Variants
    const string PLAYER_IDLE_SHOOT = "Player_Idle_Shoot";


    #endregion

    #region Start/Defining Variables
    // Start is called before the first frame update
    void Awake()
    {
        // Get components

        // Physics
        rb2d = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();

        // Visuals
        playerSprite = GetComponent<SpriteRenderer>();
        spriteAnimator = GetComponent<SpriteAnimator>();
        dashGhosts = GetComponent<Ghost>();

        // Ground Detection
        playerSize = playerCollider.size;
        colliderOffset = playerCollider.offset;
        boxSize = new Vector2(playerSize.x - subtractFromBoxSize, groundedSkin);

        // Audio
        playerAudioManager = GameObject.FindGameObjectWithTag("PlayerAudio").GetComponent<ObjectAudioManager>();

        // Gameplay

        // Input
        inputManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        
        // Health
        playerHealth = GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<PlayerHealth>();

        // Attacks
        attackColliderOffset = attackCollider.offset;
        leftAttackColliderOffset = -attackColliderOffset;

    }

    #endregion


    // Update is called once per frame
    void Update()
    {
        if(rb2d.velocity.x >= moveSpeed && !isDashing) // If velocity is surpassing move speed
        {
            rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y); // reset velocity back to movespeed
        }
        else if(rb2d.velocity.x <= -moveSpeed && !isDashing) // Same for moving left
        {
            rb2d.velocity = new Vector2(-moveSpeed, rb2d.velocity.y);
        }

        isFacingLeft = playerSprite.flipX;
        if (isFacingLeft)
            attackCollider.offset = leftAttackColliderOffset;
        else
            attackCollider.offset = attackColliderOffset;
        
        if (!jumpRequest && !isRebounding)
            // Check to see if player is grounded   
            isGrounded = Physics2D.OverlapBox(boxCenter + colliderOffset, boxSize, 0f, groundedMask) != null;

        
        if (!isHurt && !isDead) // If not in hurt state and not dead
            InputAndResponse();

        else if(Time.time >= endHurtTime)
        {
            isHurt = false;
        }

        if (isGrounded && !landed)
        {
            OnLandEvent();
        }
        else if (!isGrounded)
        {
            landed = false;
        }


        CapDownwardVelocity();

        if (rb2d.velocity.y >= 0 && !isGrounded)
            isGrounded = false;

        
    }

    void OnLandEvent()
    {
        landed = true;
        
        isAttacking = false;
        Instantiate(landingParticles, new Vector3(transform.position.x, transform.position.y + landingParticleOffset), transform.rotation);
    }

    void CapDownwardVelocity()
    {
        if (!isGrounded && rb2d.velocity.y < -maxDownwardVelocity)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, -maxDownwardVelocity);
        }
    }

    public void InitializeDash() {
        if (movement.x != 0 && (dashStage == 1 || dashStage == 2 || dashStage == 8) && Time.time >= nextDashTime && !isHurt)
        {
            if (isGrounded) // Initiate the dash if on the ground
            {
                dashHasEnded = false;
                // This will activate the Dash() function
                isDashing = true;

                // Make Ghosts
                dashGhosts.makeGhost = true;

                // Reset the timers
                nextDashTime = Time.time + 1f / dashRate;
                currentDashLength = Time.time + dashLength;
            }
            // Allow for dash jumping and play Dash sound
            if(dashStage == 1 && isGrounded)
            {
                dashStage += 1;
                allowDashSound = true;
            }
            else
                dashStage = 0;
        }
        
    }

    private void InputAndResponse() // all things to do with pressing buttons and making the player react are here
    {
        //
        // Running
        //
        inputX = inputManager.inputX;

        if (Time.time >= nextWallJumpTime)
        {  
            movement.x = inputX * moveSpeed;
        }
        else
            movement.x = 0;
        
        if (!isGrounded && !isAttacking && rb2d.velocity.y > 0)
            spriteAnimator.ChangeAnimationState(PLAYER_JUMP);
        else if (!isGrounded && !isAttacking && rb2d.velocity.y < 0)
            spriteAnimator.ChangeAnimationState(PLAYER_FALL);

        switch(movement.x)
        {
            case 0:
                rb2d.sharedMaterial = idleMaterial; // Make sure the player doesn't slide when idle
                if (!isShooting && !isAttacking && isGrounded)
                    spriteAnimator.ChangeAnimationState(PLAYER_IDLE); // Play idle animation
                else if (!isAttacking && isGrounded)
                    spriteAnimator.ChangeAnimationState(PLAYER_IDLE_SHOOT); // Play idle shoot animation
            break;
            default:
                
                rb2d.sharedMaterial = movingMaterial; // Remove friction from the player
                if (!isAttacking && isGrounded)
                    spriteAnimator.ChangeAnimationState(PLAYER_RUN); // Play run animation
                

                if (movement.x < 0)
                playerSprite.flipX = true; // Flip the player sprite if moving left
                else if (movement.x > 0)
                playerSprite.flipX = false; // Keep the player facing right if otherwise

            break;
        }

        if(rb2d.velocity.y < 0)
            isRebounding = false;

        // End Dash
        if (isHurt)
        {
            EndDash();
        }
        else if (isGrounded && (inputManager.dashReleased || Time.time >= currentDashLength || dashStage == 0 || movement.x == 0))
        {
            EndDash();
        }

    }

    public void RecieveJumpInput() {
        if (isGrounded || isOnWall)
        {
            isAttacking = false;
            jumpRequest = true;
            dashStage *= 4;
        }
    }

    // Physics are used in Fixed Update
    private void FixedUpdate()
    {

        //=====================
        //======MOVEMENT=======
        //=====================

        // Setup ground for detection
        boxCenter = (Vector2)transform.position + Vector2.down * (playerSize.y + boxSize.y) * 0.5f;

        if (jumpRequest)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0.0f); // if jump request has been sent, reset y velocity
        }

        if (Time.time >= nextWallJumpTime && !isDead)
            Move();

        // Dash
        if (isDashing)
        Dash();

        #region Jump

        if (jumpRequest)
        {
            if (!isOnWall)
                rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Jump if jump button is pressed
            else
            {
                // Reset the timer
                rb2d.AddForce(Vector2.up * jumpForce + new Vector2(-movement.x * wallJumpForce, 1), ForceMode2D.Impulse); // Walljump if jump button is pressed
                nextWallJumpTime = Time.time + wallJumpMoveCooldown;
            }
            
            jumpRequest = false;
            isGrounded = false;
            
        }
        
        #endregion
   
        if (isHurt)
        {
            rb2d.velocity = new Vector2(hurtDirection * knockbackSpeed, rb2d.velocity.y); // apply knockback
        }

        if (Time.time < newDashSmoothingTime && isGrounded && !dashHasEnded)
        {
            decreasingDashSpeed -= Time.deltaTime * 2;
            if (decreasingDashSpeed <= dashEndMinspeed)
            {
                dashHasEnded = true;
            }
            rb2d.velocity = new Vector2(rb2d.velocity.x * decreasingDashSpeed, rb2d.velocity.y);
            
        }
    }

    void Move()
    {
        // Set the current grounded force
        float currentGroundedForce = groundedForce;
        if (rb2d.velocity.y > 0 && !isDashing) // if going up a slope
            currentGroundedForce -= rb2d.velocity.y * slopeCounterForce;
        else if (rb2d.velocity.y < 0)
            currentGroundedForce += -rb2d.velocity.y;

        if (movement.x != 0) // If player is moving
            rb2d.velocity = new Vector2(movement.x, rb2d.velocity.y); // Move player if left or right is pressed
        else if ((rb2d.velocity.x > 0 && rb2d.velocity.x <= velocityThreshold) || (rb2d.velocity.x < 0 && rb2d.velocity.x >= -velocityThreshold))
            rb2d.velocity = new Vector2(0.0f, rb2d.velocity.y);
        else if (!isGrounded) // If player is not moving and is in the air
            rb2d.velocity = new Vector2(rb2d.velocity.x * airSmoothingAmount, rb2d.velocity.y);
        

        if (!inputManager.jumpHeld && isGrounded && !isRebounding)
             rb2d.velocity = new Vector2(rb2d.velocity.x, -currentGroundedForce); // apply downward force to stop player from flying off of slopes   
    }

    void Dash()
    {
        decreasingDashSpeed = dashSpeed;
        rb2d.velocity = new Vector2(rb2d.velocity.x * dashSpeed, rb2d.velocity.y);
        
        if (allowDashSound)
        {
            playerAudioManager.Play("Dash");
            allowDashSound = false;
        }
    }

    void EndDash()
    {
        if (!dashHasEnded)
            newDashSmoothingTime = Time.time + dashSmoothingTime; // apply smoothing on end of dash

        isDashing = false;
        dashGhosts.makeGhost = false;
        dashStage = 1;
    }

    public void Hurt()
    {
        EndDash();

        rb2d.sharedMaterial = movingMaterial;

        spriteAnimator.ChangeAnimationState(PLAYER_HURT);
        playerAudioManager.Play("Hurt");

        if (playerSprite.flipX) // detect the direction the player is facing and change the hurt direction accordingly
            // Hurt direction should be the opposite of what the player's facing direction is
            hurtDirection = 1;
        else
            hurtDirection = -1;
            
        isHurt = true;
        endHurtTime = Time.time + HurtLength;
    }

    public void Attack(string attackAnimation, float currentAttackDamage, float attackTime, Vector2 currentAttackForce, bool isAttackGrounded)
    {
        spriteAnimator.ChangeAnimationState(attackAnimation);

        groundedAttack = isAttackGrounded;
        attackForce = currentAttackForce; // Get force
        attackDamage = currentAttackDamage; // Get damage

        isAttacking = true;
        StartCoroutine(Attacking(attackTime));
        playerAudioManager.Play("Attack1");
    }

    IEnumerator Attacking(float duration)
    {
        float normalizedTime = 0; // time since coroutine started
        

        while (normalizedTime <= duration && isAttacking)
        {
            normalizedTime += Time.deltaTime / duration; // count up every loop (time.deltatime is the time between each frame)

            if (groundedAttack && !isGrounded && rb2d.velocity.y > 0)
            {
                normalizedTime += 100;
            }

            yield return null;
        }
        
        // End attack once while loop is finished
        isAttacking = false;
    }

    public void AirRebound()
    {
        isAttacking = false;
        isRebounding = true;
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0.0f);
        rb2d.AddForce(new Vector2(0.0f, airReboundStrength), ForceMode2D.Impulse);
    }
    


}
