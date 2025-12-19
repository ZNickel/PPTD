using log4net.Core;
using Source;
using UnityEngine;
using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(LevelGrid))]
    public class GridEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var grid = (LevelGrid)target;

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Create")) grid.CreateGrid();
            if (GUILayout.Button("Save")) grid.SaveGrid();
            if (GUILayout.Button("Load")) grid.LoadGrid();
            GUILayout.EndHorizontal();
        }
    }
}