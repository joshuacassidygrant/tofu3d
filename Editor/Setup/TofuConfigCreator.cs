using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class TofuConfigCreator
{
    public static string FolderName = "tofuConfig";
    public static string CoreFolder = "Assets/tofu3d/Core";
    public static string PluginsFolder = "Assets/tofu3d/Plugins";
    public static string ScriptsFolder = "Assets/Scripts";

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
        string[] eventListPaths = {CoreFolder + "/eventList", PluginsFolder + "/eventList", ScriptsFolder + "/eventList"};


        var content = @"
using System;

public enum EventKey {
";

        foreach (string eventListPath in eventListPaths)
        {
            StreamReader input = new StreamReader(eventListPath);
            while (!input.EndOfStream)
            {
                string line = input.ReadLine();
                if (line == null) break;
                line = line.Replace("\n", "").Replace("\r", "");
                if (!LegalEnumCheck(line))
                {
                    Debug.LogWarning("Can't read enum name for events:" + line);
                }
                else
                {
                    content += line + ",\n";
                }
            }
        }

        content += "}";

        File.WriteAllText(path, content);
        AssetDatabase.ImportAsset(path);
    }


    public static void DeleteConfigFolder()
    {
        AssetDatabase.DeleteAsset("Assets/" + FolderName);
    }

    public static bool LegalEnumCheck(string name)
    {
        Regex rg = new Regex("[^a-zA-Z0-9_]");
        return !rg.IsMatch(name);
    }
}
