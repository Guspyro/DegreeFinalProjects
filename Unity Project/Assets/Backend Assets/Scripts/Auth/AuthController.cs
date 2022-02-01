using TMPro;
using UnityEngine;

public class AuthController : MonoBehaviour
{
    public static AuthController instance;

    public GameObject authButtons;
    public GameObject authLogged;
    public TMP_Text usernameText;

    void Awake()
    {
        string refreshToken = PlayerPrefs.GetString("refreshToken");
        if (AuthStorage.isSignedIn)
        {
            SignedIn();
        }
        else if (refreshToken != "")
        {
            StartCoroutine(AuthActions.RefreshAccessToken(refreshToken));
        }
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        AuthStorage.signedinEvent.AddListener(SignedIn);
        AuthStorage.signedoutEvent.AddListener(SignedOut);
    }

    public void SignedIn()
    {
        ActivateSignedInUI(AuthStorage.username);
    }

    private void ActivateSignedInUI(string username)
    {
        authButtons.SetActive(false);
        authLogged.SetActive(true);
        usernameText.text = username;
    }

    public void SignOut()
    {
        AuthActions.Signout();
        CloudWorldStorage.ResetWorldList();
    }

    public void SignedOut()
    {
        authButtons.SetActive(true);
        authLogged.SetActive(false);
        usernameText.text = "";
    }
}
