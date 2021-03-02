using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float timeUntilFire; // time until the next time the player fires, set before running
    [SerializeField] float damage; // Damage that will be dealt when shooting the player

    float nextFireTime; // The active version of the above

    [HideInInspector] public float direction = 1; // -1 when left, 1 when right
    [HideInInspector] public bool canSeePlayer = false; // true when player has been spotted

    string currentState; // Current animation state

    [SerializeField] GameObject bullet;

    Animator playerAnimator;
    Rigidbody2D rb2d;
    BulletCounter bulletCounter;
    SpriteRenderer spriteRenderer;
    PlayerController playerController;

    const string TEST_OBJECT_IDLE = "Test_Object_Idle";
    const string TEST_OBJECT_HURT = "Test_Object_Hurt";

    private void Awake()
    {
        nextFireTime = timeUntilFire;
        playerAnimator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bulletCounter = GameObject.FindGameObjectWithTag("BulletManager").GetComponent<BulletCounter>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {

        if (canSeePlayer && Time.time >= nextFireTime) // If time is above the cooldown
        {
            nextFireTime = Time.time + timeUntilFire; // Reset timer
            Instantiate(bullet, transform.position, transform.rotation).GetComponent<MoveEnemyBullet>().GetEnemyInfo(this, damage); // Create bullet and set the "EnemyScript" argument to this script
        }
    }

    public void FaceDirectionOfPlayer(Vector2 playerPos)
    {
        if (playerPos.x > transform.position.x) // If player is to the right of the enemy
        {
            direction = 1;
            spriteRenderer.flipX = false;
        }
        else // If player is to the left of the enemy
        {
            direction = -1;
            spriteRenderer.flipX = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) // When touching a trigger object
    {

        if (other.CompareTag("Bullet")) // Check to see if the object is a bullet
        {
            Destroy(other.gameObject); // Destroy Bullet
            bulletCounter.DecreaseBulletAmount(); // Decrease bullet counter
            
            TakeDamage(other.GetComponent<MoveBullet>().damage);
            ChangeAnimationState(TEST_OBJECT_HURT);  // Play hurt animation
        }
        else if (other.CompareTag("PlayerMelee"))
        {
            // Take damage and add force
            TakeDamage(playerController.attackDamage);
            rb2d.AddForce(playerController.attackForce + new Vector2(0.0f, -rb2d.velocity.y), ForceMode2D.Impulse);

            ChangeAnimationState(TEST_OBJECT_HURT); // Play hurt animation
        }

        if (health <= 0) // if health is less than or equal to 0
        {
            Destroy(gameObject); // Kill enemy
        }

        Invoke("ReturnToIdleAnimation", 0.1f); // reset to idle animation after hit
    }

    void TakeDamage(float damage)
    {
        health -= damage;
    }

    void ReturnToIdleAnimation() // Resets back to idle animation
    {
        ChangeAnimationState(TEST_OBJECT_IDLE);
    }

    void ChangeAnimationState(string newState) // Change Animation states
    {

        //stop the same animation from interrupting itself
        if (currentState == newState) return;
        // Play the animation
        playerAnimator.Play(newState);

        // Reassign the current state
        currentState = newState;
    }
}
