using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject createWorldMenu;
    public GameObject loadWorldMenu;
    public GameObject signupMenu;
    public GameObject signinMenu;

    private void Start()
    {
        WorldController.instance.gameStarted = false;
    }

    public void OpenCreateWorldMenu()
    {
        mainMenu.SetActive(false);
        createWorldMenu.SetActive(true);
    }

    public void OpenLoadWorldMenu()
    {
        mainMenu.SetActive(false);
        loadWorldMenu.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }
}