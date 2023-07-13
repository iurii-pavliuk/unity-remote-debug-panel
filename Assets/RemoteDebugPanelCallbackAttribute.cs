using System;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
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

public class RemoteDebugPanelCallbacks
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
                    Debug.Log($"Method: {methodInfo.Name}, attribute: {attribute.Info}");

                    var className = methodInfo.Name + "Trigger";
                    var methodName = $"{type.FullName}.{methodInfo.Name}" ;
                    var code = attribute.UIType switch
                    {
                        RemoteDebugPanelItemUIType.InputField => ItemsUITemplate.ItemWithInputField(className,
                            methodName, attribute.Info),
                        RemoteDebugPanelItemUIType.Checkbox | RemoteDebugPanelItemUIType.InputField => ItemsUITemplate
                            .ItemWithCheckboxAndInputField(className, methodName, attribute.Info),
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    // save st to a text file
                    var path = Application.dataPath + "/Editor/" + className + ".cs";
                    File.WriteAllText(path, code);
                }
            }
        }
        
        EditorUtility.RequestScriptReload();
    }
}

public abstract class ItemsUITemplate
{
    public static string ItemWithInputField(string className, string methodName, string attributeInfo)
    {
        var st = new StringBuilder();
        st.AppendLine("using UnityEditor;");
        st.AppendLine("using UnityEngine;");
        st.AppendLine("namespace Editor {");
        st.AppendLine($"public class {className} : IRemoteDebugPanelWindowItem " + " {");
        st.AppendLine($"private string _input = \"\";");
        st.AppendLine('\t' + $"[InitializeOnLoadMethod] private static void OnDomainReload() {{ RemoteDebugPanelWindow.Items.Add(new {className}()); }}");
        st.AppendLine('\t' + $"public void OnRender() {{");
        st.AppendLine('\t' + $"\t" + $"GUILayout.Label(\"{attributeInfo}\");");
        st.AppendLine('\t' + $"_input = GUILayout.TextField(_input);");
        st.AppendLine('\t' + $"if (GUILayout.Button(\"{attributeInfo}\")) {{" +
                      $"Debug.Log(\"Hello from {methodName} \");" +
                      $"{methodName}(int.Parse(_input));" +
                      $"}}");
        st.AppendLine('\t' + "}");
        st.AppendLine("}");
        st.AppendLine("}");
        
        return st.ToString();
    }
    
    public static string ItemWithCheckboxAndInputField(string className, string methodName, string attributeInfo)
    {
        var st = new StringBuilder();
        st.AppendLine("using UnityEditor;");
        st.AppendLine("using UnityEngine;");
        st.AppendLine("namespace Editor {");
        st.AppendLine($"public class {className} : IRemoteDebugPanelWindowItem " + " {");
        st.AppendLine($"private string _input = \"\";");
        st.AppendLine($"private bool _checkbox = false;");
        st.AppendLine('\t' + $"[InitializeOnLoadMethod] private static void OnDomainReload() {{ RemoteDebugPanelWindow.Items.Add(new {className}()); }}");
        st.AppendLine('\t' + $"public void OnRender() {{");
        st.AppendLine('\t' + $"\t" + $"GUILayout.Label(\"{attributeInfo}\");");
        st.AppendLine('\t' + $"_input = GUILayout.TextField(_input);");
        st.AppendLine('\t' + $"_checkbox = GUILayout.Toggle(_checkbox, \"Checkbox\");");
        st.AppendLine('\t' + $"if (GUILayout.Button(\"{attributeInfo}\")) {{" +
                      $"Debug.Log(\"Hello from {methodName} \");" +
                      $"{methodName}(_input, _checkbox);" +
                      $"}}");
        st.AppendLine('\t' + "}");
        st.AppendLine("}");
        st.AppendLine("}");
        
        return st.ToString();
    }
}


