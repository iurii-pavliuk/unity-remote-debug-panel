using Editor;
using UnityEditor;
using UnityEngine;
namespace Editor {
public class SetTargetFramerateTrigger : IRemoteDebugPanelWindowItem  {
	[InitializeOnLoadMethod] private static void OnDomainReload() { RemoteDebugPanelWindow.Items.Add(new SetTargetFramerateTrigger()); }
	public void OnRender() {
	if (GUILayout.Button("SetTargetFramerateTrigger")) {Debug.Log("Hello from SetTargetFramerate ");}
	}
}
}
