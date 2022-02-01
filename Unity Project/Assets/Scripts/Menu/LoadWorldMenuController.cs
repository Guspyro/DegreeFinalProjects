using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadWorldMenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject loadWorldMenu;
    public GameObject loadingMessage;

    public CloudMenuController cloudWorldMenuController;

    public GameObject worldListContainer;
    public GameObject worldSelectElement;
    public ScrollRect rect;

    private List<string> allWorlds;

    private void OnEnable()
    {
        ReloadList();
    }

    public void ReloadList()
    {
        allWorlds = WorldIO.GetAllWorlds();
        GenerateList();
    }

    public void SelectWorld(string worldName)
    {
        WorldController.instance.LoadWorld(worldName);
    }

    public void UploadWorld(string worldName)
    {
        cloudWorldMenuController.UploadWorld(worldName);
    }

    public void DeleteWorld(string worldName)
    {
        WorldIO.RemoveWorld(worldName);
        allWorlds = WorldIO.GetAllWorlds();
        GenerateList();
    }

    public void GenerateList()
    {
        int rowSize = 70;
        foreach (Transform child in worldListContainer.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < allWorlds.Count; i++)
        {
            GameObject newWorldElement = Instantiate(worldSelectElement, worldListContainer.transform);
            newWorldElement.transform.localPosition = new Vector3(0, -rowSize * i - rowSize, 0);

            newWorldElement.GetComponent<WorldListElement>().Create(allWorlds[i], this);
        }

        RectTransform rect = worldListContainer.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, allWorlds.Count * rowSize + rowSize);
    }

    public void Back()
    {
        loadWorldMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void SetLoading(bool newLoading)
    {
        loadingMessage.SetActive(newLoading);
    }
}