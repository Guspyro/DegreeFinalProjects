using UnityEngine;

[System.Serializable]
public class TemplateLayout
{
    private static int size = 10;
    [System.Serializable]
    public struct rowData
    {
        public char[] row;
    }

    public rowData[] rows = new rowData[size];

    public char[,] getTemplate()
    {
        char[,] layout = new char[size,size];
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                layout[col, size - 1 - row] = rows[row].row[col];
            }
        }
        return layout;
    }
}