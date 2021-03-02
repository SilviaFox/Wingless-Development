using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    [SerializeField] GameObject player;

    [HideInInspector] public float health; 

    DeathScript deathScript; // When health is 0, trigger this script

    private void Awake()
    {
        health = maxHealth;
        deathScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DeathScript>(); // Script for when the player dies
    }

    public void AddHealth(float healingAmount)
    {
        health += healingAmount; // Add health by given amount

        if (health >= maxHealth) // Health caps at 100
        {
            health = maxHealth;
        }
    }

    public void RemoveHealth(float damage)
    {

        health -= damage; // Remove health by a given amount

        if (health <= 0) // Kill player
        {
            deathScript.Death();
            health = 0;
        }
    }

}
