using UnityEngine;

public class DetectPlayer : MonoBehaviour
{

    // Player Detection
    [SerializeField] float detectionRadius = 7f;
    [SerializeField] LayerMask mask;
    Collider2D detectionCircle;

    // Enemy script
    [SerializeField] EnemyScript enemyScript;

    // Player Position
    [HideInInspector] Transform playerTransform;
    [HideInInspector] Vector2 playerPosition;


    private void Start()
    {
        playerTransform = PlayerController.current.gameObject.transform;
    }

    void Update()
    {
        detectionCircle = Physics2D.OverlapCircle(transform.position, detectionRadius, mask);
        playerPosition = playerTransform.position;

        if (detectionCircle)
        {
            enemyScript.FaceDirectionOfPlayer(playerPosition);
            enemyScript.canSeePlayer = true;
        }
        else
        {
            enemyScript.canSeePlayer = false;
        }
    }
}
