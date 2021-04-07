using UnityEngine;

public class CoinScript : MonoBehaviour
{

    Rigidbody2D rb2d;
    ObjectAudioManager sounds;

    Transform playerTransform;
    bool moving = false;
    

    [SerializeField] float timeForMove = 1f;
    float timeUntilMove;
    [SerializeField] float speed = 9f;
    [SerializeField] int maxRandomForce = 5;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sounds = GetComponent<ObjectAudioManager>();

        // Get random X and Y for forces
        int randX = Random.Range(-maxRandomForce, maxRandomForce + 1);
        int randY = Random.Range(-maxRandomForce, maxRandomForce + 1);

        float randTime = Random.Range(0, 5);

        // Set time until coins move on their own
        timeUntilMove = Time.time + timeForMove + (randTime / 10);

        // Add random force
        rb2d.AddForce(new Vector2(randX, randY), ForceMode2D.Impulse);
    }

    private void Update()
    {
        // Enable movement if time it up
        if (!moving && Time.time > timeUntilMove)
        {
            sounds.Play("Suck");
            rb2d.bodyType = RigidbodyType2D.Kinematic;
            moving = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (moving)
        {
                
            Vector3 newPos = Vector3.MoveTowards(transform.position, playerTransform.position, speed * Time.fixedDeltaTime);
            rb2d.MovePosition(newPos);
        }
    }

    
}
