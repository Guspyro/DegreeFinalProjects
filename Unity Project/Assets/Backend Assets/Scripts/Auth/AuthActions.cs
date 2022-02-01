using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

using UnityEngine;
using UnityEngine.Networking;

public static class AuthActions
{
    public static IEnumerator CreateAccount(string newUsername, string newPassword, string newEmail, Action<bool, string> callback)
    {
        WWWForm signupBody = new WWWForm();
        signupBody.AddField("username", newUsername);
        signupBody.AddField("email", newEmail);
        signupBody.AddField("password", newPassword);

        UnityWebRequest request = UnityWebRequest.Post(ConstantsBackend.BaseUrlAuth + ConstantsBackend.UrlSignup, signupBody);
        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Dictionary<string, string> response = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.downloadHandler.text);
            if (request.responseCode == 200)
            {
                callback?.Invoke(true, null);
            }
            else
            {
                callback?.Invoke(false, response["message"]);
            }
        }
    }

    public static IEnumerator Authenticate(string username, string password, Action<bool, string> callback)
    {
        WWWForm signinBody = new WWWForm();
        signinBody.AddField("username", username);
        signinBody.AddField("password", password);

        UnityWebRequest request = UnityWebRequest.Post(ConstantsBackend.BaseUrlAuth+ConstantsBackend.UrlSignin, signinBody);
        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Dictionary<string, string> response = JsonConvert.DeserializeObject<Dictionary<string,string>>(request.downloadHandler.text);
            if(request.responseCode == 200)
            {
                AuthStorage.SetValues(response["Username"], response["AccessToken"], response["RefreshToken"], true);
                callback?.Invoke(true, null);
            }
            else
            {
                callback?.Invoke(false, response["message"]);
            }
        }
    }

    public static void Signout()
    {
        AuthStorage.RemoveValues();
    }

    public static IEnumerator RefreshAccessToken(string refreshToken)
    {
        WWWForm refreshAccessTokenBody = new WWWForm();
        refreshAccessTokenBody.AddField("refreshToken", refreshToken);

        UnityWebRequest request = UnityWebRequest.Post(ConstantsBackend.BaseUrlAuth + ConstantsBackend.UrlRefreshAccessToken, refreshAccessTokenBody);
        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Dictionary<string, string> response = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.downloadHandler.text);

            if (request.responseCode == 200)
            {
                AuthStorage.SetValues(response["Username"], response["AccessToken"], response.ContainsKey("RefreshToken") ? response["RefreshToken"]: refreshToken, true);
            }
            else
            {
                AuthStorage.RemoveValues();
            }
        }
    }

    public static IEnumerator ChangePassword(string accessToken, string oldPassword, string newPassword, Action<bool, string> callback)
    {
        WWWForm signinBody = new WWWForm();
        signinBody.AddField("accessToken", accessToken);
        signinBody.AddField("oldPassword", oldPassword);
        signinBody.AddField("newPassword", newPassword);

        UnityWebRequest request = UnityWebRequest.Post(ConstantsBackend.BaseUrlAuth + ConstantsBackend.UrlChangePassword, signinBody);
        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Dictionary<string, string> response = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.downloadHandler.text);
            if (request.responseCode == 200)
            {
                callback?.Invoke(true, null);
            }
            else
            {
                callback?.Invoke(false, response["message"]);
            }
        }
    }

    public static IEnumerator ForgotPassword(string username, Action<bool, string> callback)
    {
        WWWForm signinBody = new WWWForm();
        signinBody.AddField("username", username);

        UnityWebRequest request = UnityWebRequest.Post(ConstantsBackend.BaseUrlAuth + ConstantsBackend.UrlForgotPassword + "?confirm=false", signinBody);
        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Dictionary<string, string> response = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.downloadHandler.text);
            if (request.responseCode == 200)
            {
                callback?.Invoke(true, null);
            }
            else
            {
                callback?.Invoke(false, response["message"]);
            }
        }
    }

    public static IEnumerator ConfirmForgotPassword(string confirmationCode, string username, string password, Action<bool, string> callback)
    {
        WWWForm signinBody = new WWWForm();
        signinBody.AddField("confirmationCode", confirmationCode);
        signinBody.AddField("username", username);
        signinBody.AddField("password", password);

        UnityWebRequest request = UnityWebRequest.Post(ConstantsBackend.BaseUrlAuth + ConstantsBackend.UrlForgotPassword + "?confirm=true", signinBody);
        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Dictionary<string, string> response = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.downloadHandler.text);
            if (request.responseCode == 200)
            {
                callback?.Invoke(true, null);
            }
            else
            {
                callback?.Invoke(false, response["message"]);
            }
        }
    }
}
