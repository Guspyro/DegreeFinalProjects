using TMPro;
using UnityEngine;

public class CreateWorldMenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject createWorldMenu;

    public TMP_InputField worldName;
    public TMP_InputField worldSeed;
    public TMP_Dropdown worldType;
    public TMP_Dropdown worldSize;

    public void CreateWorld()
    {
        bool validAttributes = WorldController.instance.SetWorldAttributes(worldName.text, worldSeed.text, GetType(worldType.value), GetSize(worldSize.value));
        if (validAttributes)
        {
            WorldController.instance.CreateWorld();
        }
    }

    public int GetType(int input)
    {
        switch (input)
        {
            case 0: return Constants.GenerationTypes.Final;
            case 1: return Constants.GenerationTypes.PerlinNoise;
            case 2: return Constants.GenerationTypes.CelularAutomata;
            case 3: return Constants.GenerationTypes.Templates;
            case 4: return Constants.GenerationTypes.RandomGeneration;
            default: return Constants.GenerationTypes.RandomGeneration;
        }
    }

    public int[] GetSize(int input)
    {
        switch (input)
        {
            case 0: return Constants.WorldSizes.Small;
            case 2: return Constants.WorldSizes.Wide;
            default: return Constants.WorldSizes.Large;
        }
    }

    public void Back()
    {
        ResetFields();
        createWorldMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    private void ResetFields()
    {
        worldName.text = "";
        worldSeed.text = "";
        worldType.value = 0;
        worldSize.value = 0;
    }

}