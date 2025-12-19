using Source;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Way))]
    public class WayEditor : UnityEditor.Editor
    {
        private bool _capture;
        private Way _way;

        private void OnEnable()
        {
            _way = (Way)target;
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            _capture = false;
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear")) _way.Clear();
            if (GUILayout.Button("Reverse")) _way.Reverse();
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            if (GUILayout.Button($"{(_capture ? "Stop" : "Start")} Capture")) 
                _capture = !_capture;
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if (!_capture) return;

            var e = Event.current;

            if (e.type != EventType.MouseDown || e.button != 0) return;
            var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            var plane = new Plane(Vector3.forward, Vector3.zero);

            if (plane.Raycast(ray, out var dist))
            {
                var hit = ray.GetPoint(dist);
                hit.z = 0;
                hit.x = Mathf.Round(hit.x);
                hit.y = Mathf.Round(hit.y);

                Undo.RecordObject(_way, "Add Point");
                _way.Add(hit);

                EditorUtility.SetDirty(_way);
            }

            e.Use();
        }
    }
}