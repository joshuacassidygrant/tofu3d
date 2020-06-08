using System;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class TofuConfigCreator
{
    public static string FolderName = "tofuConfig";
    public static string CoreFolder = "Assets/tofu3d/Core";
    public static string PluginsFolder = "Assets/tofu3d/Plugins";
    public static string ScriptsFolder = "Assets/Scripts";
    public static string EventsConfigFileName = "eventList.txt";

    [MenuItem("Assets/Generate Tofu3d Scripts", false, -1)]
    static void Create()
    {
        DeleteConfigFolder();
        string newFolderPath = CreateFolderAndAsmdef();

        GenerateEventsEnum(newFolderPath + "/EventKey.cs");


    }

    private static string CreateFolderAndAsmdef()
    {
        string guid = AssetDatabase.CreateFolder("Assets", FolderName);
        string newFolderPath = AssetDatabase.GUIDToAssetPath(guid);

        string asmPath = newFolderPath + "/TofuConfig.asmdef";

        string asmContent = @"
            {
	            ""name"": ""TofuConfig""
            }
        ";
        File.WriteAllText(asmPath, asmContent);
        AssetDatabase.ImportAsset(asmPath);
        return newFolderPath;
    }

    public static void GenerateEventsEnum(string path)
    {
        string[] eventListPaths = {CoreFolder + "/" + EventsConfigFileName, PluginsFolder + "/" + EventsConfigFileName, ScriptsFolder + "/" + EventsConfigFileName };

        StringBuilder sb = new StringBuilder();
        sb.Append(@"
namespace TofuConfig
{
    public enum EventKey
    {
");

        HashSet<string> eventKeySet = new HashSet<string>();

        foreach (string eventListPath in eventListPaths)
        {
            StreamReader input = new StreamReader(eventListPath);
            while (!input.EndOfStream)
            {
                string key = input.ReadLine();
                if (key == null) break;
                key = key.Replace("\n", "").Replace("\r", "");
                if (!LegalEnumCheck(key))
                {
                    Debug.LogWarning("Can't read enum name for events:" + key);
                } else if (eventKeySet.Contains(key))
                {
                    Debug.LogWarning("Ignoring duplicate event key value: " + key);
                }
                else
                {
                    eventKeySet.Add(key);
                    sb.Append(key);
                    sb.Append(",");
                    sb.Append(Environment.NewLine);
                }
            }
            input.Close();
        }
        

        sb.Append("}}");

        File.WriteAllText(path, sb.ToString());
        AssetDatabase.ImportAsset(path);
    }


    public static void DeleteConfigFolder()
    {
        AssetDatabase.DeleteAsset("Assets/" + FolderName);
    }

    public static bool LegalEnumCheck(string name)
    {
        if (name == "") return false;
        Regex rg = new Regex("[^a-zA-Z0-9_]");
        return !rg.IsMatch(name);
    }
}
