#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TilemapLayers))]
public class TilemapLayersDrawer : PropertyDrawer
{
    private const float PADDING = 2f;
    private const float LABEL_HEIGHT = 16f;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing * 3;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        try
        {
            Rect headerRect = new Rect(
                position.x,
                position.y,
                position.width,
                EditorGUIUtility.singleLineHeight
            );

            EditorGUI.LabelField(headerRect, label, EditorStyles.boldLabel);

            // Высота одной строки
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float verticalSpacing = EditorGUIUtility.standardVerticalSpacing;

            // Позиция для первого ряда (подписи)
            float labelsY = position.y + lineHeight + verticalSpacing;
            // Позиция для второго ряда (поля)
            float fieldsY = labelsY + lineHeight + verticalSpacing;

            // Ширина каждого поля
            float fieldWidth = (position.width - PADDING * 4) / 3f;

            // Первое поле - Floor
            DrawTilemapField(
                new Rect(position.x + PADDING, labelsY, fieldWidth, lineHeight),
                new Rect(position.x + PADDING, fieldsY, fieldWidth, lineHeight),
                "Floor",
                property.FindPropertyRelative("floor")
            );

            // Второе поле - Walls
            DrawTilemapField(
                new Rect(position.x + PADDING + fieldWidth + PADDING, labelsY, fieldWidth, lineHeight),
                new Rect(position.x + PADDING + fieldWidth + PADDING, fieldsY, fieldWidth, lineHeight),
                "Walls",
                property.FindPropertyRelative("walls")
            );

            // Третье поле - Loot
            DrawTilemapField(
                new Rect(position.x + PADDING + (fieldWidth + PADDING) * 2, labelsY, fieldWidth, lineHeight),
                new Rect(position.x + PADDING + (fieldWidth + PADDING) * 2, fieldsY, fieldWidth, lineHeight),
                "Loot",
                property.FindPropertyRelative("loot")
            );
        }
        catch (System.Exception e)
        {
            // Отладочная информация
            Debug.LogWarning($"Error drawing TilemapLayers: {e.Message}");

            // Резервный вариант отрисовки
            Rect errorRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(errorRect, "Tilemap Layers (Error)");
        }
        finally
        {
            EditorGUI.EndProperty();
        }
    }

    private void DrawTilemapField(Rect labelRect, Rect fieldRect, string label, SerializedProperty property)
    {
        if (property != null)
        {
            // Рисуем подпись
            EditorGUI.LabelField(labelRect, label, EditorStyles.miniLabel);

            // Рисуем поле
            EditorGUI.PropertyField(fieldRect, property, GUIContent.none);
        }
        else
        {
            // Если свойство не найдено, показываем ошибку
            EditorGUI.LabelField(labelRect, $"{label} (Not Found)", EditorStyles.miniLabel);
            EditorGUI.LabelField(fieldRect, "Property missing", EditorStyles.helpBox);
        }
    }
}
#endif