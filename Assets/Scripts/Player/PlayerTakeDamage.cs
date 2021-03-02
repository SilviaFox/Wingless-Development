using UnityEngine;

public class PlayerTakeDamage : MonoBehaviour
{
    [SerializeField] PlayerHealth playerHealth; // Script that controlls the player's health
    PlayerController playerController;
    float damage;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>(); // get the player controller
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyBullet" && !playerController.isHurt && !playerController.isDead) // Make sure the player isn't already in a hurt state
        {
            Destroy(other.gameObject);
            playerController.Hurt();
            damage = other.GetComponent<MoveEnemyBullet>().damage; // Get damage from the bullet
            playerHealth.RemoveHealth(damage); // damage the player

            if (playerHealth.health == 0 && !playerController.isDead)
            {
                playerController.isDead = true;
            }
        }
    }
}
