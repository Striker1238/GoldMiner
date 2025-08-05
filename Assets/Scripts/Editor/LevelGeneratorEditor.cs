#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelGenerator generator = (LevelGenerator)target;
        if (GUILayout.Button("🔁 Перегенерировать уровень"))
        {
            generator.StartLevelGeneration();
        }
    }
}
#endif
