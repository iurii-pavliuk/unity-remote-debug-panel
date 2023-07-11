using UnityEditor;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;

namespace Editor
{
    public class EditorConnectionWindow : EditorWindow
    {
        private EditorConnection _editorConnection;
        private string _messageToSend = "";
        private bool _checkboxValue;

        [MenuItem("Tools/Editor Connection Window")]
        public static void ShowWindow()
        {
            GetWindow<EditorConnectionWindow>("Editor Connection Window");
        }

        private void OnEnable()
        {
            _editorConnection = EditorConnection.instance;
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Editor Connection Window", EditorStyles.boldLabel);

            _messageToSend = EditorGUILayout.TextField("Message to send:", _messageToSend);

            if (GUILayout.Button("Send Message"))
            {
                SendMessageToPlayer(_messageToSend);
            }
            
            _checkboxValue = EditorGUILayout.Toggle("Custom Checkbox", _checkboxValue);
        }

        private void SendMessageToPlayer(string message)
        {
            var messageData = System.Text.Encoding.ASCII.GetBytes(message);
            _editorConnection.Send(MessageTypes.MyCustomMessage, messageData);
        }
    }
}