using UnityEngine;

public class LoadingController : MonoBehaviour
{
    public static LoadingController instance;
    public GameObject loading;

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

    public void EnableLoading()
    {
        loading.SetActive(true);
    }

    public void DisableLoading()
    {
        loading.SetActive(false);
    }
}
