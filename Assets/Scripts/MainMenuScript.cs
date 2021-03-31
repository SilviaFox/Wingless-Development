using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] string firstScene;

    [SerializeField] GameObject main, settings, mainFirstSelected, settingsFirstSelected;

    public void StartGame() {
        SceneManager.LoadScene(firstScene);
    }

    public void Settings() {
        main.SetActive(false);
        settings.SetActive(true);
        EventSystem.current.SetSelectedGameObject(settingsFirstSelected);
    }

    public void SettingsToMain() {
        main.SetActive(true);
        settings.SetActive(false);
        EventSystem.current.SetSelectedGameObject(mainFirstSelected);
    }

    public void Exit() {
        Application.Quit();
    }

}
