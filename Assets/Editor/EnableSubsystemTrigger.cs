using UnityEditor;
using UnityEngine;
namespace Editor {
public class EnableSubsystemTrigger : IRemoteDebugPanelWindowItem  {
private string _input = "";
private bool _checkbox = false;
	[InitializeOnLoadMethod] private static void OnDomainReload() { RemoteDebugPanelWindow.Items.Add(new EnableSubsystemTrigger()); }
	public void OnRender() {
		GUILayout.Label("Enable/disable Unity subsystem");
	_input = GUILayout.TextField(_input);
	_checkbox = GUILayout.Toggle(_checkbox, "Checkbox");
	if (GUILayout.Button("Enable/disable Unity subsystem")) {Debug.Log("Hello from RemoteDebugPanelCallbacks.EnableSubsystem ");RemoteDebugPanelCallbacks.EnableSubsystem(_input, _checkbox);}
	}
}
}
