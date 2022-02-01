using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TemplateLayout))]
public class Array2DEditor : PropertyDrawer
{
	int sizeX = 10;
	int sizeY = 10;
	int boxWidth = 20;
	int boxHeight = 20;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.PrefixLabel(position, label);
		Rect newposition = position;
		newposition.y += boxHeight;
		SerializedProperty data = property.FindPropertyRelative("rows");

		for (int j = 0; j < sizeX; j++)
		{
			SerializedProperty row = data.GetArrayElementAtIndex(j).FindPropertyRelative("row");
			newposition.height = boxHeight;
			newposition.width = boxWidth;
			if (row.arraySize != sizeX)
				row.arraySize = sizeX;
			for (int i = 0; i < sizeY; i++)
			{
				EditorGUI.PropertyField(newposition, row.GetArrayElementAtIndex(i), GUIContent.none);
				newposition.x += newposition.width;
			}

			newposition.x = position.x;
			newposition.y += boxHeight;
		}
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return boxHeight * sizeY + boxHeight;
	}
}
