using System.Text;

namespace Editor.CodeGeneration
{
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
}