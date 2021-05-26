using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    [SerializeField] GameObject player, postProcessEffect;

    ObjectAudioManager audioManager; // Audio manager to play the heal sound

    [HideInInspector] public float health;
    [HideInInspector] public float visualHealth; 

    DeathScript deathScript; // When health is 0, trigger this script

    private void Start()
    {
        // Set to max health
        health = maxHealth;
        visualHealth = health;

        audioManager = PlayerController.playerAudioManager;
        deathScript = GameObject.FindObjectOfType<DeathScript>(); // Script for when the player dies
    }

    public IEnumerator AddHealth(float healingAmount)
    {
        audioManager.Play("Heal");
        postProcessEffect.SetActive(true);

        // Add healing amount to health
        health += healingAmount;
        // Cap health at max
        if (health > maxHealth)
            health = maxHealth;
        
        // Increase visual health amount
        for (int i = 0; i < healingAmount; i++)
        {

            if (visualHealth > maxHealth) // Health caps at max
                visualHealth = maxHealth;
            else
                visualHealth ++; // Add health by given amount
            
            yield return null;
        }

        // Disable post process so that it can be re-enabled by another health pickup
        Invoke("DisablePostProcess", .4f);
    }

    void DisablePostProcess()
    {
        postProcessEffect.SetActive(false);
    }

    public void RemoveHealth(float damage)
    {

        health -= damage;
        visualHealth -= damage; // Remove health by a given amount

        if (health <= 0) // Kill player
        {
            deathScript.Death();
            health = 0;
        }
    }

}
