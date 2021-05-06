using UnityEngine;

public class EnemyGetAffectedByForce : MonoBehaviour
{

    Rigidbody2D rb2d;

    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void PushEnemy(float xAttackForce, float yAttackForce, int direction)
    {
        // Force that is applied to enemy
        Vector2 force = new Vector2(xAttackForce * direction, yAttackForce);
        // Add force
        rb2d.AddForce(force + new Vector2(0.0f, -rb2d.velocity.y), ForceMode2D.Impulse);
    }
}
