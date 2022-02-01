using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public void Resume()
    {
        GameController.instance.ResumeGame();
    }

    public void Save()
    {
        WorldController.instance.SaveWorld();
        AlertController.instance.CreateAlert(Constants.SuccessMessages.WorldSaved, true);
    }

    public void SaveAndExit()
    {
        WorldController.instance.SaveWorld();
        Resume();
        SceneManager.LoadScene("MainMenu");
    }

}