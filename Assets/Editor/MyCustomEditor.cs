using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class MyCustomEditor : EditorWindow
    {
        public VisualTreeAsset uxml;
        
        [MenuItem("Tools/My Custom Editor")]
        public static void ShowMyEditor()
        {
            // This method is called when the user selects the menu item in the Editor
            EditorWindow wnd = GetWindow<MyCustomEditor>();
            wnd.titleContent = new GUIContent("My Custom Editor");
        }

        private void CreateGUI()
        {
            rootVisualElement.Add(uxml.CloneTree());
        }
    }
}



