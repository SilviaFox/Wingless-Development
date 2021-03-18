using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] string firstScene;

    public void StartGame() {
        SceneManager.LoadScene(firstScene);
    }
}
