using System;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace TofuCore.Editor.Setup
{

    public class TofuConfigCreator {
        public static string FolderName = "tofuConfig";
        public static string CoreFolder = "Assets/tofu3d/Core";
        public static string PluginsFolder = "Assets/tofu3d/Plugins";
        public static string ConfigFolder = "Assets/Config";
        public static string EventsConfigFileName = "eventList.txt";
        public static string ConfigKeyFileName = "configKeyList.txt";
        public static string ScriptsFolder = "Assets/Scripts";

        /**
         * Run this to create/regenerate a separate tofuconfig folder with an asmdef,
         * then federate all content from eventList.txt files under tofu3d/Core, tofu3d/Plugins, Scripts
         * into a single Events enum.
         */
        [MenuItem("Assets/Generate Tofu3d Scripts", false, -1)]
        static void Create() {
            DeleteConfigFolder();
            string newFolderPath = CreateFolderAndAsmdef();

            GenerateEnum(newFolderPath + "/EventKey.cs", "EventKey", EventsConfigFileName);
            GenerateEnum(newFolderPath + "/ConfigKey.cs", "ConfigKey", ConfigKeyFileName);
        }

        private static string CreateFolderAndAsmdef() {
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

        public static void GenerateEnum(string path, string enumName, string inputFileName) {
            string[] enumListPaths = { CoreFolder + "/" + inputFileName, PluginsFolder + "/" + inputFileName, ConfigFolder + "/" + inputFileName, ScriptsFolder + "/" + inputFileName };

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
namespace TofuConfig
{
    public enum
");
            sb.Append(enumName);
            sb.Append(@"
    {
");

            HashSet<string> enumKeySet = new HashSet<string>();

            foreach (string enumPath in enumListPaths) {
                if (!File.Exists(enumPath)) {
                    Debug.LogWarning($"No file {inputFileName} at {enumPath}, please include one to federate event lists.");
                }
                else
                {
                    StreamReader input = new StreamReader(enumPath);
                    while (!input.EndOfStream) {
                        string key = input.ReadLine();
                        if (key == null) break;
                        key = key.Replace("\n", "").Replace("\r", "");
                        Regex ignoreRegex = new Regex(@"(^$)|(\/\/\w*)");

                        if (ignoreRegex.IsMatch(key)) {
                            // Ignore whitespace and comments
                            Debug.Log("ignored " + key);
                        } else if (!LegalEnumCheck(key)) {
                            Debug.LogWarning($"Can't read enum name for {enumName}: {key}");
                        } else if (enumKeySet.Contains(key)) {
                            Debug.LogWarning($"Ignoring duplicate {enumName} key value: {key}");
                        } else {
                            enumKeySet.Add(key);
                            sb.Append(key);
                            sb.Append(",");
                            sb.Append(Environment.NewLine);
                        }
                    }
                    input.Close();
                }
            }


            sb.Append("}}");

            File.WriteAllText(path, sb.ToString());
            AssetDatabase.ImportAsset(path);
        }

        public static void DeleteConfigFolder() {
            AssetDatabase.DeleteAsset("Assets/" + FolderName);
        }

        public static bool LegalEnumCheck(string name) {
            if (name == "") return false;
            Regex rg = new Regex(@"[^a-zA-Z0-9_]");
            return !rg.IsMatch(name);
        }
    }

}
