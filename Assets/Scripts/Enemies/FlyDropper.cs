using UnityEngine;

public class FlyDropper : MonoBehaviour
{

    [SerializeField] float moveSpeed;
    [SerializeField] float patrolTime;

    float currentPatrolTime;
    bool wallDetection;

    [SerializeField] LayerMask mask;
    [SerializeField] LayerMask groundMask;
    [SerializeField] GameObject bomb;
    [SerializeField] GameObject secondPhase;
    [SerializeField] GameObject bombExplosion;
    [SerializeField] float bombDropDivider; // Divide the drop of the bomb

    Rigidbody2D rb2d;
    SpriteAnimator spriteAnimator;
    SpriteRenderer spriteRenderer;

    bool detectedPlayer = false;
    bool droppedBomb = false;
    int facingDirection = -1;

    private void Start()
    {
        currentPatrolTime = Time.time + currentPatrolTime;
        rb2d = GetComponent<Rigidbody2D>();
        spriteAnimator = GetComponent<SpriteAnimator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!detectedPlayer)
            detectedPlayer = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, mask);
        else
            PlayerDetected();
        

    }

    void FixedUpdate()
    {
        rb2d.AddForce(new Vector2(moveSpeed * facingDirection, 0));
        if (currentPatrolTime < Time.time || wallDetection)
        {
            facingDirection *= -1;
            currentPatrolTime = Time.time + patrolTime;
        }

        spriteRenderer.flipX = facingDirection == 1;

        

        wallDetection = Physics2D.Raycast(transform.position, new Vector2(facingDirection, 0), 0.5f, groundMask);
    }

    void PlayerDetected()
    {
        detectedPlayer = false;
        if (!droppedBomb)
        {
            spriteAnimator.ChangeAnimationState("FlyDropper_Dropped");
            droppedBomb = true;
            Instantiate(bomb, transform.position, transform.rotation).GetComponent<FallingExplosive>().horizontalForce = rb2d.velocity.x / bombDropDivider;
        }

        Invoke("NextPhase", 1f);

    }

    void NextPhase()
    {
        // Trigger the next phase, delete this object and replace it with the next
        Destroy(this.gameObject);
        // Instantiate the second enemy phase and set its health to the current health of the enemy
        Instantiate(secondPhase, transform.position, transform.rotation).GetComponent<EnemyLogic>().SetHealth(GetComponent<EnemyLogic>().health);
    }

    private void OnDestroy()
    {
        if (!droppedBomb)
            Instantiate(bombExplosion, transform.position, transform.rotation);

    }

}
