using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] float health;
    float playerDirection = 0;

    SpriteAnimator spriteAnimator;
    Rigidbody2D rb2d;

    PlayerController playerController;

    const string TEST_OBJECT_IDLE = "Test_Object_Idle";
    const string TEST_OBJECT_HURT = "Test_Object_Hurt";

    private void Awake()
    {
        spriteAnimator = GetComponent<SpriteAnimator>();
        rb2d = GetComponent<Rigidbody2D>();

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D other) // When touching a trigger object
    {
        if (other.CompareTag("Bullet") || other.CompareTag("EnemyBullet")) // Check to see if the object is a bullet
        {
            health -= 10; // if it is a bullet, remove health [FIX: ADD BULLET SPECIFIC DAMAGE]
            spriteAnimator.ChangeAnimationState(TEST_OBJECT_HURT, 0);  // Play hurt animation
        }
        else if (other.CompareTag("PlayerMelee"))
        {
            if (playerController.isFacingLeft)
                playerDirection = -1;
            else
                playerDirection = 1;

            health -= 10; // if it is a bullet, remove health [FIX: ADD ATTACK SPECIFIC DAMAGE]

            Vector2 force = new Vector2(playerController.attackForce.x * playerDirection, playerController.attackForce.y);
            rb2d.AddForce(force + new Vector2(0.0f, -rb2d.velocity.y), ForceMode2D.Impulse);

            if (!playerController.isGrounded)
            {
                playerController.AirRebound();
                // Call Air Rebound Function
            }

            spriteAnimator.ChangeAnimationState(TEST_OBJECT_HURT, 0);  // Play hurt animation
        }

        if (health <= 0) // if health is less than or equal to 0
        {
            Destroy(gameObject); // Kill enemy
        }

        Invoke("ReturnToIdleAnimation", 0.1f); // reset to idle animation after hit
    }

    void ReturnToIdleAnimation() // Resets back to idle animation
    {
        spriteAnimator.ChangeAnimationState(TEST_OBJECT_IDLE, 0);
    }

}
