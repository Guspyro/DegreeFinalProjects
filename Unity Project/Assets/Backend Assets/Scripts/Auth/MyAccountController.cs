using UnityEngine;

public class MyAccountController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject myAccountMenu;
    public GameObject changePasswordMenu;

    public void OpenMyAccount()
    {
        myAccountMenu.SetActive(true);
        mainMenu.SetActive(false);
        changePasswordMenu.SetActive(false);
    }

    public void OpenChangePassword()
    {
        changePasswordMenu.SetActive(true);
        myAccountMenu.SetActive(false);
    }

    public void Back()
    {
        myAccountMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

}
