using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

[CustomPropertyDrawer(typeof(TilePalette))]
public class TilePaletteDrawer : PropertyDrawer
{
    const float TileSize = 32f;
    const float Padding = 4f;
    const float LabelHeight = 10f;

    // Уникальный ключ для хранения активного свойства в EditorGUIUtility
    private const string ActivePropertyKey = "TilePaletteDrawer_ActiveProperty";

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        const int mainRows = 3;
        const int innerRows = 2;
        const int headers = 2;

        float cellSize = TileSize + Padding * 2 + LabelHeight;
        float labelHeight = EditorGUIUtility.singleLineHeight;

        // Вычисляем общее количество строк:
        float totalHeight =
            labelHeight +                     // Название поля
            Padding +
            headers * (labelHeight + Padding) +
            mainRows * cellSize +
            innerRows * cellSize;

        return totalHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var topLeft = property.FindPropertyRelative("topLeftTile");
        var top = property.FindPropertyRelative("topTile");
        var topRight = property.FindPropertyRelative("topRightTile");

        var left = property.FindPropertyRelative("leftTile");
        var center = property.FindPropertyRelative("centerTile");
        var right = property.FindPropertyRelative("rightTile");

        var bottomLeft = property.FindPropertyRelative("bottomLeftTile");
        var bottom = property.FindPropertyRelative("bottomTile");
        var bottomRight = property.FindPropertyRelative("bottomRightTile");

        var innerTopLeft = property.FindPropertyRelative("topLeftInnerCornerTile");
        var innerTopRight = property.FindPropertyRelative("topRightInnerCornerTile");
        var innerBottomLeft = property.FindPropertyRelative("bottomLeftInnerCornerTile");
        var innerBottomRight = property.FindPropertyRelative("bottomRightInnerCornerTile");

        // Заголовок
        EditorGUI.LabelField(
            new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
            label
        );

        float y = position.y + EditorGUIUtility.singleLineHeight + Padding;

        float cellSize = TileSize + LabelHeight + Padding * 2;

        DrawRow(position, y, topLeft, top, topRight);
        y += cellSize;

        DrawRow(position, y, left, center, right);
        y += cellSize;

        DrawRow(position, y, bottomLeft, bottom, bottomRight);
        y += cellSize + Padding;

        DrawRow(position, y, innerTopLeft, innerTopRight);
        y += cellSize;

        DrawRow(position, y, innerBottomLeft, innerBottomRight);

        EditorGUI.EndProperty();
    }

    private void DrawRow(Rect position, float y, params SerializedProperty[] props)
    {
        float x = position.x;
        foreach (var prop in props)
        {
            Rect cellRect = new Rect(x, y, TileSize + Padding * 2, TileSize + LabelHeight + Padding * 2);
            DrawTilePreview(cellRect, prop);
            x += cellRect.width + Padding;
        }
    }

    private void DrawTilePreview(Rect rect, SerializedProperty property)
    {
        EditorGUI.BeginProperty(rect, GUIContent.none, property);

        var tileBase = property.objectReferenceValue as TileBase;

        Rect previewRect = new Rect(
            rect.x + Padding,
            rect.y + Padding,
            TileSize,
            TileSize
        );

        Rect labelRect = new Rect(
            rect.x + Padding,
            rect.y + Padding + TileSize,
            TileSize,
            LabelHeight
        );

        if (tileBase != null)
        {
            Texture2D preview = AssetPreview.GetAssetPreview(tileBase);
            if (preview != null)
            {
                GUI.DrawTexture(previewRect, preview, ScaleMode.ScaleToFit);
            }
            else
            {
                EditorGUI.HelpBox(previewRect, "Preview...", MessageType.None);
            }
        }
        else
        {
            EditorGUI.HelpBox(previewRect, "None", MessageType.None);
        }

        string label = tileBase != null ? tileBase.name : "None";
        var labelStyle = new GUIStyle(EditorStyles.label)
        {
            alignment = TextAnchor.UpperCenter,
            fontSize = 9
        };
        EditorGUI.LabelField(labelRect, label, labelStyle);

        // 👇 УНИКАЛЬНЫЙ Control ID
        int controlID = GUIUtility.GetControlID(FocusType.Passive);

        Event evt = Event.current;

        // Клик по полю – показать Object Picker
        if (evt.type == EventType.MouseDown && rect.Contains(evt.mousePosition))
        {
            EditorGUIUtility.ShowObjectPicker<TileBase>(tileBase, false, "", controlID);
            EditorPrefs.SetString(ActivePropertyKey, property.propertyPath);
            evt.Use();
        }

        // При выборе объекта из Object Picker
        if (evt.commandName == "ObjectSelectorUpdated" &&
            EditorGUIUtility.GetObjectPickerControlID() == controlID)
        {
            if (EditorPrefs.GetString(ActivePropertyKey) == property.propertyPath)
            {
                var selectedObject = EditorGUIUtility.GetObjectPickerObject();
                property.objectReferenceValue = selectedObject;
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        EditorGUI.EndProperty();
    }

}