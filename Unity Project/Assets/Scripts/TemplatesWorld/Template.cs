using UnityEngine;

[CreateAssetMenu(fileName = "NewTemplate")]
public class Template : ScriptableObject
{
    public string type;
    [SerializeField]
    private TemplateLayout templateLayout;

    public char[,] template;

    public void Initialize()
    {
        template = templateLayout.getTemplate();
    }
}
