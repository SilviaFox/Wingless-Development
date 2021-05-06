using UnityEngine;

public class FallingExplosive : MonoBehaviour
{
    [SerializeField] GameObject explosionObject;
    [HideInInspector] public float horizontalForce; // Set by enemy that drops it
    [SerializeField] float verticalForce = 5; // Initial jump upwards
    Rigidbody2D rb2d;

    private void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Invoke("AddDropForces", 0.0001f);
    }

    private void AddDropForces()
    {
        rb2d.AddForce(new Vector2(horizontalForce, verticalForce), ForceMode2D.Impulse); //On spawn, jump up a little
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Instantiate(explosionObject, transform.position, transform.rotation);
        Destroy(gameObject);
    }

}
