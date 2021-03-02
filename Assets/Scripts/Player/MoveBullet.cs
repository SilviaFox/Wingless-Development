using UnityEngine;

public class MoveBullet : MonoBehaviour
{

    Vector2 startPosition; // Position where the bullet will start;
    Rigidbody2D rb2d; // Get the rigidbody for moving the bullet
    BulletCounter bulletCounter; // Counts the amount of active bullets;
    SpriteRenderer bulletRenderer;

    bool playerLeft; // If true, the player is moving left, determined by the sprite renderer
    bool bulletOffscreen; // If true, the bullet's despawn countdown should start;
    bool isOnWall;
    float direction = 1; // Direction value to change depending on players direction

    public float damage = 10f;
    [Space]
    [SerializeField] float bulletSpeed; // speed of bullets
    [SerializeField] float bulletDespawnTime = 0.5f; // time it takes for bullets to despawn
    [SerializeField] float bulletDespawnRate = 0.1f; // speed at which bullets will despawn

    void Awake() // Triggered when object is created
    {

        bulletCounter = GameObject.FindGameObjectWithTag("BulletManager").GetComponent<BulletCounter>();
        bulletRenderer = GetComponent<SpriteRenderer>();

        // Position

        // startPosition = GameObject.FindGameObjectWithTag("Player").transform.position; // Starts from player's position
        // transform.position = startPosition; // change position to player position when creating the object.

        // Movement

        rb2d = GetComponent<Rigidbody2D>(); // Rigidbody for moving the object
        playerLeft = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().flipX; // Get direction the player is facing
        isOnWall = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isOnWall;

        if (playerLeft)
            direction = -1; // bullet direction will be multiplied by -1 
        if (isOnWall)
            direction *= -1; // reverse bullet direction if on a wall
        if (direction == -1)
            bulletRenderer.flipX = true; // flip bullet sprite depending on direction

        Debug.Log(direction);


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

    // Update is called once per frame
    void FixedUpdate()
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
                bulletCounter.DecreaseBulletAmount();
            }
        }
    }
    
}
