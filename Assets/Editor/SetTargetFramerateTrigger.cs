using UnityEditor;
using UnityEngine;
namespace Editor {
public class SetTargetFramerateTrigger : IRemoteDebugPanelWindowItem  {
private string _input = "";
	[InitializeOnLoadMethod] private static void OnDomainReload() { RemoteDebugPanelWindow.Items.Add(new SetTargetFramerateTrigger()); }
	public void OnRender() {
		GUILayout.Label("Set target framerate");
	_input = GUILayout.TextField(_input);
	if (GUILayout.Button("Set target framerate")) {Debug.Log("Hello from RemoteDebugPanelCallbacks.SetTargetFramerate ");RemoteDebugPanelCallbacks.SetTargetFramerate(int.Parse(_input));}
	}
}
}
