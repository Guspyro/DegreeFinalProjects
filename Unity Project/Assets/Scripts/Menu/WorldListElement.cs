using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldListElement : MonoBehaviour
{
    public TMP_Text worldNameText;
    string worldName = "";
    [HideInInspector]
    public LoadWorldMenuController worldMenuController;

    public void Create(string name, LoadWorldMenuController wm)
    {
        worldMenuController = wm;
        worldName = name;
        worldNameText.text = name;
    }

    public void RemoveWorld()
    {
        worldMenuController.DeleteWorld(worldName);
        Destroy(gameObject);
    }

    public void SelectWorld()
    {
        worldMenuController.SelectWorld(worldName);
    }

    public void MoveWorldToCloud()
    {
        worldMenuController.UploadWorld(worldName);
    }
}
