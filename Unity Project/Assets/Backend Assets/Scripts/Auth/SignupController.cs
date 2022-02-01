using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SignupController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject signupMenu;

    public TMP_InputField username;
    public TMP_InputField password;
    public TMP_InputField confirmPassword;
    public TMP_InputField email;
    public Toggle terms;

    private AlertController alertController;

    private void Start()
    {
        alertController = AlertController.instance;
    }

    public void Signup()
    {
        if (HasErrors()) return;

        username.enabled = false;
        password.enabled = false;
        confirmPassword.enabled = false;
        email.enabled = false;
        terms.enabled = false;
        LoadingController.instance.EnableLoading();

        Action<bool, string> callback = new Action<bool,string>(SignupDone);

        StartCoroutine(AuthActions.CreateAccount(username.text, password.text, email.text, callback));
    }

    public void SignupDone(bool success, string errorMessage)
    {
        username.enabled = true;
        password.enabled = true;
        confirmPassword.enabled = true;
        email.enabled = true;
        terms.enabled = true;
        LoadingController.instance.DisableLoading();

        if (success)
        {
            alertController.CreateAlert(ConstantsBackend.SuccessMessages.Signup, true);
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
        if (username.text == "" || password.text == "" || confirmPassword.text == "" || email.text == "")
        {
            alertController.CreateAlert(ConstantsBackend.ErrorMessages.MandatoryFields);
            return true;
        }
        else if (!Regex.IsMatch(email.text, ConstantsBackend.EmailPattern))
        {
            alertController.CreateAlert(ConstantsBackend.ErrorMessages.EmailFormat);
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
        else if (!terms.isOn)
        {
            alertController.CreateAlert(ConstantsBackend.ErrorMessages.TermsNotAccepted);
            return true;
        }
        return false;
    }

    public void OpenSingup()
    {
        signupMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void Back()
    {
        ResetFields();
        signupMenu.SetActive(false);
        mainMenu.SetActive(true);
    }


    private void ResetFields()
    {
        username.text = "";
        password.text = "";
        confirmPassword.text = "";
        email.text = "";
        terms.isOn = false;
    }
}
