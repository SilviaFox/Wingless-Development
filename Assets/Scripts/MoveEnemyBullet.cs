using UnityEngine;

public class MoveEnemyBullet : MonoBehaviour
{

    [SerializeField] float bulletSpeed; // speed of bullets
    [SerializeField] float bulletDespawnTime = 0.5f; // time it takes for bullets to despawn
    [SerializeField] float bulletDespawnRate = 0.1f; // speed at which bullets will despawn
    [HideInInspector] public float damage;

    bool bulletOffscreen; // If true, the bullet's despawn countdown should start;
    float direction;

    Rigidbody2D rb2d;
    Vector2 startPos;

    public void GetEnemyInfo(EnemyScript enemyScript, float enemyDamage) // This is triggered when the bullet is instantiated
    {
        direction = enemyScript.direction; // Get direction of the enemy
        // transform.position = enemyScript.gameObject.transform.position; // Get the position of the enemy
        damage = enemyDamage; // Damage equals
    }

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    

    // When no longer visible on camera
    #region Bullets visibility on camera 
    private void OnBecameInvisible()
    {
         bulletOffscreen = true;
    }

    private void OnBecameVisible()
    {
        bulletOffscreen = false;
    }

    #endregion

    private void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + Vector2.right * bulletSpeed * direction); // New position is the current one, plus a direction, times by speed and direction

        // Countdown bullet's despawn time and then delete it when it reaches 0 if it is offscreen
        if (bulletOffscreen)
        {
            if (bulletDespawnTime > 0)
            bulletDespawnTime -= bulletDespawnRate; 
            else
            {
                Destroy(gameObject);
            }
        }
    }

}
