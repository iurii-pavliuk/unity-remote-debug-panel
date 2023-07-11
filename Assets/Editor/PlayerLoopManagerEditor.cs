using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    [CustomEditor(typeof(PlayerLoopManager))]
    public class PlayerLoopManagerEditor : UnityEditor.Editor
    {
        public VisualTreeAsset uxml;
        
        public override VisualElement CreateInspectorGUI()
        {
            var myInspector = new VisualElement();
            myInspector.Add(new Label("This is a custom inspector"));
            uxml.CloneTree(myInspector);

            if (Application.isPlaying)
            {
                var thisTarget = this.target as PlayerLoopManager;

                foreach (var cluster in PlayerLoopData.SystemsClusters)
                {
                    var toggle = new Toggle(cluster.Key);
                    toggle.RegisterCallback<ClickEvent>(evt =>
                    {
                        if (thisTarget != null) thisTarget.EnableCluster(cluster.Key, ((Toggle)evt.currentTarget).value);
                    });
                    myInspector.Add(toggle);
                }
            }
            
            return myInspector;
        }
    }
    
    // [CustomPropertyDrawer(typeof(MySystem))]
    public class PlayerLoopSystemDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create a new VisualElement to be the root the property UI
            var container = new VisualElement();

            var popup = new UnityEngine.UIElements.PopupWindow();
            popup.text = "System";
            popup.Add(new PropertyField(property.FindPropertyRelative("name"), "Name"));
            popup.Add(new PropertyField(property.FindPropertyRelative("isEnabled"), "Is Enabled"));
            popup.Add(new PropertyField(property.FindPropertyRelative("subSystems"), "Sub-systems"));
            container.Add(popup);
            
            return container;
        }
    }
}