using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyLogic : MonoBehaviour
{
    public float health;
    [SerializeField] bool invincible;

    [SerializeField] GameObject[] coins;
    [SerializeField] bool affectedByForce;

    [Space]

    [SerializeField] UnityEvent onDamage;

    int playerDirection;

    BulletCounter bulletCounter;
    PlayerController playerController;
    DamageNumbers damageNumbers;
    ObjectAudioManager playerAudioManager;
    EnemyGetAffectedByForce forceScript;

    private void Start()
    {
        playerController = PlayerController.current;
        playerAudioManager = PlayerController.playerAudioManager;
        damageNumbers = FindObjectOfType<DamageNumbers>();
        bulletCounter = FindObjectOfType<BulletCounter>();

        if (affectedByForce)
            forceScript = gameObject.AddComponent<EnemyGetAffectedByForce>() as EnemyGetAffectedByForce;
    }

    private void OnTriggerEnter2D(Collider2D other) // When touching a trigger object
    {

        if (other.CompareTag("Bullet")) // Check to see if the object is a bullet
        {
            if (!other.GetComponent<MoveBullet>().canGoThroughEnemies) // if bullet cannot go through enemies
                {
                    Destroy(other.gameObject); // Destroy Bullet
                    bulletCounter.DecreaseBulletAmount(); // Decrease bullet counter
                }
            else
                other.GetComponent<MoveBullet>().GoThroughEnemy();
            
            TakeDamage(other.GetComponent<MoveBullet>().damage);
        }
        else if (other.CompareTag("PlayerMelee"))
        {
            // Take damage and add force
            TakeDamage(playerController.attackDamage);

            if (playerController.isFacingLeft)
                playerDirection = -1;
            else
                playerDirection = 1;

            if (affectedByForce)
                forceScript.PushEnemy(playerController.attackForce.x, playerController.attackForce.y, playerDirection);

            if (!playerController.isGrounded)
            {
                playerController.AirRebound();
                // Call Air Rebound Function
            }
        }

        if (health <= 0 && !invincible) // if health is less than or equal to 0
        {
            for (int i = 0; i < coins.Length; i++)
            {
                Instantiate(coins[i], transform.position, transform.rotation);
            }
            Destroy(gameObject); // Kill enemy
        }
    }

    public void SetHealth(float newHealth)
    {
        health = newHealth;
    }

    // private void OnDestroy()
    // {
    //     bulletCounter.DecreaseBulletAmount(); // Decrease bullet counter
    // }

    void TakeDamage(float damage)
    {
        onDamage.Invoke();
        health -= damage;
        damageNumbers.OnHit(transform.position, damage);
        playerAudioManager.Play("Hitsound");
    }

}
