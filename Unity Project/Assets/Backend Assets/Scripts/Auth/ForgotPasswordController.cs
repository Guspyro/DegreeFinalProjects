using System;
using TMPro;
using UnityEngine;

public class ForgotPasswordController : MonoBehaviour
{
    public GameObject signinMenu;
    public GameObject forgotPasswordMenu;
    public GameObject step1;
    public GameObject step2;

    public TMP_InputField username;

    public TMP_InputField confirmationCode;
    public TMP_InputField password;
    public TMP_InputField confirmPassword;

    string currentUsername = "";

    private AlertController alertController;

    private void Start()
    {
        alertController = AlertController.instance;
    }

    public void ForgotPassword()
    {
        if (Setp1HasErrors()) return;

        Action<bool, string> callback = new Action<bool, string>(ForgotPasswordDone);
        username.enabled = false;
        LoadingController.instance.EnableLoading();

        currentUsername = username.text;
        StartCoroutine(AuthActions.ForgotPassword(currentUsername, callback));
    }

    public void ForgotPasswordDone(bool success, string errorMessage)
    {
        username.enabled = true;
        LoadingController.instance.DisableLoading();

        if (success)
        {
            alertController.CreateAlert(ConstantsBackend.SuccessMessages.ForgotPassword, true);
            OpenConfirmForgotPassword();
        }
        else
        {
            currentUsername = "";
            Debug.Log(errorMessage);
            alertController.CreateAlert(errorMessage);
        }
    }

    public bool Setp1HasErrors()
    {
        if (username.text == "")
        {
            alertController.CreateAlert(ConstantsBackend.ErrorMessages.MandatoryFields);
            return true;
        }

        return false;
    }

    public void ConfirmForgotPassword()
    {
        if (Setp2HasErrors()) return;

        Action<bool, string> callback = new Action<bool, string>(ConfirmForgotPasswordDone);
        confirmationCode.enabled = false;
        password.enabled = false;
        confirmPassword.enabled = false;
        LoadingController.instance.EnableLoading();

        StartCoroutine(AuthActions.ConfirmForgotPassword(confirmationCode.text, currentUsername, password.text, callback));
    }

    public void ConfirmForgotPasswordDone(bool success, string errorMessage)
    {
        confirmationCode.enabled = true;
        password.enabled = true;
        confirmPassword.enabled = true;
        LoadingController.instance.DisableLoading();

        if (success)
        {
            alertController.CreateAlert(ConstantsBackend.SuccessMessages.ConfirmForgotPassword, true);
            BackToSingin();
        }
        else
        {
            Debug.Log(errorMessage);
            alertController.CreateAlert(errorMessage);
        }
    }

    public bool Setp2HasErrors()
    {
        if (confirmationCode.text == "" || password.text == "" || confirmPassword.text == "")
        {
            alertController.CreateAlert(ConstantsBackend.ErrorMessages.MandatoryFields);
            return true;
        }
        else if (password.text.Length < 6 || password.text.Length > 255)
        {
            alertController.CreateAlert(ConstantsBackend.ErrorMessages.PasswordLength);
            return true;
        }
        else if (password.text != confirmPassword.text)
        {
            alertController.CreateAlert(ConstantsBackend.ErrorMessages.PasswordMatch);
            return true;
        }

        return false;
    }

    public void OpenForgotPassword()
    {
        forgotPasswordMenu.SetActive(true);
        step1.SetActive(true);
        signinMenu.SetActive(false);
    }

    public void OpenConfirmForgotPassword()
    {
        step2.SetActive(true);
        step1.SetActive(false);
    }

    public void Back()
    {
        ResetFieldsStep1();
        step1.SetActive(false);
        forgotPasswordMenu.SetActive(false);
        signinMenu.SetActive(true);
    }


    private void ResetFieldsStep1()
    {
        username.text = "";
        currentUsername = "";
    }

    public void BackToStep1()
    {
        ResetFieldsStep2();
        step1.SetActive(true);
        step2.SetActive(false);
    }

    private void ResetFieldsStep2()
    {
        confirmationCode.text = "";
        password.text = "";
        confirmPassword.text = "";
        currentUsername = "";
    }

    public void BackToSingin()
    {
        ResetFieldsStep1();
        ResetFieldsStep2();
        step1.SetActive(false);
        step2.SetActive(false);
        forgotPasswordMenu.SetActive(false);
        signinMenu.SetActive(true);
    }

}
