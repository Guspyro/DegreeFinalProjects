using UnityEngine;
using UnityEngine.Events;

public static class AuthStorage
{
    public static string username = "";
    public static string accessToken = "";
    public static string refreshToken = "";
    public static bool isSignedIn = false;

    public static UnityEvent signedinEvent = new UnityEvent();
    public static UnityEvent signedoutEvent = new UnityEvent();

    public static void SetValues(string newUsername, string newAccessToken, string newRefreshToken, bool isNowSignedin)
    {
        accessToken = newAccessToken;
        username = newUsername;
        refreshToken = newRefreshToken;
        isSignedIn = isNowSignedin;
        if (isSignedIn) signedinEvent.Invoke();
        else signedoutEvent.Invoke();
    }

    public static void StoreRefreshToken()
    {
        PlayerPrefs.SetString("refreshToken", refreshToken);
    }

    public static void RemoveValues()
    {
        SetValues("", "", "", false);
        StoreRefreshToken();
    }

    public static void SetUsername(string newUsername)
    {
        username = newUsername;
        PlayerPrefs.SetString("username", username);
    }
}
