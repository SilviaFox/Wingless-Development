using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] float health;

    string currentState; // Current animation state

    Animator playerAnimator;
    Rigidbody2D rb2d;

    PlayerController playerController;

    const string TEST_OBJECT_IDLE = "Test_Object_Idle";
    const string TEST_OBJECT_HURT = "Test_Object_Hurt";

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D other) // When touching a trigger object
    {
        if (other.CompareTag("Bullet") || other.CompareTag("EnemyBullet")) // Check to see if the object is a bullet
        {
            health -= 10; // if it is a bullet, remove health [FIX: ADD BULLET SPECIFIC DAMAGE]
            ChangeAnimationState(TEST_OBJECT_HURT);  // Play hurt animation
        }
        else if (other.CompareTag("PlayerMelee"))
        {
            health -= 10; // if it is a bullet, remove health [FIX: ADD ATTACK SPECIFIC DAMAGE]
            rb2d.AddForce(playerController.attackForce, ForceMode2D.Impulse);
            ChangeAnimationState(TEST_OBJECT_HURT);  // Play hurt animation
        }

        if (health <= 0) // if health is less than or equal to 0
        {
            Destroy(gameObject); // Kill enemy
        }

        Invoke("ReturnToIdleAnimation", 0.1f); // reset to idle animation after hit
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
