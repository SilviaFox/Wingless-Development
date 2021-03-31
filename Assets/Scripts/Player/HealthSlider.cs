using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    Slider healthSlider;
    PlayerHealth healthScript;

    float health;

    private void Awake()
    {
        healthSlider = GetComponent<Slider>();
        healthScript = GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        health = healthScript.visualHealth;

        healthSlider.value = health;
    }
}
