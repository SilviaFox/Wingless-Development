using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject firstSelected;
    InputMaster menuControls;

    // Start is called before the first frame update
    void Start()
    {
        menuControls = new InputMaster();
        menuControls.Menu.Enable();
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
