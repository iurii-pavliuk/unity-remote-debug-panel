using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class RemoteDebugPanelWindow : EditorWindow
    {
        private string _inputText = "";
        private Color _chosenColor = Color.white;

        [MenuItem("Tools/Remote debug panel")]
        public static void ShowWindow()
        {
            GetWindow<RemoteDebugPanelWindow>("Remote debug panel");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Remote debug panel", EditorStyles.boldLabel);

            _inputText = EditorGUILayout.TextField("Text Input", _inputText);
            _chosenColor = EditorGUILayout.ColorField("Color Picker", _chosenColor);

            if (GUILayout.Button("Print Text"))
            {
                Debug.Log("Input Text: " + _inputText);
            }

            if (GUILayout.Button("Change Background Color"))
            {
                Camera.main.backgroundColor = _chosenColor;
            }
        }
    }
}