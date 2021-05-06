using UnityEngine;
using System.Collections;

public class PlayerTakeDamage : MonoBehaviour
{
    [SerializeField] PlayerHealth playerHealth; // Script that controlls the player's health
    [SerializeField] float iFrameTime = 0.75f;
    BoxCollider2D hurtBox; // Box that determines where the player can be hurt
    SpriteRenderer playerSprite;
    PlayerController playerController;
    float damage;

    private void Awake()
    {
        playerController = transform.parent.gameObject.GetComponent<PlayerController>(); // get the player controller
        playerSprite = transform.parent.gameObject.GetComponent<SpriteRenderer>();
        hurtBox = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "HurtPlayer" && !playerController.isHurt && !playerController.isDead) // Make sure the player isn't already in a hurt state
        {
            DamageInfo hurtInfo = other.GetComponent<DamageInfo>(); // Get damage info
            

            if (hurtInfo.destroyOnHit)
                Destroy(other.gameObject);

            playerController.Hurt();
            damage = hurtInfo.damage; // Get damage from the bullet
            playerHealth.RemoveHealth(damage); // damage the player

            if (playerHealth.health == 0 && !playerController.isDead)
            {
                playerController.isDead = true;
            }

            StartCoroutine(IFrames());

        }
    }

    IEnumerator IFrames()
    {

        float timeUntilEnd = Time.time + iFrameTime;
        hurtBox.enabled = false; // disable hurtbox

        while (timeUntilEnd > Time.time)
        {
            playerSprite.enabled = !playerSprite.enabled;
            yield return new WaitForFixedUpdate();
        }

        playerSprite.enabled = true;
        hurtBox.enabled = true;
    }
}
