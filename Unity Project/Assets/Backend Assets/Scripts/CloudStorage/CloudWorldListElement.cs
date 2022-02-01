using TMPro;
using UnityEngine;

public class CloudWorldListElement : MonoBehaviour
{
    public TMP_Text worldNameText;
    string worldName = "";
    [HideInInspector]
    public CloudMenuController cloudWorldMenuController;

    public void Create(string name, CloudMenuController cmc)
    {
        cloudWorldMenuController = cmc;
        worldName = name;
        worldNameText.text = name;
    }

    public void DeleteWorld()
    {
        cloudWorldMenuController.DeleteWorld(worldName);
    }

    public void SelectWorld()
    {
        cloudWorldMenuController.SelectWorld(worldName);
    }

    public void MoveWorldToLocal()
    {
        cloudWorldMenuController.DownloadWorld(worldName);
    }
}
