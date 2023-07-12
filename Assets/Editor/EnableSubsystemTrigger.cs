
using UnityEditor;
using UnityEngine;
namespace Editor {
public class EnableSubsystemTrigger : IRemoteDebugPanelWindowItem  {
	[InitializeOnLoadMethod] private static void OnDomainReload() { RemoteDebugPanelWindow.Items.Add(new EnableSubsystemTrigger()); }
	public void OnRender() {
	if (GUILayout.Button("EnableSubsystemTrigger")) {Debug.Log("Hello from EnableSubsystem ");}
	}
}
}
