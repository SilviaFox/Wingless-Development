using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScript : MonoBehaviour
{
    [SerializeField] GameObject deathScreen; // The part of the death screen ui
    [SerializeField] float restartWaitTime;

    Scene loadedLevel;

    private void Awake()
    {
        loadedLevel = SceneManager.GetActiveScene(); // Loaded level = the current level
    }

    public void Death()
    {
        deathScreen.SetActive(true); // Activate death screen animation
        Invoke("RestartScene", restartWaitTime); // wait for animation to finish before restarting
    }

    void RestartScene()
    {
        SceneManager.LoadScene(loadedLevel.buildIndex); // Load the current scene
    }
}
