using TMPro;
using UnityEngine;
using System;

public class ChangePasswordController : MonoBehaviour
{
    public GameObject myAccountMenu;
    public GameObject changePasswordMenu;

    public TMP_InputField oldPassword;
    public TMP_InputField newPassword;
    public TMP_InputField confirmNewPassword;

    private AlertController alertController;

    private void Start()
    {
        alertController = AlertController.instance;
    }

    public void ChangePassword()
    {
        if (HasErrors()) return;

        oldPassword.enabled = false;
        newPassword.enabled = false;
        confirmNewPassword.enabled = false;
        LoadingController.instance.EnableLoading();

        Action<bool, string> callback = new Action<bool,string>(ChangePasswordDone);

        StartCoroutine(AuthActions.ChangePassword(AuthStorage.accessToken, oldPassword.text, newPassword.text, callback));
    }

    public void ChangePasswordDone(bool success, string errorMessage)
    {
        oldPassword.enabled = true;
        newPassword.enabled = true;
        confirmNewPassword.enabled = true;
        LoadingController.instance.DisableLoading();

        if (success)
        {
            alertController.CreateAlert(ConstantsBackend.SuccessMessages.ChangePassword, true);
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
        if (oldPassword.text == "" || newPassword.text == "")
        {
            alertController.CreateAlert(ConstantsBackend.ErrorMessages.MandatoryFields);
            return true;
        }
        else if (oldPassword.text.Length < 6 || newPassword.text.Length > 255)
        {
            alertController.CreateAlert(ConstantsBackend.ErrorMessages.PasswordLength);
            return true;
        }
        else if (oldPassword.text.Length < 6 || newPassword.text.Length > 255)
        {
            alertController.CreateAlert(ConstantsBackend.ErrorMessages.PasswordLength);
            return true;
        }
        else if (newPassword.text != confirmNewPassword.text)
        {
            alertController.CreateAlert(ConstantsBackend.ErrorMessages.PasswordMatch);
            return true;
        }

        return false;
    }

    public void Back()
    {
        ResetFields();
        changePasswordMenu.SetActive(false);
        myAccountMenu.SetActive(true);
    }


    private void ResetFields()
    {
        oldPassword.text = "";
        newPassword.text = "";
        confirmNewPassword.text = "";
    }
}
