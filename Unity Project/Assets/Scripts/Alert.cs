using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Alert : MonoBehaviour
{
    public static AlertController instance;
    public TMP_Text alert;

    void Start()
    {
        Destroy(gameObject, 5.0f);
    }

    public void Close()
    {
        Destroy(gameObject);
    }

    public void SetText(string text)
    {
        alert.text = text;
    }

    public void SetColor(Color color)
    {
        GetComponent<Image>().color = color;
    }
}
