using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudMenuController : MonoBehaviour
{

    public GameObject cloudWorldListContainer;
    public GameObject cloudWorldSelectElement;
    public GameObject loadingMessage;
    public GameObject notSignedinMessage;
    public ScrollRect rect;

    public LoadWorldMenuController localWorldMenuController;

    private AlertController alertController;

    private void Start()
    {
        alertController = AlertController.instance;
    }

    private void OnEnable()
    {
        AuthStorage.signedinEvent.AddListener(Signedin);
        AuthStorage.signedoutEvent.AddListener(Signedout);

        if (AuthStorage.accessToken != "" && CloudWorldStorage.allWorlds == null)
        {
            ListWorlds();
        }
        else if (AuthStorage.accessToken == "")
        {
            Signedout();
        }
        else
        {
            notSignedinMessage.SetActive(false);
            loadingMessage.SetActive(false);
            GenerateList();
        }
    }

    private void OnDisable()
    {
        AuthStorage.signedinEvent.RemoveListener(Signedin);
        AuthStorage.signedoutEvent.RemoveListener(Signedout);
    }

    private void Signedin()
    {
        ListWorlds();
    }

    private void Signedout()
    {
        CloudWorldActions.ResetWorldList();
        GenerateList();
        loadingMessage.SetActive(false);
        notSignedinMessage.SetActive(true);
    }

    private void ListWorlds()
    {
        loadingMessage.SetActive(true);
        notSignedinMessage.SetActive(false);

        Action<bool, string> callback = new Action<bool, string>(ListWorldsDone);
        StartCoroutine(CloudWorldActions.ListWorlds(AuthStorage.accessToken, callback));
    }

    public void ListWorldsDone(bool success, string errorMessage)
    {
        loadingMessage.SetActive(false);
        if (success)
        {
            GenerateList();
        }
        else
        {
            Debug.Log(errorMessage);
            alertController.CreateAlert(errorMessage);
        }
    }

    public void SelectWorld(string worldName)
    {
        loadingMessage.SetActive(true);

        Action<bool, string> callback = new Action<bool, string>(GetWorldDone);
        StartCoroutine(CloudWorldActions.GetWorld(AuthStorage.accessToken, worldName, callback));
    }

    public void GetWorldDone(bool success, string errorMessage)
    {
        loadingMessage.SetActive(false);
        if (success)
        {
            World loadedWorld = CloudWorldStorage.GetCloudWorldAsWorld();
            WorldController.instance.LoadWorld(loadedWorld);
        }
        else
        {
            Debug.Log(errorMessage);
            alertController.CreateAlert(errorMessage);
        }
    }

    public void DownloadWorld(string worldName)
    {
        loadingMessage.SetActive(true);
        localWorldMenuController.SetLoading(true);

        Action<bool, string> callback = new Action<bool, string>(DownloadWorldDone);
        StartCoroutine(CloudWorldActions.DownloadWorld(AuthStorage.accessToken, worldName, callback));
    }

    public void DownloadWorldDone(bool success, string message)
    {
        loadingMessage.SetActive(false);
        localWorldMenuController.SetLoading(false);

        if (success)
        {
            localWorldMenuController.ReloadList();
            GenerateList();
            alertController.CreateAlert(message, true);
        }
        else
        {
            Debug.Log(message);
            alertController.CreateAlert(message);
        }
    }

    public void UploadWorld(string worldName)
    {
        if (AuthStorage.accessToken == "")
        {
            alertController.CreateAlert(ConstantsBackend.ErrorMessages.NotSignedin);
            return;
        }
        else if (CloudWorldStorage.allWorlds.Count >= ConstantsBackend.MaxCloudWorlds)
        {
            alertController.CreateAlert(ConstantsBackend.ErrorMessages.MaxCloudWorldsReached);
            return;
        }
        loadingMessage.SetActive(true);
        localWorldMenuController.SetLoading(true);

        Action<bool, string> callback = new Action<bool, string>(UploadWorldDone);
        StartCoroutine(CloudWorldActions.UploadWorld(AuthStorage.accessToken, worldName, callback));
    }

    public void UploadWorldDone(bool success, string message)
    {
        loadingMessage.SetActive(false);
        localWorldMenuController.SetLoading(false);

        if (success)
        {
            GenerateList();
            localWorldMenuController.ReloadList();
            alertController.CreateAlert(message, true);
        }
        else
        {
            Debug.Log(message);
            alertController.CreateAlert(message);
        }
    }

    public void DeleteWorld(string worldName)
    {
        loadingMessage.SetActive(true);

        Action<bool, string> callback = new Action<bool, string>(DeleteWorldDone);
        StartCoroutine(CloudWorldActions.DeleteWorld(AuthStorage.accessToken, worldName, callback));
    }

    public void DeleteWorldDone(bool success, string message)
    {
        loadingMessage.SetActive(false);
        if (success)
        {
            GenerateList();
            alertController.CreateAlert(message, true);
        }
        else
        {
            Debug.Log(message);
            alertController.CreateAlert(message);
        }
    }

    public void GenerateList()
    {
        int rowSize = 70;
        foreach (Transform child in cloudWorldListContainer.transform)
        {
            Destroy(child.gameObject);
        }

        if (CloudWorldStorage.allWorlds == null) return;

        for (int i = 0; i < CloudWorldStorage.allWorlds.Count; i++)
        {
            GameObject newWorldElement = Instantiate(cloudWorldSelectElement, cloudWorldListContainer.transform);
            newWorldElement.transform.localPosition = new Vector3(0, -rowSize * i - rowSize, 0);

            newWorldElement.GetComponent<CloudWorldListElement>().Create(CloudWorldStorage.allWorlds[i], this);
        }

        RectTransform rect = cloudWorldListContainer.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, CloudWorldStorage.allWorlds.Count * rowSize + rowSize);
    }
}