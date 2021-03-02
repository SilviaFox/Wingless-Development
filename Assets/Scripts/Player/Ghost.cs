using UnityEngine;

public class Ghost : MonoBehaviour
{

    [SerializeField] float ghostDelay; // Delay before next ghost is created
    private float ghostDelaySeconds;
    [SerializeField] GameObject ghost;
    [HideInInspector] public bool makeGhost = false; // If makeGhost is enabled, set in PlayerMovement
    [SerializeField] SpriteRenderer playerSprite; // Call spriterenderers from this and the player.
    [SerializeField] SpriteRenderer ghostSprite;

    // Start is called before the first frame update
    void Start()
    {
        ghostDelaySeconds = ghostDelay;
    }

    private void Update()
    {
        ghostSprite.sprite = playerSprite.sprite; // Change to player's sprite
        ghostSprite.flipX = playerSprite.flipX; // Flip X if player's sprite is flipped
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (makeGhost)
        {
        if (ghostDelaySeconds > 0)
        {
            ghostDelaySeconds -= Time.fixedDeltaTime;
        }
        else
        {
            //generate a ghost
            GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation); // Create ghost from prefab on the player's position
            ghostDelaySeconds = ghostDelay; // Reset ghost delay
            Destroy(currentGhost, 1f); // Destroy the current ghost

        }
        }
    }
}
