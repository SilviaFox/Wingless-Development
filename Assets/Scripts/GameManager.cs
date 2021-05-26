using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject healthBar;
    public GameObject pauseFirstObject;
    Shooting shootingScript;
    
    InputManager inputManager;

    private void Start()
    {
        shootingScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Shooting>();
        inputManager = FindObjectOfType<InputManager>();
        Unpause();
    }

    public void Restart() {
            inputManager.DisableAll(); // Disable all current inputs so that they don't cause errors on load
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    

    public void Pause() {
        if (pauseMenu != null) // TODO: test to see if we even need this with the fixes implemented for the other pause bugs.
        {
            pauseMenu.SetActive(true);
            pauseMenu.GetComponent<Animator>().Play("PauseMenu_FadeIn");
            healthBar.SetActive(false);
        }
        
        Time.timeScale = 0.0f;
        EventSystem.current.SetSelectedGameObject(pauseFirstObject);
    }

    public void Unpause() {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        inputManager.Unpause();
        shootingScript.OnUnpause();

        // Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        healthBar.SetActive(true);
        
    }

    public void Exit() {
        Time.timeScale = 1.0f;
        inputManager.DisableAll();
        SceneManager.LoadScene("MainMenu");
    }
}
