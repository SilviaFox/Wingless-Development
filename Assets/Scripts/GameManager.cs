using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;
    public GameObject pauseFirstObject;
    Shooting shootingScript;
    
    InputManager inputManager;

    private void Start()
    {
        shootingScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Shooting>();
        inputManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        Unpause();
    }

    public void Restart() {
            inputManager.DisableAll(); // Disable all current inputs so that they don't cause errors on load
            // Because of course thats an issue
            // Seriously, what is up with this new input system?
            // Like I get why you'd want it to call functions and stuff, but surely you could've done something to make it
            // At least function somewhat sensically
            // This doesn't feel optimized for the current scripting API at all, which is silly because its pushed out with
            // The current scripting API.
            // We won't even have node stuff out of beta until like... the end of the year.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    

    public void Pause() {
        if (pauseMenu != null) // TODO: test to see if we even need this with the fixes implemented for the other pause bugs.
        {
            pauseMenu.SetActive(true);
            pauseMenu.GetComponent<Animator>().Play("PauseMenu_FadeIn");
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
        
    }
}
