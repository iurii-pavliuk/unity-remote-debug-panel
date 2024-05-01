using System;

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