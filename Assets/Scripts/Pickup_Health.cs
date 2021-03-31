using UnityEngine;

public class Pickup_Health : MonoBehaviour
{
    [SerializeField] float health; // Health to add
    PlayerHealth healthScript; // Script that holds player's health values

    private void Start()
    {
        healthScript = GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && healthScript.health != healthScript.maxHealth) // If the player touches this
        {
            healthScript.StartCoroutine(healthScript.AddHealth(health));
            Destroy(this.gameObject); // Destroy this
        }
    }
}
