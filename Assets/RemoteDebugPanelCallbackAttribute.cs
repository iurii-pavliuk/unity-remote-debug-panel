using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method)]
public class RemoteDebugPanelCallbackAttribute : System.Attribute
{
    public RemoteDebugPanelCallbackAttribute(string info, RemoteDebugPanelItemUIType uiType)
    {
        this.Info = info;
        this.UIType = uiType;
    }

    public string Info { get; set; }
    public RemoteDebugPanelItemUIType UIType { get; set; }
}

[Flags]
public enum RemoteDebugPanelItemUIType
{
    InputField = 0x0,
    Checkbox = 0x1,
}

public abstract class RemoteDebugPanelCallbacks
{
    [RemoteDebugPanelCallback("Set target framerate", RemoteDebugPanelItemUIType.InputField)]
    public static void SetTargetFramerate(int fps)
    {
        Application.targetFrameRate = fps;
        Debug.Log($"Set frame: {fps}");
    }
    
    [RemoteDebugPanelCallback("Enable/disable Unity subsystem", RemoteDebugPanelItemUIType.Checkbox | RemoteDebugPanelItemUIType.InputField)]
    public static void EnableSubsystem(string name, bool isEnabled)
    {
        Debug.Log($"EnableSubsystem: {name}, {isEnabled}");
    }

    [RemoteDebugPanelCallback("Send message", RemoteDebugPanelItemUIType.InputField)]
    public static void SendMessage(string msg)
    {
        Debug.Log($"Message: {msg}");
    }
}