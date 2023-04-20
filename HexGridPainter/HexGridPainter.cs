using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HexGridPainter
{
    public class HexGridPainter : EditorWindow
    {
        private static bool _pressed = false;
        private static Object? _target;
        private static GameObject? _selectedGameObject;

        [MenuItem("Tools/Hex Grid Painter")]
        private static void Init()
        {
            var window = (HexGridPainter)GetWindow(typeof(HexGridPainter));
            window.titleContent.text = "Hex Grid Painter";
            window.maxSize = new Vector2(300, 145);
            window.minSize = window.maxSize;
            SceneView.duringSceneGui += OnSceneGUIDelegate;
            window.Show();
        }
        
        private void OnDestroy()
        {
            SceneView.duringSceneGui -= OnSceneGUIDelegate;
        }

        private static void OnSceneGUIDelegate(SceneView sceneView)
        {
            HandleUtility.Repaint();
            if (!_pressed) return;
            Paint();
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Grid Object", GUILayout.Width(100));
            _target = EditorGUILayout.ObjectField(_target, typeof(MonoBehaviour), false);
            GUILayout.EndHorizontal();

            EditorGUI.BeginChangeCheck();
            _pressed = GUILayout.Toggle(_pressed, "Paint", "Button");
            if (EditorGUI.EndChangeCheck())
            {
                _selectedGameObject = _pressed ? Selection.activeGameObject : null;
                Selection.activeGameObject = null;
            }

            GUILayout.EndVertical();
        }

        private static void Paint()
        {
            var selectedHex = Selection.activeGameObject;
            if (selectedHex == null || _selectedGameObject == null || selectedHex == _selectedGameObject) return;

            var obj = PrefabUtility.InstantiatePrefab(_target, _selectedGameObject.transform) as MonoBehaviour;
            if (obj != null) obj.transform.localPosition = selectedHex.transform.localPosition;

            DestroyImmediate(selectedHex);
            Selection.activeGameObject = null;
        }
    }
}
