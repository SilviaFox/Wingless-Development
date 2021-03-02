using UnityEngine;

public class GetGroundAngle : MonoBehaviour
{
    PlayerController playerController;

    float angle;
    [SerializeField] float groundCheckDistance = 0.0001f;
    [SerializeField] LayerMask mask;

    [SerializeField] Transform point1;
    [SerializeField] Transform point2;

    [SerializeField] Rigidbody2D rotationPoint;
    float roundingAmount = 10;

    Vector2 hitPoint1;
    Vector2 hitPoint2;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {

        if(playerController.isGrounded) // if grounded
        {

            // Cast rays down to detect ground
            RaycastHit2D hit1 = Physics2D.Raycast(point1.position, Vector2.down, groundCheckDistance, mask);
            RaycastHit2D hit2 = Physics2D.Raycast(point2.position, Vector2.down, groundCheckDistance, mask);
            
            if (hit1 && hit2) // if rays hit
            {
                // Get point rays hit
                hitPoint1 = hit1.point;
                hitPoint2 = hit2.point;
                // Get angle based on the point the rays hit
                angle = Mathf.Round((hitPoint1.y - hitPoint2.y) * roundingAmount) / roundingAmount * Mathf.Rad2Deg; // Get the difference between these points
            }
            else
                angle = 0.0f;
        }
        else
            angle = 0.0f;

        rotationPoint.rotation = angle;

        
        
    }
}
