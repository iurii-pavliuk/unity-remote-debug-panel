using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;

namespace Editor
{
    public class RemoteDebugPanelWindow : EditorWindow
    {
        public static List<IRemoteDebugPanelWindowItem> Items = new List<IRemoteDebugPanelWindowItem>();
        
        private EditorConnection _editorConnection;
        private string _messageToSend = "";
        private bool _checkboxValue;

        [MenuItem("Tools/Remote Debug Panel")]
        public static void ShowWindow()
        {
            GetWindow<RemoteDebugPanelWindow>("Remote Debug Panel");
        }

        private void OnEnable()
        {
            _editorConnection = EditorConnection.instance;
        }
        
        private void OnDisable()
        {
            _editorConnection.DisconnectAll();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Remote Debug Panel", EditorStyles.boldLabel);
            foreach (var windowItem in Items)
            {
                windowItem.OnRender();
            }
            
            // _messageToSend = EditorGUILayout.TextField("Message to send:", _messageToSend);
            //
            // if (GUILayout.Button("Send Message"))
            // {
            //     SendMessageToPlayer(_messageToSend);
            // }
            //
            // _checkboxValue = EditorGUILayout.Toggle("Custom Checkbox", _checkboxValue);
        }

        private void SendMessageToPlayer(string message)
        {
            var messageData = System.Text.Encoding.ASCII.GetBytes(message);
            _editorConnection.Send(MessageTypes.MyCustomMessage, messageData);
        }
        
        private void OnMessageReceived(MessageEventArgs messageArgs)
        {
            string receivedMessage = System.Text.Encoding.ASCII.GetString(messageArgs.data);
            Debug.Log("Received message: " + receivedMessage);
        }
    }

    public interface IRemoteDebugPanelWindowItem
    {
        void OnRender();
    }
}