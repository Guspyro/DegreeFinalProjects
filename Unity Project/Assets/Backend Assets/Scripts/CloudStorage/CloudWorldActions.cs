using System;
using Newtonsoft.Json;

using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public static class CloudWorldActions
{
    public static IEnumerator ListWorlds(string token, Action<bool, string> callback)
    {
        string url = ConstantsBackend.BaseUrlCloudStorage + ConstantsBackend.UrlWorlds + "?accessToken=" + token;
        UnityWebRequest request = UnityWebRequest.Get(url);
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
                string[] worlds = JsonConvert.DeserializeObject<string[]>(response["worlds"]);
                CloudWorldStorage.SetWorldList(new List<string>(worlds));

                callback?.Invoke(true, null);
            }
            else
            {
                callback?.Invoke(false, response["message"]);
            }
        }
    }

    public static IEnumerator GetWorld(string token, string worldName, Action<bool, string> callback)
    {
        string url = ConstantsBackend.BaseUrlCloudStorage + ConstantsBackend.UrlWorld + "?accessToken=" + token + "&worldName=" + worldName;
        UnityWebRequest request = UnityWebRequest.Get(url);
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
                byte[] world = Convert.FromBase64String(response["world"]);
                CloudWorldStorage.SetWorld(new MemoryStream(world));
                callback?.Invoke(true, null);
            }
            else
            {
                callback?.Invoke(false, response["message"]);
            }
        }
    }

    public static IEnumerator DownloadWorld(string token, string worldName, Action<bool, string> callback)
    {

        yield return GetWorld(token, worldName, null);

        WorldIO.SaveWorld(worldName, CloudWorldStorage.cloudWorld);
        yield return DeleteWorld(token, worldName, null);
        CloudWorldStorage.RemoveWorld();

        callback?.Invoke(true, ConstantsBackend.SuccessMessages.DownloadWorld);

    }

    public static IEnumerator UploadWorld(string token, string worldName, Action<bool, string> callback)
    {
        WWWForm uploadWorldBody = new WWWForm();
        string url = ConstantsBackend.BaseUrlCloudStorage + ConstantsBackend.UrlWorld + "?accessToken=" + token + "&worldName=" + worldName;

        UnityWebRequest request = UnityWebRequest.Put(url, Convert.ToBase64String(WorldIO.GetWorld(worldName)));
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
                WorldIO.RemoveWorld(worldName);
                CloudWorldStorage.AddWorldToList(worldName);
                callback?.Invoke(true, ConstantsBackend.SuccessMessages.UploadWorld);
            }
            else
            {
                callback?.Invoke(false, response["message"]);
            }
        }
    }

    public static IEnumerator DeleteWorld(string token, string worldName, Action<bool, string> callback)
    {
        string url = ConstantsBackend.BaseUrlCloudStorage + ConstantsBackend.UrlWorld + "?accessToken=" + token + "&worldName=" + worldName;
        UnityWebRequest request = UnityWebRequest.Delete(url);
        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            Debug.Log(request.error);
        }
        else
        {
            if (request.responseCode == 200)
            {
                CloudWorldStorage.RemoveWorldFromList(worldName);
                callback?.Invoke(true, ConstantsBackend.SuccessMessages.DeleteWorld);
            }
            else
            {
                callback?.Invoke(false, ConstantsBackend.ErrorMessages.DeleteWorld);
            }
        }
    }

    public static void ResetWorldList()
    {
        CloudWorldStorage.ResetWorldList();
    }
}
