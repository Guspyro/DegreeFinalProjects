using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SigninController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject signinMenu;

    public TMP_InputField username;
    public TMP_InputField password;
    public Toggle keepMeSigned;

    private AlertController alertController;

    private void Start()
    {
        alertController = AlertController.instance;
    }

    public void Signin()
    {
        if (HasErrors()) return;

        Action<bool, string> callback = new Action<bool, string>(SinginDone);
        username.enabled = false;
        password.enabled = false;
        keepMeSigned.enabled = false;
        LoadingController.instance.EnableLoading();

        StartCoroutine(AuthActions.Authenticate(username.text, password.text, callback));
    }

    public void SinginDone(bool success, string errorMessage)
    {
        username.enabled = true;
        password.enabled = true;
        keepMeSigned.enabled = true;
        LoadingController.instance.DisableLoading();

        if (success)
        {
            alertController.CreateAlert(ConstantsBackend.SuccessMessages.Signin, true);
            if (keepMeSigned.isOn) AuthStorage.StoreRefreshToken();

            Back();
        }
        else
        {
            Debug.Log(errorMessage);
            alertController.CreateAlert(errorMessage);
        }
    }

    public bool HasErrors()
    {
        if (username.text == "" || password.text == "")
        {
            alertController.CreateAlert(ConstantsBackend.ErrorMessages.MandatoryFields);
            return true;
        }
        else if (password.text.Length < 6 || password.text.Length > 255)
        {
            alertController.CreateAlert(ConstantsBackend.ErrorMessages.PasswordLength);
            return true;
        }
        return false;
    }

    public void OpenSignin()
    {
        signinMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void Back()
    {
        ResetFields();
        signinMenu.SetActive(false);
        mainMenu.SetActive(true);
    }


    private void ResetFields()
    {
        username.text = "";
        password.text = "";
        keepMeSigned.isOn = false;
    }
}
