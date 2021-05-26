using UnityEngine;

public class DeathScript : MonoBehaviour
{
    [SerializeField] GameObject deathScreen; // The part of the death screen ui
    [SerializeField] float restartWaitTime;

    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void Death()
    {
        deathScreen.SetActive(true); // Activate death screen animation
        Invoke("RestartScene", restartWaitTime); // wait for animation to finish before restarting
    }

    void RestartScene()
    {
        gameManager.Restart();
    }
}
