#if UNITY_EDITOR

using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editor.CodeGeneration
{
    public abstract class CodeGenerator
    {
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
}

#endif