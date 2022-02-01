using UnityEngine;

public class AlertController : MonoBehaviour
{
    public static AlertController instance;
    public Alert alert;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void CreateAlert(string message, bool isSuccess = false)
    {
        Alert newAlert = Instantiate(alert, transform);
        newAlert.SetText(message);
        if (isSuccess) newAlert.SetColor(Color.green);
    }

}
