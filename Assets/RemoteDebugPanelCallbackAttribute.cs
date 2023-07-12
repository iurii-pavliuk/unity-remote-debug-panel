using System;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method)]
public class RemoteDebugPanelCallbackAttribute : System.Attribute
{
    public RemoteDebugPanelCallbackAttribute(string someString)
    {
        this.SomeString = someString;
    }

    public string SomeString { get; set; }
}

public class RemoteDebugPanelCallbacks
{
    [RemoteDebugPanelCallback("Set target framerate")]
    public void SetTargetFramerate(int fps)
    {
        Application.targetFrameRate = fps;
    }
    
    [RemoteDebugPanelCallback("Enable/disable Unity subsystem")]
    public void EnableSubsystem(string name, bool isEnabled)
    {
        Debug.Log($"EnableSubsystem: {name}, {isEnabled}");
    }
    
    [MenuItem("Tools/Generate code")]
    public static void GenerateCode()
    {
        // get methods with RemoteDebugPanelCallbackAttribute from current assembly
        var attributeType = typeof(RemoteDebugPanelCallbackAttribute);
        var assembly = Assembly.GetExecutingAssembly();
        
        foreach (var type in assembly.GetTypes())
        {
            foreach (var methodInfo in type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (methodInfo.GetCustomAttribute(attributeType) is RemoteDebugPanelCallbackAttribute attribute)
                {
                    Debug.Log($"Method: {methodInfo.Name}, attribute: {attribute.SomeString}");

                    var className = methodInfo.Name + "Trigger";
                    var st = new StringBuilder();
                    st.AppendLine("using UnityEditor;");
                    st.AppendLine("using UnityEngine;");
                    st.AppendLine("namespace Editor {");
                    st.AppendLine($"public class {className} : IRemoteDebugPanelWindowItem " + " {");
                    st.AppendLine('\t' + $"[InitializeOnLoadMethod] private static void OnDomainReload() {{ RemoteDebugPanelWindow.Items.Add(new {className}()); }}");
                    st.AppendLine('\t' + $"public void OnRender() {{");
                    st.AppendLine('\t' + $"if (GUILayout.Button(\"{className}\")) {{Debug.Log(\"Hello from {methodInfo.Name} \");}}");
                    st.AppendLine('\t' + "}");
                    st.AppendLine("}");
                    st.AppendLine("}");
                    
                    // save st to a text file
                    var path = Application.dataPath + "/Editor/" + className + ".cs";
                    File.WriteAllText(path, st.ToString());
                }
            }
        }
        
        EditorUtility.RequestScriptReload();
    }
}


