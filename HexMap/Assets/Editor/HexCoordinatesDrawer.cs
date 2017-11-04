using UnityEngine;
using UnityEditor;


/// <summary>
/// This class is to show hex cube coordinates on the Unity Editor
/// </summary>
[CustomPropertyDrawer(typeof(HexCoordinates))]
public class HexCoordinatesDrawer : PropertyDrawer
{

    public override void OnGUI(
       Rect position, SerializedProperty property, GUIContent label
        ) {
        HexCoordinates coordinates = new HexCoordinates(
            property.FindPropertyRelative("_x").intValue,
            property.FindPropertyRelative("_z").intValue
        );
        position = EditorGUI.PrefixLabel(position, label);
        GUI.Label(position, coordinates.ToString());
    }

}


