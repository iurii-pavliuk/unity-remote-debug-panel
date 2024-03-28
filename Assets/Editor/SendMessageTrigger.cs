using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class SendMessageTrigger : IRemoteDebugPanelWindowItem
    {
        private string _input = "";

        [InitializeOnLoadMethod]
        private static void OnDomainReload()
        {
            RemoteDebugPanelWindow.Items.Add(new SendMessageTrigger());
        }

        public void OnRender()
        {
            GUILayout.Label("Send message");
            _input = GUILayout.TextField(_input);
            if (GUILayout.Button("Send message"))
            {
                Debug.Log("Hello from RemoteDebugPanelCallbacks.SendMessage ");
                RemoteDebugPanelCallbacks.SendMessage(_input);
            }
        }
    }
}