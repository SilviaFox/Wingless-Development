using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;
    Shooting shootingScript;

    private void Awake()
    {
        shootingScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Shooting>();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        pauseMenu.GetComponent<Animator>().Play("PauseMenu_FadeIn");
        Time.timeScale = 0.0f;
    }

    public void Unpause()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        shootingScript.OnUnpause();
    }
}
